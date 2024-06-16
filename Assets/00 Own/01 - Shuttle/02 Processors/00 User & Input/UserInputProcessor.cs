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
        public AircraftBaseProcessor _configBaseProcessor;
        private void Awake()
        {            
            aInput = new HIDInputComputer(); //Initialise the InputActions
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>(); //The airplane to pass those values on to.
        }
        private void OnEnable()
        {
            aInput.Enable();
        }
        private void Update()
        {
            controlOverrideHIDInputs();
            controlDebugHIDInputs();
        }
        private void controlDebugHIDInputs()
        {
            _configBaseProcessor._DebugCursortoggleInput = aInput.Debugcontrols.pDebugDebugcontrolsTogglecursor.triggered;
        }
        private void controlOverrideHIDInputs()
        {
            _configBaseProcessor._OverridePitchInput = aInput.Aircraftcontrols.aControlsurfacesPitchcontrolOverride.ReadValue<float>();
            _configBaseProcessor._OverrideRollInput = aInput.Aircraftcontrols.aControlsurfacesRollcontrolOverride.ReadValue<float>();
            _configBaseProcessor._OverrideYawInput = aInput.Aircraftcontrols.aControlsurfacesYawcontrolOverride.ReadValue<float>();
            _configBaseProcessor._OverrideAirbrakeInput = aInput.Aircraftcontrols.aControlsurfacesAirbrakecontrolOverride.ReadValue<float>();
            _configBaseProcessor._OverrideFlapsInput = aInput.Aircraftcontrols.aControlsurfacesFlapcontrolOverride.triggered;
            _configBaseProcessor._OverrideLandingGearInput = aInput.Aircraftcontrols.aAircraftsystemsLandinggearcontrolOverride.triggered;
        }
    }
}
