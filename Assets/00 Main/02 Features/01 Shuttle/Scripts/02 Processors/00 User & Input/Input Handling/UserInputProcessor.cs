using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Input Processor for Unity's "new" Input system. I got to learn it quite late :(
namespace Viguar.Aircraft
{
    public class UserInputProcessor : MonoBehaviour
    {
        public HIDInputComputer aInput;
        private AircraftBaseProcessor _configBaseProcessor;

        private void Awake()
        {
            aInput = new HIDInputComputer(); //Initialise the InputActions
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>(); //The airplane to pass those values on to.
        }
        private void OnEnable()
        {
            aInput.Enable();
        }

        private void OnDisable()
        {
            aInput.Disable();
        }

        private void Update()
        {
            controlOverrideHIDInputs();
            controlDebugHIDInputs();
            controlKeyboardMouseCockpitHIDInputs();
        }



        private void controlDebugHIDInputs()
        {
            _configBaseProcessor._DebugCursortoggleInput = aInput.Debugcontrols.pDebugDebugcontrolsTogglecursor.triggered;
            _configBaseProcessor._DebugShutterInput = aInput.Debugcontrols.pDebugDebugcontrolsCloseDebugWindow.triggered;
        }
        private void controlOverrideHIDInputs()
        {
            InputAction _DirectionalControlOverrideAction = aInput.Aircraftcontrols.aAircraftcontrolDirectionalcontrolOverride;
            InputAction _AirbrakeControlOverrideAction = aInput.Aircraftcontrols.aAircraftcontrolAirbrakecontrolOverride;
            InputAction _FlapsControlOverrideAction = aInput.Aircraftcontrols.aAircraftcontrolFlapscontrolOverride;
            InputAction _GearControlOverrideAction = aInput.Aircraftcontrols.aAircraftcontrolGearcontrolOverride;

            Vector3 directionalControlOverride = _DirectionalControlOverrideAction.ReadValue<Vector3>();
            float airbrakeControlOverride = _AirbrakeControlOverrideAction.ReadValue<float>();
            bool flapControlOverride = _FlapsControlOverrideAction.triggered;
            bool gearControlOverride = _GearControlOverrideAction.triggered;


            _configBaseProcessor._OverrideRollInput = directionalControlOverride.x;
            _configBaseProcessor._OverridePitchInput = directionalControlOverride.y;
            _configBaseProcessor._OverrideYawInput = directionalControlOverride.z;

            _configBaseProcessor._OverrideAirbrakeInput = airbrakeControlOverride;
            _configBaseProcessor._OverrideFlapsInput = flapControlOverride;
            _configBaseProcessor._OverrideLandingGearInput = gearControlOverride;
        }
        private void controlKeyboardMouseCockpitHIDInputs()
        {
            InputAction _CockpitSubmitInput = aInput.Pilot.pCockpitMainclick;
            InputAction _CockpitAdjustInput = aInput.Pilot.pCockpitDirectional;

            Vector2 adjustDirectional = _CockpitAdjustInput.ReadValue<Vector2>();
            float cockpitSubmit = _CockpitSubmitInput.ReadValue<float>();

            _configBaseProcessor._PilotHIDSubmitInput = (cockpitSubmit == 0 ? false : true);
            _configBaseProcessor._PilotHIDAdjustmentInput = adjustDirectional;
        }
    }
}

