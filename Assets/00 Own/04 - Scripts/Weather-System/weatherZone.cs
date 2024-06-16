using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;
using Viguar.Aircraft;


namespace Viguar.WeatherDynamics
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(weatherCalculator))]
    public class weatherZone : MonoBehaviour
    {
        private Transform m_zone;
        private weatherController m_weather;
        private weatherCalculator m_weatherCalculator;
        private AircraftBaseProcessor _configBaseProcessor;
        
        public GameObject[] zClouds;
        public ParticleSystem[] zPrecipitationEmitters;

        public localWeatherConditions zoneWeather;
        public WindProperties windProperties;
        public PrecipitationProperties zonePrecipitation;
        public FogGenerationProperties zoneFog;       

        void Start()
        {
            configureWeatherZone();
            initWind();
            setLocalWeatherValues();
            m_weatherCalculator.generateGlobalWeather(zoneWeather.localSealevelTemperature, zoneWeather.localSealevelPressure, zoneWeather.localSealevelHumidity);
            generateVisualEffects();         
        }

        void FixedUpdate()
        {
            if (m_weather.m_dynamicWeather == true)
            {
                moveIntoDirection();
            }
        }

        private void Update()
        {
            controlWind();
        }

        private void configureWeatherZone()
        {
            gameObject.tag = "weatherZone";
            m_weather = GameObject.FindGameObjectWithTag("weatherController").GetComponent<weatherController>();
            m_weatherCalculator = GetComponent<weatherCalculator>();
            m_zone = GetComponent<Transform>();
            
            _configBaseProcessor = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();           
            m_zone.GetComponent<BoxCollider>().size = new Vector3(m_weather.m_weatherZoneSize, m_weather.m_weatherZoneGenerationDistance, m_weather.m_weatherZoneSize);

            if (Vector3.Distance(transform.position, _configBaseProcessor.transform.position) < m_weather.shortestDistance)
            {
                m_weather.shortestDistance = Vector3.Distance(transform.position, _configBaseProcessor.transform.position);
                UpdateAircraftEnvironmentInformation();
            }
        }
        private void setLocalWeatherValues()
        {
            //Depending on how big the randomizer value is, we allow the temperature to vary from temperature - maxRange  to temperature + maxRange
            int maxTempDifference = 10;
            int maxPressureDifference = 10;
            int maxHumidityDifference = 10;
            int maxWindDirDifference = 180;
            int maxWindStrengthDifference = 10;
            int maxWindTurbulenceDifference = 10;
            int maxGustDifference = 50;

            int maxRandomTemperature = Mathf.RoundToInt(m_weather.m_dynWeatherZoneRandomizer * maxTempDifference);
            int maxRandomPressure = Mathf.RoundToInt(m_weather.m_dynWeatherZoneRandomizer * maxPressureDifference);
            int maxRandomHumidity = Mathf.RoundToInt(m_weather.m_dynWeatherZoneRandomizer * maxHumidityDifference);

            int maxRandomWindDir = Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, m_weather.m_dynWeatherZoneRandomizer) * maxWindDirDifference);
            int maxRandomWindStrength = Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, m_weather.baseWindStrength) * maxWindStrengthDifference);
            int maxRandomWindTurbulence = Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, m_weather.baseWindTurbulenceStrength) * maxWindTurbulenceDifference);
            int maxRandomWindGust = Mathf.RoundToInt(Mathf.InverseLerp(0f, 1f, m_weather.baseWindGustStrength) * maxGustDifference);


            zoneWeather.localSealevelTemperature = Mathf.Clamp(Random.Range(m_weather.m_AirTemperature - maxRandomTemperature, m_weather.m_AirTemperature + maxRandomTemperature), -40, 60);
            zoneWeather.localSealevelPressure = Mathf.Clamp(Random.Range(m_weather.m_AirPressure - maxRandomPressure, m_weather.m_AirPressure + maxRandomPressure), 870, 1084);
            zoneWeather.localSealevelHumidity = Mathf.Clamp(Random.Range(m_weather.m_RelativeHumidity - maxRandomHumidity, m_weather.m_RelativeHumidity + maxRandomHumidity), 1, 100);

            windProperties.localBaseWindStrength = Random.Range(m_weather.baseWindStrength - maxRandomWindStrength, m_weather.baseWindStrength + maxRandomWindStrength);
            windProperties.localBaseTurbulenceStrength = Random.Range(m_weather.baseWindTurbulenceStrength - maxRandomWindTurbulence, m_weather.baseWindTurbulenceStrength + maxRandomWindTurbulence);
            windProperties.localBaseGustStrength = Random.Range(m_weather.baseWindGustStrength - maxRandomWindGust, m_weather.baseWindGustStrength + maxRandomWindGust);
        }
        public void UpdateAircraftEnvironmentInformation()
        {
            _configBaseProcessor._CurrentWeatherZone = GetComponent<weatherZone>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "aircraft")
            {
                UpdateAircraftEnvironmentInformation();
                turnOnPrecipitation();
            }
        }       
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "weatherController")
            {
                Destroy(gameObject);
            }
            if(other.tag == "aircraft")
            {
                turnOffPrecipitation();
            }
        }

        private void moveIntoDirection()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * m_weather.m_weatherZoneSpeed * 0.1f);
        }
       
        private void initWind()
        {
            windProperties.localWindGustsPerMinute = m_weather.windGustsPerMinute;
            windProperties.localWindGustDuration = m_weather.windGustsDurationSeconds;
            windProperties.localWindGustCurve = m_weather.windGustShape;

            windProperties.localWindGustOccurrenceThreshold = 60 / windProperties.localWindGustsPerMinute;
            windProperties.localWindGustTimer = 0;
            windProperties.localWindGustOccurrenceTimer = 0;
            windProperties.localCurrentWindGustStrength = 0;
        }
        private void controlWind()
        {
            calculateTurbulenceFluctuations();
            controlGusting();
            windProperties.localCurrentWindStrengthTotal = windProperties.localBaseWindStrength + (windProperties.localCurrentWindTurbulenceStrength * windProperties.localTurbulenceFluctuationRatio) + windProperties.localCurrentWindGustStrength;
        }
        private void calculateTurbulenceFluctuations()
        {
            windProperties.localCurrentWindTurbulenceStrength = Random.Range(-windProperties.localBaseTurbulenceStrength, windProperties.localBaseTurbulenceStrength);
            windProperties.localTurbulenceFluctuationRatio = Mathf.InverseLerp(-windProperties.localBaseTurbulenceStrength, windProperties.localBaseTurbulenceStrength, windProperties.localCurrentWindTurbulenceStrength);
        }
        private void controlGusting()
        {
            runGustingTimer();
            calculateWindGusting();
        }
        private void runGustingTimer()
        {
            windProperties.localWindGustOccurrenceTimer += Time.deltaTime;
            if(windProperties.localWindGustOccurrenceTimer > windProperties.localWindGustOccurrenceThreshold)
            {
                if (!windProperties.localWindGustOccurring) { windProperties.localWindGustOccurring = true; }
                windProperties.localWindGustOccurrenceTimer = 0;
            }
        }
        private void calculateWindGusting()
        {
            if(windProperties.localWindGustOccurring)
            {
                windProperties.localWindGustTimer += Time.deltaTime;
                if(windProperties.localWindGustTimer < windProperties.localWindGustDuration)
                {
                    windProperties.localCurrentWindGustStrength = windProperties.localBaseGustStrength * windProperties.localWindGustCurve.Evaluate(Mathf.InverseLerp(0 , windProperties.localWindGustDuration, windProperties.localWindGustTimer));
                }
                else
                {
                    windProperties.localWindGustTimer = 0;
                    windProperties.localWindGustOccurring = false;
                    windProperties.localCurrentWindGustStrength = 0;
                }
            }
        }


        private void generateVisualEffects()
        {
            generateClouds();
            generateFog();
            generateParticles();
            generatePrecipitation();
        }
        private void generateClouds()
        {
            if (m_weather.m_generateCloudFormation)
            {
                if (m_weatherCalculator.ZoneCloudsPossible)
                {
                    initialiseCloudArray();
                    spawnClouds();
                    randomizeClouds();
                }
            }
        }
        private void generatePrecipitation()
        {
            if (m_weather.m_generatePrecipitation)
            {
                if (m_weatherCalculator.ZonePrecipitationPossible)
                {
                    initialisePrecipitation();
                    createPrecipitation();
                }
            }
        }
        private void generateFog()
        {
            if (m_weather.m_generateFog)
            {
                if (m_weatherCalculator.ZoneFogPossible)
                {
                    initialiseFog();
                    calculateFogProperties();
                    zoneFog.zoneVolume.profile.TryGet<Fog>(out var fog);
                    if (fog)
                    {
                        fog.meanFreePath.value = zoneFog.visibilityDistance;
                        fog.depthExtent.value = zoneFog.volumetricFogDistance;
                        fog.baseHeight.value = zoneFog.denseFogHeight;
                        fog.maximumHeight.value = zoneFog.maxFogHeight;
                    }
                }
            }
        }
        private void generateParticles()
        {
            if (m_weather.m_generateParticles)
            {

            }
        }


        void initialiseCloudArray()
        {
            int maxCloudAmountDifference = Mathf.RoundToInt(m_weather.m_maxCloudsPerZone * m_weather.m_dynWeatherZoneRandomizer / 2);
            zoneWeather.localCloudAmount = Random.Range(m_weather.m_maxCloudsPerZone - maxCloudAmountDifference, m_weather.m_maxCloudsPerZone);
            zClouds = new GameObject[zoneWeather.localCloudAmount];
        }
        void spawnClouds()
        {
            for (int i = 0; i <= zoneWeather.localCloudAmount -1; i++)
            {

                    GameObject randomCloud;
                    randomCloud = m_weather.LowLevelCloudPrefabs[Random.Range(0, m_weather.LowLevelCloudPrefabs.Length)];
                    zClouds[i] = Instantiate(randomCloud, transform.position, Quaternion.identity);
                               
            }                                           
        }
        void randomizeClouds()
        {
            foreach(GameObject cloud in zClouds)
            {
                int maxAltitudeDifference = Mathf.RoundToInt(m_weather.m_midLevelCloudsAltitude * 0.5f * m_weather.m_dynWeatherZoneRandomizer);
                int spawnAltitude = Random.Range(m_weather.m_midLevelCloudsAltitude - maxAltitudeDifference, m_weather.m_midLevelCloudsAltitude + maxAltitudeDifference);
                float cloudPartitionSize = GetComponent<BoxCollider>().size.x / (zClouds.Length + 1);
                float randomCloudSizeMultiplier = Random.Range(cloudPartitionSize - cloudPartitionSize * m_weather.m_dynWeatherZoneRandomizer, cloudPartitionSize + cloudPartitionSize * m_weather.m_dynWeatherZoneRandomizer);
                Vector3 spawnPoint = new Vector3(Random.Range(GetComponent<BoxCollider>().size.x * -0.5f, GetComponent<BoxCollider>().size.x * 0.5f), spawnAltitude, Random.Range(GetComponent<BoxCollider>().size.z * -0.5f, GetComponent<BoxCollider>().size.z * 0.5f));
                Vector3 CloudSize = new Vector3(GetComponent<weatherCalculator>().GlobalCloudIntensity, GetComponent<weatherCalculator>().GlobalCloudIntensity, GetComponent<weatherCalculator>().GlobalCloudIntensity);
                cloud.transform.parent = transform;
                cloud.transform.localPosition = spawnPoint;
                cloud.transform.localScale = CloudSize * randomCloudSizeMultiplier;
            }

            
        }

        void initialiseFog()
        {
            zoneFog.zoneVolume = GetComponent<Volume>();
            VolumeProfile zoneProfile = new VolumeProfile();
            zoneFog.zoneVolume.profile = zoneProfile;

            Fog zFog = zoneProfile.Add<Fog>(zoneProfile);
            zFog.active = true;
            zFog.SetAllOverridesTo(false);
            zFog.enabled.overrideState = true;
            zFog.meanFreePath.overrideState = true;
            zFog.baseHeight.overrideState = true;
            zFog.maximumHeight.overrideState = true;
            zFog.enableVolumetricFog.overrideState = true;
            zFog.depthExtent.overrideState = true;

            zFog.enabled.value = true;
            zFog.enableVolumetricFog.value = true;
        }
        void calculateFogProperties()
        {
            zoneFog.visibilityDistance = Mathf.RoundToInt((int) (Math.Sqrt(10 * (m_weatherCalculator.GlobalFogIntensity * 10) + 1) * (10 * Math.Sqrt(zoneWeather.localSealevelHumidity + 1) * Math.Sqrt((zoneWeather.localSealevelTemperature + 273) / 273))));
            zoneFog.volumetricFogDistance = Mathf.RoundToInt(zoneFog.visibilityDistance / 10);
            zoneFog.denseFogHeight = Mathf.RoundToInt(m_weatherCalculator.GlobalFogIntensity * 0.75f * m_weather.m_midLevelCloudsAltitude / 100);
            zoneFog.denseFogHeight = 0;
            zoneFog.maxFogHeight = Mathf.RoundToInt(m_weather.m_midLevelCloudsAltitude);
        }

        void initialisePrecipitation()
        {
            zonePrecipitation.zPrecipitationType = m_weatherCalculator.GlobalPrecipitationType;
            zPrecipitationEmitters = new ParticleSystem[zoneWeather.localCloudAmount];
        }
        void createPrecipitation()
        {
            int i = 0;
            foreach (PrecipitationConfig config in m_weather.pConfig)
            {                
                if (config.precipitation == zonePrecipitation.zPrecipitationType)
                {                    
                    foreach (GameObject cInfo in zClouds)
                    {
                        GameObject precipitationEmitter;
                        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
                        float averageSize = (cInfo.transform.localScale.x + cInfo.transform.localScale.y + cInfo.transform.localScale.z) / 3;
                        precipitationEmitter = Instantiate(config.precipitationPrefab, cInfo.transform.localPosition, spawnRotation);
                        precipitationEmitter.transform.parent = cInfo.transform;
                        precipitationEmitter.transform.localScale = Vector3.one;
                        precipitationEmitter.transform.localPosition = Vector3.zero;

                        var particleSystemMain = precipitationEmitter.GetComponent<ParticleSystem>().main;
                        var particleSystemShape = precipitationEmitter.GetComponent<ParticleSystem>().shape;
                        particleSystemShape.length = cInfo.transform.localPosition.y / cInfo.transform.localScale.y;
                        particleSystemShape.radius = 0.5f;
                        particleSystemMain.maxParticles = 1;
                        particleSystemMain.maxParticles = Mathf.RoundToInt(averageSize * config.particlesPerUnit);
                        zPrecipitationEmitters[i] = precipitationEmitter.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
            }
        }
        void turnOffPrecipitation()
        {
            foreach (ParticleSystem emitter in zPrecipitationEmitters)
            {
                emitter.Stop();
            }
        }
        void turnOnPrecipitation()
        {
            foreach (ParticleSystem emitter in zPrecipitationEmitters)
            {
                emitter.Play();
            }
        }
    }

    [Serializable]
    public class localWeatherConditions
    {
        public int localSealevelTemperature;     //The weather zone's air temperature at sea level.
        public int localSealevelPressure;        //The weather zone's atmospheric pressure at sea level.
        public int localSealevelHumidity;        //The weather zone's relative humidity at sea level.

        public int localCloudAmount;
    }

    [Serializable]
    public class FogGenerationProperties
    {
        public Volume zoneVolume;
        public int visibilityDistance;
        public int denseFogHeight;
        public int maxFogHeight;
        public int volumetricFogDistance;
    }

    [Serializable]
    public class PrecipitationProperties
    {
        public weatherCalculator.PrecipitationTypes zPrecipitationType;        
    }

    [Serializable]
    public class WindProperties
    {
        public float localBaseWindStrength;
        public float localBaseTurbulenceStrength;
        public float localBaseGustStrength;

        public float localCurrentWindStrengthTotal;
        public float localCurrentWindTurbulenceStrength;
        public float localTurbulenceFluctuationRatio;
        public float localCurrentWindGustStrength;

        public float localWindGustsPerMinute;
        public float localWindGustDuration;

        public float localWindGustTimer;
        public float localWindGustOccurrenceTimer;
        public float localWindGustOccurrenceThreshold;
        public bool localWindGustOccurring;
        public AnimationCurve localWindGustCurve;
    }
}