using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viguar.Aircraft
{
    public class AircraftControlInputProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private AircraftControlSurfacesProcessor _controlSurfaceProcessor;
        private AircraftLandingGearProcessor _landingGearProcessor;

        private float totalPitchInput;
        private float totalYawInput;
        private float totalRollInput;
        private float totalThrottleInput;
        private float totalAirbrakeInput;
        private float totalWheelbrakeInput;

        private bool debugMouseState = false;

        private Vector2 mouseSteering;
        private float mouseSterringSensitivity = 4.5f;

        public void InitControlInputProcessor(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
            _controlSurfaceProcessor = _configBaseProcessor._aircraftControlSurfacesProcessor;
            _landingGearProcessor = _configBaseProcessor._aircraftLandingGearProcessor;
            Cursor.visible = debugMouseState;
        }
        public void PerformInputCalculations()
        {
            GetHIDInput();
            GetDebugHIDInput();
            CalculateTotalInput();
            SetVirtualControlInputs();
        }
        private void SetVirtualControlInputs()
        {
            _configBaseProcessor._YokePitchSetting = totalPitchInput;
            _configBaseProcessor._YokeRollSetting = totalRollInput;
            _configBaseProcessor._PedalRudderSetting = totalYawInput;
            _configBaseProcessor._LeverAirbrakeSetting = Mathf.Clamp01(_configBaseProcessor._LeverAirbrakeSetting + (totalAirbrakeInput * 0.005f)); //Changes the airbrake slowly
            _configBaseProcessor._PedalWheelbrakeSetting = totalWheelbrakeInput;
            if(totalAirbrakeInput == 0) { _configBaseProcessor._LeverAirbrakeSetting = Mathf.Clamp01(_configBaseProcessor._LeverAirbrakeSetting -  0.001f); }
                _configBaseProcessor._LeverFlapsSetting = _configBaseProcessor._HIDFlapsInput;

            _configBaseProcessor._LeverThrottleSetting = Mathf.Clamp01(_configBaseProcessor._LeverThrottleSetting + (totalThrottleInput * 0.01f)); //Changes the throttle lever gradually
            _configBaseProcessor._SwitchTogaSetting = _configBaseProcessor._HIDTogaInput;
            _configBaseProcessor._LandingGearInitiated = _configBaseProcessor._HIDLandingGearInput;
        }
        private void GetHIDInput()
        {           
            _configBaseProcessor._HIDPitchInput = Mathf.Clamp(_configBaseProcessor._OverridePitchInput + mouseSteering.y, -1f, 1f);//Mathf.Clamp(Input.GetAxis("Pitch"), -1, 1);
            _configBaseProcessor._HIDYawInput = _configBaseProcessor._OverrideYawInput;//Mathf.Clamp(Input.GetAxis("Yaw"), -1, 1);
            _configBaseProcessor._HIDRollInput = Mathf.Clamp(_configBaseProcessor._OverrideRollInput + mouseSteering.x, -1f, 1f);//Mathf.Clamp(Input.GetAxis("Roll"), -1, 1);
            _configBaseProcessor._HIDAirbrakeInput = _configBaseProcessor._OverrideAirbrakeInput;//Mathf.Clamp(Input.GetAxis("Airbrake"), 0, 1);
            _configBaseProcessor._HIDWheelbrakeInput = _configBaseProcessor._OverrideAirbrakeInput;//Mathf.Clamp(Input.GetAxis("Airbrake"), 0, 1);
            _configBaseProcessor._HIDFlapsInput = _configBaseProcessor._OverrideFlapsInput;//Input.GetButtonDown("Flaps");

            _configBaseProcessor._HIDThrottleInput = Mathf.Clamp(Input.GetAxis("Throttle"), -1, 1);
            _configBaseProcessor._HIDTogaInput = Input.GetButtonDown("Toga");
            _configBaseProcessor._HIDLandingGearInput = _configBaseProcessor._OverrideLandingGearInput;//Input.GetButtonDown("LandingGear");            
        }
        private void GetDebugHIDInput()
        {
            if(_configBaseProcessor._DebugCursortoggleInput)
            {
                _configBaseProcessor._DebugCursorActive = !_configBaseProcessor._DebugCursorActive;
                Cursor.visible = _configBaseProcessor._DebugCursorActive;
                _configBaseProcessor._userInputProcessor.CheckMouseLockingState();
            }       
        }
        private void CalculateTotalInput()
        {
            totalPitchInput = Mathf.Clamp(_configBaseProcessor._HIDPitchInput + _configBaseProcessor._APPitchInput, -1, 1);
            totalYawInput = Mathf.Clamp(_configBaseProcessor._HIDYawInput + _configBaseProcessor._APYawInput, -1, 1);
            totalRollInput = Mathf.Clamp(_configBaseProcessor._HIDRollInput + _configBaseProcessor._APRollInput, -1, 1);
            totalThrottleInput = Mathf.Clamp(_configBaseProcessor._HIDThrottleInput + _configBaseProcessor._ATThrottleInput, -1, 1);
            totalAirbrakeInput = Mathf.Clamp(_configBaseProcessor._HIDAirbrakeInput + _configBaseProcessor._APAirbrakeInput, 0, 1);
            totalWheelbrakeInput = Mathf.Clamp01(_configBaseProcessor._HIDWheelbrakeInput);

            _configBaseProcessor._DetectedThrottleInput = (totalThrottleInput == 0 ? false : true);
            _configBaseProcessor._DetectedPitchInput = (totalPitchInput == 0 ? false : true);
            _configBaseProcessor._DetectedRollInput = (totalRollInput == 0 ? false : true);
            _configBaseProcessor._DetectedYawInput = (totalYawInput == 0 ? false : true);
        }
        public void SetDirectionalSteeringInput(Vector2 steering)
        {
            mouseSteering = steering;
        }
    }
}
