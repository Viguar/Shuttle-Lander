using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftStateProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();
        }

        public void EvaluateAircraftStates()
        {
            CheckStateFlying();
            CheckStateLanding();
            CheckStateTakeoff();
            CheckStateGrounded();
            CheckStateStationary();
            CheckStateParked();
            CheckStateStalled();
            CheckStateCrashed();
        }

        private void CheckStateFlying()
        {
            if(!_configBaseProcessor._StateGrounded && !_configBaseProcessor._StateCrashed) { _configBaseProcessor._StateFlying = true; }
            else { _configBaseProcessor._StateFlying = false; }
        }
        private void CheckStateLanding()
        {
            if(_configBaseProcessor._StateFlying)
            {
                if(_configBaseProcessor._AltitudeRaycast < _configBaseProcessor._StateLandingDetectionMaxAltitude)
                {
                    if (_configBaseProcessor._VerticalSpeed > _configBaseProcessor._StateLandingDetectionVerticalSpeedRegime.x && _configBaseProcessor._VerticalSpeed < _configBaseProcessor._StateLandingDetectionVerticalSpeedRegime.y && _configBaseProcessor._ForwardSpeed > _configBaseProcessor._StateLandingDetectionSpeedRegime.x && _configBaseProcessor._ForwardSpeed < _configBaseProcessor._StateLandingDetectionSpeedRegime.y)
                    {
                        if(_configBaseProcessor._LandingGearType == ConfigLandingGear._cLandingGearTypes.RetractableGear)
                        {
                            if(_configBaseProcessor._LandingGearExtended) { _configBaseProcessor._StateLanding = true; }
                            else { _configBaseProcessor._StateLanding = false; }
                            
                        }
                        else
                        {
                            _configBaseProcessor._StateLanding = true;
                        }                        
                    }                    
                }
            }
            else
            {
                _configBaseProcessor._StateLanding = false;
            }
        }
        private void CheckStateTakeoff()
        {
            if(_configBaseProcessor._StateGrounded && _configBaseProcessor._ForwardSpeed > _configBaseProcessor._StateTakeoffDetectionSpeedRegime.x && _configBaseProcessor._ForwardSpeed < _configBaseProcessor._StateTakeoffDetectionSpeedRegime.y)
            {
                _configBaseProcessor._StateTakeoff = true;
            }
            else
            {
                _configBaseProcessor._StateTakeoff = false;
            }
        }
        private void CheckStateGrounded()
        {
            if (_configBaseProcessor._LandingGearWheelsAmount > 0)
            {
                int groundedWheels = 0;
                foreach (WheelCollider wheel in _configBaseProcessor._LandingGearWheels)
                {
                    if (wheel.isGrounded) { groundedWheels = groundedWheels + 1; }
                    _configBaseProcessor._LandingGearWheelsAmountGrounded = groundedWheels;
                }
                if (groundedWheels >= _configBaseProcessor._LandingGearWheelsAmount / 2)
                {
                    _configBaseProcessor._StateGrounded = true;
                }
                else
                {
                    _configBaseProcessor._StateGrounded = false;
                    groundedWheels = 0;
                    _configBaseProcessor._LandingGearWheelsAmountGrounded = groundedWheels;
                }
            }
        }
        private void CheckStateStationary()
        {
            if (_configBaseProcessor._StateGrounded && _configBaseProcessor._ForwardSpeed < _configBaseProcessor._StateStationaryDetectionSpeedRegime.y && _configBaseProcessor._LeverThrottleSetting == 0)
            {
                _configBaseProcessor._StateStationary = true;                
            }
            else
            {
                _configBaseProcessor._StateStationary = false;
            }
        }
        private void CheckStateParked()
        {
            if (_configBaseProcessor._StateStationary && !_configBaseProcessor._MasterEnginesOn)
            {
                _configBaseProcessor._StateParked = true;
            }
            else
            {
                _configBaseProcessor._StateParked = false;
            }
        }
        private void CheckStateStalled()
        {
            if (!_configBaseProcessor._StateGrounded && _configBaseProcessor._StallState == AircraftBaseProcessor.StallStateTypes.Upset)
            {
                _configBaseProcessor._StateStalled = true;
            }
            else
            {
                _configBaseProcessor._StateStalled = false;
            }
        }
        private void CheckStateCrashed()
        {

        }




    }
}
