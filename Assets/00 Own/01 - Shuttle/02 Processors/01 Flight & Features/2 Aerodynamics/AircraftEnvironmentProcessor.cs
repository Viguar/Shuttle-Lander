using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Viguar.WeatherDynamics;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    [RequireComponent(typeof(Rigidbody))]
    public class AircraftEnvironmentProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private ConfigEnvironmentImpact _cEnvironmentConfig;
        private Rigidbody _aircraftRigidbody;

        private float minDensity = 0.00909935413f;  //Lowest Possible Density of Air at Sea Level
        private float maxDensity = 0.01620037228f;  //Highest Possible Density of Air at Sea Level

        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();
            InitEnvironmentProcessor();
        }

        private void Update()
        {
            PerformEnvironmentCalculations();
        }

        public void PerformEnvironmentCalculations()
        {
            SetWeatherValues();
            ClampEnvironmentValues();
            CalculateAltitudeRates();
            if (_configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics) { ControlEfficiencies(); }
            if (_configBaseProcessor._EnvironmentPrecipitationAffectsAerodynamics) {  }
            if (_configBaseProcessor._EnvironmentWindAffectsAerodynamics) { ControlWindEffect(); }
        }

        private void ClampEnvironmentValues()
        {

        }
        private void CalculateAltitudeRates()
        {
            //Air Temperature drops by 1°C for every 100m of altitude in a linear fashion in 'good' weather conditions and by 0.6°C for every 100m of altitude when rainfall/snowfall occurs.. 
            //Relative Humidity drops approximately by 0.25% every 100m of altitude.
            //Base formula: AltitudeValue = SealevelValue - (Falloff * Altitude / 100)            
            _configBaseProcessor._AltitudeAirTemperature = _configBaseProcessor._CurrentSeaLevelAirTemperature - (_configBaseProcessor._AirtemperatureAltitudeFalloff * (_configBaseProcessor._AltitudeSeaLevel / 100));
            _configBaseProcessor._AltitudeRelativeHumidity = _configBaseProcessor._CurrentSeaLevelRelativeHumidity - (_configBaseProcessor._RelativeHumidityAltitudeFalloff * (_configBaseProcessor._AltitudeSeaLevel / 100));
            _configBaseProcessor._AltitudeWindStrength = _configBaseProcessor._CurrentSeaLevelWindStrength + (_configBaseProcessor._WindStrengthAltitudeIncrease * (_configBaseProcessor._AltitudeSeaLevel / 100));
            _configBaseProcessor._AltitudeAirPressure = _configBaseProcessor._CurrentSeaLevelAirPressure - (_configBaseProcessor._CurrentSeaLevelAirPressure * 0.1f * (_configBaseProcessor._AltitudeSeaLevel / 100));
        }
        private void CalculateAirDensity()
        {
            //We calculate the air density based on the sea level airpressure, altitude humidity and altitude temperature.
            //For reference, sea level air density at 15°C and 1013hPa is around 1225 kg/m3. 287 is the gas constant of dry air. Alt. Temperature "+ 283.15" is the °C to Kelvin convserion. 0.003f is a correction factor.
            //Humidity / 100 so that it's a decimal. 
            //Base formula (based on the real world model, not 100% realistic): Density = Pressure / (GasConstant * TemperatureKelvin * (1 - (CorrectionFactor * Humidity / 100))
            _configBaseProcessor._CurrentSeaLevelAirDensity = _configBaseProcessor._CurrentSeaLevelAirPressure / (287 * (_configBaseProcessor._CurrentSeaLevelAirTemperature + 273.15f) * (1 - (0.003f * _configBaseProcessor._CurrentSeaLevelRelativeHumidity / 100)));
            _configBaseProcessor._AltitudeAirDensity = _configBaseProcessor._AltitudeAirPressure / (287 * (_configBaseProcessor._AltitudeAirTemperature + 273.15f) * (1 - (0.003f * _configBaseProcessor._AltitudeRelativeHumidity / 100)));
        }
        private void CalculateEfficiencies()
        {
            //The efficiencies are 0 = 0% to 1 = 100%.
            _configBaseProcessor._AirTemperatureResponseEfficiency = _configBaseProcessor._AirTemperatureResponse.Evaluate(Mathf.InverseLerp(60, -40, _configBaseProcessor._AltitudeAirTemperature));
            _configBaseProcessor._AirDensityResponseEfficiency = _configBaseProcessor._AirDensityResponse.Evaluate(Mathf.InverseLerp(maxDensity, minDensity, _configBaseProcessor._AltitudeAirDensity));
            _configBaseProcessor._AltitudeResponseEfficiency = _configBaseProcessor._AltitudeResponse.Evaluate(Mathf.InverseLerp(1, 0, _configBaseProcessor._AltitudeRelative));
        }
        private void ControlEfficiencies()
        {
            CalculateAirDensity();
            CalculateEfficiencies();
            //Averaged Efficiency of all three
            _configBaseProcessor._LiftPotential = (_configBaseProcessor._AirTemperatureResponseEfficiency + _configBaseProcessor._AirDensityResponseEfficiency + _configBaseProcessor._AltitudeResponseEfficiency) / 3;
            _configBaseProcessor._DragPotential = (_configBaseProcessor._AirTemperatureResponseEfficiency + _configBaseProcessor._AirDensityResponseEfficiency + _configBaseProcessor._AltitudeResponseEfficiency) / 3;
            _configBaseProcessor._TorquePotential = (_configBaseProcessor._AirTemperatureResponseEfficiency + _configBaseProcessor._AirDensityResponseEfficiency + _configBaseProcessor._AltitudeResponseEfficiency) / 3;
            _configBaseProcessor._ThrustPotential = (_configBaseProcessor._AirTemperatureResponseEfficiency + _configBaseProcessor._AirDensityResponseEfficiency + _configBaseProcessor._AltitudeResponseEfficiency) / 3;
        }
         
        private void ControlWindEffect()
        {
            CalculateWindSpeed();
            CalculateWindForces();
        }
        private void CalculateWindForces()
        {
            //Get the angle between aircraft looking direction & wind direction, so we have an Angle of Attack relative to the Wind. 
            //If our AoA is < 90°, we're facing along with the wind direction, If the AoA > 90°, we're facing against the Wind. (Which might give us extra lift, because more air flows over the wings.) 
            //Therefore: The smaller the angle when against the wind, the bigger the "windlift".
            //Therefore: The faster we are when with the wind, the less "windforce" we apply.
            _configBaseProcessor._AltitudeWindStrength += _configBaseProcessor._AltitudeWindStrength * _configBaseProcessor._WindStrengthFactor;          
            Vector3 windDirection = _configBaseProcessor._CurrentWeatherZone.transform.forward;
            float windAoA = Vector3.Angle(transform.forward, windDirection.normalized);

            if(windAoA < 90)
            {
                //With the wind, less wind forces are exerted onto us due to the aircrafts aerodynamic nature if we align with the wind. The faster we are, the less effective the wind will be.
                float AoAIntensity = Mathf.InverseLerp(0, 90, windAoA);
                float WindPushIntensity = Mathf.InverseLerp(_configBaseProcessor._CurrentWindSpeed, 0, _configBaseProcessor._ForwardSpeed);
                Vector3 windForce = Vector3.zero;
                windForce += windDirection;
                windForce *= Mathf.InverseLerp(1, 0, Vector3.Dot(_aircraftRigidbody.velocity, windDirection * _configBaseProcessor._AltitudeWindStrength));
                windForce *= Mathf.InverseLerp(1, 0, Vector3.Dot(transform.forward, windDirection));
                windForce *= _configBaseProcessor._AltitudeWindStrength;
                _aircraftRigidbody.AddForce(windForce);
                _configBaseProcessor._WindLiftPotential = 0;
            }
            if (windAoA > 90)
            {
                //Against the wind, we get more lift the more we face into the wind, and get less wind forces exerted onto us due to the aircrafts aerodynamic nature.                
                float AoAIntensity = Mathf.InverseLerp(90, 180, windAoA);
                float WindPushIntensity = Mathf.InverseLerp(90, 0, 180 - windAoA);
                float WindLiftIntensity = Mathf.InverseLerp(_configBaseProcessor._MaximumLiftSpeed, _configBaseProcessor._CurrentWindSpeed, _configBaseProcessor._ForwardSpeed);
                Vector3 windForce = Vector3.zero;
                windForce += windDirection;
                windForce *= (WindPushIntensity + AoAIntensity) / 2;
                windForce *= _configBaseProcessor._AltitudeWindStrength;
                _aircraftRigidbody.AddForce(windForce);
                _configBaseProcessor._WindLiftPotential = WindLiftIntensity * AoAIntensity;
            }
        }
        private void CalculateWindSpeed()
        {
            var localVelocity = transform.InverseTransformDirection(_configBaseProcessor._CurrentWeatherZone.transform.forward * _configBaseProcessor._AltitudeWindStrength); 
            _configBaseProcessor._CurrentWindSpeed = Mathf.Max(0, localVelocity.z);
        }

        public void SetWeatherValues()
        {
            if (_configBaseProcessor._CurrentWeatherZone != null)
            {
                _configBaseProcessor._CurrentSeaLevelAirTemperature = _configBaseProcessor._CurrentWeatherZone.zoneWeather.localSealevelTemperature;
                _configBaseProcessor._CurrentSeaLevelAirPressure = _configBaseProcessor._CurrentWeatherZone.zoneWeather.localSealevelPressure;
                _configBaseProcessor._CurrentSeaLevelRelativeHumidity = _configBaseProcessor._CurrentWeatherZone.zoneWeather.localSealevelHumidity;

                _configBaseProcessor._CurrentSeaLevelWindStrength = _configBaseProcessor._CurrentWeatherZone.windProperties.localCurrentWindStrengthTotal;
            }
        }
        private void InitEnvironmentProcessor()
        {
            if(_configBaseProcessor._EnvironmentAffectsAerodynamics)
            {
                if(GameObject.FindGameObjectWithTag("weatherController") != null)
                {
                    _configBaseProcessor._GlobalWeatherController = GameObject.FindGameObjectWithTag("weatherController").GetComponent<weatherController>();   
                }
                else { AssignFallbackEnvironmentValues(); }                
            }
            else
            {
                _configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics = false;
                _configBaseProcessor._EnvironmentPrecipitationAffectsAerodynamics = false;
                _configBaseProcessor._EnvironmentWindAffectsAerodynamics = false;
            }
        }
             
        private void AssignFallbackEnvironmentValues()
        {
                _configBaseProcessor._CurrentSeaLevelAirTemperature = _cEnvironmentConfig._cFallbackSeaLevelAirTemperature;
                _configBaseProcessor._CurrentSeaLevelAirPressure = _cEnvironmentConfig._cFallbackSeaLevelAirPressure;
                _configBaseProcessor._CurrentSeaLevelRelativeHumidity = _cEnvironmentConfig._cFallbackSeaLevelRelativeHumidity;
                _configBaseProcessor._CurrentSeaLevelWindStrength = _cEnvironmentConfig._cFallbackSeaLevelWindStrength;
        }
    }
}
