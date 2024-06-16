using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftAutoFlightProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;
        private float throttleChangeRate;
        private float yokePitchChangeRate;
        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();
        }
       
        private void InitAT()
        {
            _configBaseProcessor._ATOnline = true;
            _configBaseProcessor._ATMode = AircraftBaseProcessor._ATModes.ATModeDisengaged;
            _configBaseProcessor._ATTargetSpeed = 0;
            _configBaseProcessor._ATTargetThrust = 0;
        }
        private void InitAP()
        {

        }
        private void InitAL()
        {

        }

        public void SetAtMode(AircraftBaseProcessor._ATModes atmode)
        {
            _configBaseProcessor._ATMode = atmode;
        }
        public void SetAPMode(AircraftBaseProcessor._APModes apmode)
        {
            _configBaseProcessor._APMode = apmode;
        }
        public void SetALMode(AircraftBaseProcessor._ALModes almode)
        {
            _configBaseProcessor._ALMode = almode;
        }

        private void PerformAT()
        {
            switch(_configBaseProcessor._ATMode)
            {
                case AircraftBaseProcessor._ATModes.ATOff:
                    break;
                case AircraftBaseProcessor._ATModes.ATModeDisengaged:
                    break;
                case AircraftBaseProcessor._ATModes.ATModeCommand:
                    break;
                case AircraftBaseProcessor._ATModes.ATModeSpeed:
                    break;
                case AircraftBaseProcessor._ATModes.ATModeThrust:
                    break;
            }
        }
        private void PerformAP()
        {
            switch(_configBaseProcessor._APMode)
            {
                case AircraftBaseProcessor._APModes.APOff:
                    break;
                case AircraftBaseProcessor._APModes.APModeDisengaged:
                    break;
                case AircraftBaseProcessor._APModes.APModeCommand:
                    break;
                case AircraftBaseProcessor._APModes.APModeAltitudeAssist:
                    break;
                case AircraftBaseProcessor._APModes.APModeHeadingAssist:
                    break;
                case AircraftBaseProcessor._APModes.APModePitchAssist:
                    break;
            }
        }
        private void PerformAL()
        {
            switch(_configBaseProcessor._ALMode)
            {
                case AircraftBaseProcessor._ALModes.ALOff:
                    break;
                case AircraftBaseProcessor._ALModes.ALModeDisengaged:
                    break;
                case AircraftBaseProcessor._ALModes.ALModeCommand:
                    break;
                case AircraftBaseProcessor._ALModes.ALModeCompensate:
                    break;
            }
        }

        public void SetATTargetSpeed(float targetSpeed)
        {
            _configBaseProcessor._ATTargetSpeed = (targetSpeed > 0 ? targetSpeed : 0);
        }
        public void SetATTargetThrust(float targetThrust)
        {
            _configBaseProcessor._ATTargetThrustPercentage = Mathf.Clamp01(targetThrust);
        }
        public void SetAPTargetPitch(float targetPitch)
        {
            _configBaseProcessor._APTargetPitchAngle = Mathf.Clamp(targetPitch, _configBaseProcessor._APPitchAngleLimits.x,  _configBaseProcessor._APPitchAngleLimits.y);
        }
        public void SetAPTargetHeading(float targetHeading)
        {

        }
        public void SetAPTargetAltitude(float targetAltitude)
        {
            _configBaseProcessor._APTargetAltitude = (targetAltitude > 0 ? targetAltitude : 0);
        }
        public void SetAPTargetAltitudeChangeRate(float changeRate)
        {
            _configBaseProcessor._APTargetVerticalSpeed = Mathf.Clamp(changeRate, 0, _configBaseProcessor._APVerticalSpeedLimit);
        }

        private void OnATModeSpeed()
        {
            _configBaseProcessor._ATTargetVelocity = Vector3.forward * _configBaseProcessor._ATTargetSpeed; //Turn the target speed into a vector "_ATTargetVelocity".
            _configBaseProcessor._ATTargetForce = ((_aircraftRigidbody.mass * _configBaseProcessor._ATTargetVelocity) - (_aircraftRigidbody.mass * _aircraftRigidbody.velocity)) / Time.deltaTime; //The required force to reach the target velocity derived from the 2nd Law of Thermodynamics.
            _configBaseProcessor._ATTargetThrottlePosition = _configBaseProcessor._ATTargetForce.normalized.z; //Normalize it to set the desired at throttle lever position.

            ATAdjustThrottlePosition(throttleChangeRate); //(ThrottleTarget - CurrentThrottle) = the throttle will be increased/reduced. Below the old code which should be the same technically.
           // if (_configBaseProcessor._LeverThrottleSetting < _configBaseProcessor._ATTargetThrottlePosition) { ATAdjustThrottlePosition(throttleChangeRate); }
           // if(_configBaseProcessor._LeverThrottleSetting > _configBaseProcessor._ATTargetThrottlePosition) { ATAdjustThrottlePosition(throttleChangeRate); }
        }
        private void OnATModeThrust()
        {            
            _configBaseProcessor._ATTargetThrottlePosition = _configBaseProcessor._ATTargetThrustPercentage;
            ATAdjustThrottlePosition(throttleChangeRate);
        }
        private void ATAdjustThrottleChangeRate()
        {
            float currentThrottleGap = Mathf.Clamp(_configBaseProcessor._ATTargetThrottlePosition - _configBaseProcessor._LeverThrottleSetting, -1, 1);
            throttleChangeRate = currentThrottleGap * Time.deltaTime;
        }
        private void ATAdjustThrottlePosition(float amount)
        {
            ATAdjustThrottleChangeRate();
            _configBaseProcessor._ATThrottleInput += amount;
            Mathf.Clamp(_configBaseProcessor._ATThrottleInput, -1, 1);
        }

        private void OnAPModeHeadingAssist()
        {

        }
        private void OnAPModePitchAssist()
        {
            Vector3 targetAngularVelocity = ((_aircraftRigidbody.mass * Vector3.right * _configBaseProcessor._APTargetPitchAngle) - (_aircraftRigidbody.mass * _aircraftRigidbody.angularVelocity)) / Time.deltaTime;
            _configBaseProcessor._APTargetYokePitchPosition = targetAngularVelocity.normalized.x;
            APAdjustYokePitchPosition(yokePitchChangeRate);
        }
        private void OnAPModeAltitudeAssist()
        {
            int pitchDirection = (_configBaseProcessor._AltitudeSeaLevel < _configBaseProcessor._APTargetAltitude ? 1 : -1); //Determine whether to climb (1) or descend (-1);
            Vector3 pitchDirVelocity = Vector3.up * _configBaseProcessor._APTargetVerticalSpeed * pitchDirection; //Turn this into a vector. 
            _configBaseProcessor._APTargetVerticalVelocity = (_aircraftRigidbody.velocity - pitchDirVelocity) * Time.deltaTime; //Get the desired Vertical Velocity by turning the difference in target/actual velocity into a climb rate by multiplying it with Time.deltaTime
            _configBaseProcessor._APTargetYokePitchPosition = _configBaseProcessor._APTargetVerticalVelocity.normalized.y; //Make input on the yoke until the target is reached.

            APAdjustYokePitchPosition(yokePitchChangeRate);
        }
        private void APAdjustYokePitchPositionChangeRate()
        {
            float currentYokePitchGap = Mathf.Clamp(_configBaseProcessor._APTargetYokePitchPosition - _configBaseProcessor._YokePitchSetting, -1, 1);
            yokePitchChangeRate = currentYokePitchGap * Time.deltaTime;
        }
        private void APAdjustYokePitchPosition(float amount)
        {
            APAdjustYokePitchPositionChangeRate();
            _configBaseProcessor._APPitchInput += amount;
            Mathf.Clamp(_configBaseProcessor._APPitchInput, -1, 1);
        }
    }
}

