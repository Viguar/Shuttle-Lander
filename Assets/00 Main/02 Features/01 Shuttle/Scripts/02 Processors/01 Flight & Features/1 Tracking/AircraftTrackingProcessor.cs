using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftTrackingProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;
        private float previousForwardSpeed;

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();
        }

        public void PerformAircraftTrackingCalculations()
        {
            CalculateAircraftCompassHeading();
            CalculateAircraftDegreePitchAngle();
            CalculateAircraftDegreeBankAngle();
            CalculateAircraftAltitudeSeaLevel();
            CalculateAircraftAltitudeRaycast();
            CalculateAircraftAltitudeRelative();
            CalculateAircraftForwardSpeed();
            CalculateAircraftVerticalSpeed();
            CalculateAircraftRollAngle();
            CalculateAircraftPitchAngle();
            CalculateAircraftPitchState();
            CalculateAircraftRollState();
            CalculateAircraftForwardSpeedState();
            CalculateAircraftVerticalSpeedState();
            CalculateAircraftForwardSpeedRateOfChange();
            CalculateAircraftStallState();
            CalculateAircraftAltitudeState();
        }

        private void CalculateAircraftCompassHeading()
        {
            float radianHeading = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
            radianHeading = (radianHeading + 360f) % 360f;
            _configBaseProcessor._Heading = radianHeading;
        }
        private void CalculateAircraftDegreePitchAngle()
        {
            _configBaseProcessor._PitchDegreeAngle = (90 - Vector3.Angle(Vector3.up, transform.forward)) * -1; //Untested
        }
        private void CalculateAircraftDegreeBankAngle()
        {
            _configBaseProcessor._BankDegreeAngle = (90 - Vector3.Angle(Vector3.up, transform.right)) * -1; //Untested
        }
        private void CalculateAircraftAltitudeSeaLevel()
        {
            _configBaseProcessor._AltitudeSeaLevel = transform.position.y + _configBaseProcessor._YSeaLevel;
        }
        private void CalculateAircraftAltitudeRaycast()
        {            
            var ray = new Ray(transform.position - Vector3.up * 3, -Vector3.up); //Safe distance starting below the plane to avoid colliding with any of the plane's own colliders
            Debug.DrawRay(transform.position - Vector3.up * 3, -Vector3.up, Color.red);
            RaycastHit hit;
            _configBaseProcessor._AltitudeRaycast = Physics.Raycast(ray, out hit) ? hit.distance + 3 : transform.position.y;
        }
        private void CalculateAircraftAltitudeRelative()
        {
            _configBaseProcessor._AltitudeRelative = Mathf.InverseLerp(0, _configBaseProcessor._MaximumFlightAltitude, _configBaseProcessor._AltitudeSeaLevel);
        }
        private void CalculateAircraftForwardSpeed()
        {
            var localVelocity = transform.InverseTransformDirection(_aircraftRigidbody.velocity);
            _configBaseProcessor._ForwardSpeed = Mathf.Max(0, localVelocity.z);
        }
        private void CalculateAircraftVerticalSpeed()
        {
            _configBaseProcessor._VerticalSpeed = _aircraftRigidbody.velocity.y;
        }
        private void CalculateAircraftForwardSpeedRateOfChange()
        {            
            _configBaseProcessor._ForwardSpeedRateOfChange = (_configBaseProcessor._ForwardSpeed - previousForwardSpeed) / Time.deltaTime * 10;
            previousForwardSpeed = _configBaseProcessor._ForwardSpeed;
        }
        private void CalculateAircraftRollAngle()
        {
            var flatForward = transform.forward; //Calculate the flat forward direction (with no y component).
            flatForward.y = 0;
            if (flatForward.sqrMagnitude > 0) //If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
            {
                flatForward.Normalize();
                var flatRight = Vector3.Cross(Vector3.up, flatForward);
                var localFlatRight = transform.InverseTransformDirection(flatRight);
                _configBaseProcessor._RollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
            }
        }
        private void CalculateAircraftPitchAngle()
        {
            var flatForward = transform.forward; //Calculate the flat forward direction (with no y component).
            flatForward.y = 0;
            if (flatForward.sqrMagnitude > 0) //If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
            {
                flatForward.Normalize();
                var localFlatForward = transform.InverseTransformDirection(flatForward);
                _configBaseProcessor._PitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z);
            }
        }

        private void CalculateAircraftPitchState()
        {
            if(Mathf.Abs(_configBaseProcessor._PitchDegreeAngle) < Mathf.Abs(_configBaseProcessor._MaxStablePitchAngle * _configBaseProcessor._CriticalWarningPercentage))
            {
                _configBaseProcessor._PitchAngleState = AircraftBaseProcessor.PitchAngleStateTypes.Stable;
            }
            else if(Mathf.Abs(_configBaseProcessor._PitchDegreeAngle) < Mathf.Abs(_configBaseProcessor._MaxStablePitchAngle))
            {
                _configBaseProcessor._PitchAngleState = AircraftBaseProcessor.PitchAngleStateTypes.Critical;
            }
            else
            {
                _configBaseProcessor._PitchAngleState = AircraftBaseProcessor.PitchAngleStateTypes.Upset;
            }
        }
        private void CalculateAircraftRollState()
        {
            if (Mathf.Abs(_configBaseProcessor._BankDegreeAngle) < Mathf.Abs(_configBaseProcessor._MaxStableRollAngle * _configBaseProcessor._CriticalWarningPercentage))
            {
                _configBaseProcessor._RollAngleState = AircraftBaseProcessor.RollAngleStateTypes.Stable;
            }
            else if (Mathf.Abs(_configBaseProcessor._BankDegreeAngle) < Mathf.Abs(_configBaseProcessor._MaxStableRollAngle))
            {
                _configBaseProcessor._RollAngleState = AircraftBaseProcessor.RollAngleStateTypes.Critical;
            }
            else
            {
                _configBaseProcessor._RollAngleState = AircraftBaseProcessor.RollAngleStateTypes.Upset;
            }
        }
        private void CalculateAircraftAltitudeState()
        {
            if (!_configBaseProcessor._StateGrounded && ! _configBaseProcessor._StateStationary && !_configBaseProcessor._StateParked)
            {
                if (_configBaseProcessor._AltitudeRaycast > _configBaseProcessor._MinStableAltitude * _configBaseProcessor._CriticalWarningPercentage)
                {
                    _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Stable;
                }
                else if (_configBaseProcessor._AltitudeRaycast > _configBaseProcessor._MinStableAltitude && !_configBaseProcessor._StateLanding)
                {
                    if (_configBaseProcessor._StateLanding) { _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Stable; }
                    else { _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Critical; }
                }
                else
                {
                    if (_configBaseProcessor._StateLanding) { _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Stable; }
                    else { _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Upset; }
                }
            }
            else
            {
                _configBaseProcessor._AltitudeState = AircraftBaseProcessor.AltitudeStateTypes.Stable;
            }
        }
        private void CalculateAircraftForwardSpeedState()
        {
            if (!_configBaseProcessor._StateGrounded)
            {
                if (_configBaseProcessor._ForwardSpeed < _configBaseProcessor._MinMaxStableForwardSpeed.y * _configBaseProcessor._CriticalWarningPercentage && _configBaseProcessor._ForwardSpeed > _configBaseProcessor._MinMaxStableForwardSpeed.x + (_configBaseProcessor._MinMaxStableForwardSpeed.x * Mathf.Abs(_configBaseProcessor._CriticalWarningPercentage - 1)))
                {
                    _configBaseProcessor._SpeedState = AircraftBaseProcessor.SpeedStateTypes.Stable;
                }
                else if (_configBaseProcessor._ForwardSpeed < _configBaseProcessor._MinMaxStableForwardSpeed.y && _configBaseProcessor._ForwardSpeed > _configBaseProcessor._MinMaxStableForwardSpeed.x)
                {
                    _configBaseProcessor._SpeedState = AircraftBaseProcessor.SpeedStateTypes.Critical;
                }
                else
                {
                    _configBaseProcessor._SpeedState = AircraftBaseProcessor.SpeedStateTypes.Upset;
                }
            }
            else
            {
                _configBaseProcessor._SpeedState = AircraftBaseProcessor.SpeedStateTypes.Stable;
            }
        }
        private void CalculateAircraftVerticalSpeedState()
        {
            if(Mathf.Abs(_configBaseProcessor._VerticalSpeed) < _configBaseProcessor._MaxStableVerticalSpeed * _configBaseProcessor._CriticalWarningPercentage)
            {
                _configBaseProcessor._VerticalSpeedState = AircraftBaseProcessor.VerticalSpeedStateTypes.Stable;
            }
            else if(_configBaseProcessor._VerticalSpeed < _configBaseProcessor._MaxStableVerticalSpeed)
            {
                _configBaseProcessor._VerticalSpeedState = AircraftBaseProcessor.VerticalSpeedStateTypes.Critical;
            }
            else
            {
                _configBaseProcessor._VerticalSpeedState = AircraftBaseProcessor.VerticalSpeedStateTypes.Upset;
            }
        }
        private void CalculateAircraftStallState()
        {
            _configBaseProcessor._StallLoad = 1 - _configBaseProcessor._LiftSpeedFactorCurve.Evaluate(Mathf.InverseLerp(0, _configBaseProcessor._MaximumLiftSpeed, _configBaseProcessor._ForwardSpeed));
            if (_configBaseProcessor._StallLoad > _configBaseProcessor._CriticalWarningPercentage)
            {
                _configBaseProcessor._StallState = AircraftBaseProcessor.StallStateTypes.Upset;
            }
            else if (_configBaseProcessor._StallLoad > 0.99f)
            {
                _configBaseProcessor._StallState = AircraftBaseProcessor.StallStateTypes.Critical;
            }
            else
            {
                _configBaseProcessor._StallState = AircraftBaseProcessor.StallStateTypes.Stable;
            }
        }
    }
}
