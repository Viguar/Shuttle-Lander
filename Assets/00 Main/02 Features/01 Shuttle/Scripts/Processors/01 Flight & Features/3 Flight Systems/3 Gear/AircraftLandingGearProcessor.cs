using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftLandingGearProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private bool _animationLock = false;
        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            SetLandingGearType();
            CountLandingGearWheels();
        }

        public void PerformLandingGear(bool landingGearInput)
        {
            if(landingGearInput) { InitiateLandingGearSequence(); }
            CalculateLandingGearDrag();
            ControlLandingGearWheelBrakes();
            ControlLandingGearSteeringColumn();
            SetLandingGearState();
            ControlLandingGearBrakeTemperature();
        }
        #region Gear
        public void ToggleLandingGear() //Changes landing gear to true/false after animation has finished
        {
            if (_configBaseProcessor._LandingGearType == ConfigLandingGear._cLandingGearTypes.RetractableGear)
            {
                    _configBaseProcessor._LandingGearExtended = !_configBaseProcessor._LandingGearExtended;
                    _animationLock = false;
                    //SetLandingGearState();
            }
        }
        public void SetLandingGear(bool landingGearState)
        {
            if (_configBaseProcessor._LandingGearType == ConfigLandingGear._cLandingGearTypes.RetractableGear)
            {
                _configBaseProcessor._LandingGearExtended = landingGearState;
                SetLandingGearState();
            }
        }
        private void InitiateLandingGearSequence()
        {
            if(!_animationLock)
            {
                _animationLock = true;              
            }            
        }
        private void SetLandingGearState()
        {
            if (_animationLock) { _configBaseProcessor._LandingGearState = AircraftBaseProcessor.LandingGearStateTypes.Processing; }
            else if (_configBaseProcessor._LandingGearExtended && !_animationLock) { _configBaseProcessor._LandingGearState = AircraftBaseProcessor.LandingGearStateTypes.Extended; }
            else { _configBaseProcessor._LandingGearState = AircraftBaseProcessor.LandingGearStateTypes.Retracted; }
        }
        private void SetLandingGearType()
        {
            if(_configBaseProcessor._LandingGearType == ConfigLandingGear._cLandingGearTypes.RetractableGear) { _configBaseProcessor._LandingGearInducesDrag = true; }
            else { _configBaseProcessor._LandingGearInducesDrag = false; }
        }

        private void CalculateLandingGearDrag()
        {
            if(_configBaseProcessor._LandingGearExtended) { _configBaseProcessor._LandingGearDrag = _configBaseProcessor._LandingGearDragStart; }
            else { _configBaseProcessor._LandingGearDrag = 0; }
        }
        #endregion
        #region Wheels
        private void CountLandingGearWheels()
        {
            _configBaseProcessor._LandingGearWheels = GetComponentsInChildren<WheelCollider>();
            _configBaseProcessor._LandingGearWheelsAmount = _configBaseProcessor._LandingGearWheels.Length;
        }
        private void ControlLandingGearWheelBrakes()
        {
            if (_configBaseProcessor._StateGrounded)
            {
                foreach (WheelCollider landingGearWheel in _configBaseProcessor._LandingGearWheels)
                {
                    landingGearWheel.brakeTorque = (_configBaseProcessor._LeverAirbrakeSetting > 0 ? landingGearWheel.brakeTorque = _configBaseProcessor._LandingGearWheelBrakeResponse * 10 : landingGearWheel.brakeTorque = 0f);                    
                }                
            }            
        }
        private void ControlLandingGearBrakeTemperature()
        {
            if (_configBaseProcessor._StateGrounded && !_configBaseProcessor._StateStationary)
            {
                float tempIncreaseFactor = Mathf.InverseLerp(0, _configBaseProcessor._MinMaxStableForwardSpeed.x, _configBaseProcessor._ForwardSpeed);
                if (_configBaseProcessor._PedalWheelbrakeSetting > 0)
                {
                    _configBaseProcessor._LandingGearWheelBrakeTemperature += Time.deltaTime * _configBaseProcessor._WheelBrakeTemperatureIncreasePerSecond * _configBaseProcessor._LeverAirbrakeSetting * tempIncreaseFactor * _configBaseProcessor._PedalWheelbrakeSetting;
                }
            }
            if (_configBaseProcessor._PedalWheelbrakeSetting == 0 || _configBaseProcessor._StateStationary)
            {
                if(_configBaseProcessor._LandingGearWheelBrakeTemperature > _configBaseProcessor._AltitudeAirTemperature)
                {
                    _configBaseProcessor._LandingGearWheelBrakeTemperature -= Time.deltaTime * _configBaseProcessor._WheelBrakeTemperatureIncreasePerSecond * 0.25f;
                }
                else
                {
                    _configBaseProcessor._LandingGearWheelBrakeTemperature = _configBaseProcessor._AltitudeAirTemperature;
                }
            }            
        }
        private void ControlLandingGearBrakeFluidPressure()
        {

        }
        private void ControlLandingGearHydraulicPressure()
        {

        }
        private void ControlLandingGearTireStress()
        {

        }
        private void DefineLandingGearNeutralTireStressState()
        {

        }  
        private void ControlLandingGearSteeringColumn()
        {
            if (_configBaseProcessor._StateGrounded)
            {
                WheelCollider[] steeringWheels;
                steeringWheels = _configBaseProcessor._LandingGearSteeringColumn.GetComponentsInChildren<WheelCollider>();
                if(Mathf.Abs(_configBaseProcessor._HIDRollInput) > 0)
                {
                    _configBaseProcessor._LandingGearSteeringAngle = _configBaseProcessor._YokeRollSetting * _configBaseProcessor._LandingGearWheelMaxSteeringAngle;
                }
                else
                {
                    _configBaseProcessor._LandingGearSteeringAngle = _configBaseProcessor._PedalRudderSetting * _configBaseProcessor._LandingGearWheelMaxSteeringAngle;
                }               
                foreach (WheelCollider wheel in _configBaseProcessor._LandingGearWheels)
                {
                    wheel.motorTorque = _configBaseProcessor._HIDThrottleInput * 10000;
                }
                foreach (WheelCollider steeringWheel in steeringWheels)
                {
                    steeringWheel.steerAngle = _configBaseProcessor._LandingGearSteeringAngle;
                }
            }
            else
            {
                _configBaseProcessor._LandingGearSteeringAngle = 0;
            }
        }
        #endregion
    }
}
