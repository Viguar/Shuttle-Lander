using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices; // For importing the Windows API

//Input Processor for Unity's "new" Input system. I got to learn it quite late :(
namespace Viguar.Aircraft
{
    public class UserInputProcessor : MonoBehaviour
    {
        public HIDInputComputer aInput;
        private AircraftBaseProcessor _configBaseProcessor;

        [DllImport("user32.dll")] private static extern bool SetCursorPos(int X, int Y); //Import the SetCursorPos function from user32.dll (Windows specific)
        [DllImport("user32.dll")] private static extern bool GetCursorPos(out displaycursorposition lpPoint); //Import the GetCursorPos function from user32.dll (to get the current mouse position)

        private Vector2 _LockLocalMousePosition;
        public Vector2 _RecordedMousePosition;

        public bool _RightClick; //TEMP

        private void Awake()
        {
            aInput = new HIDInputComputer(); //Initialise the InputActions
        }
        private void OnEnable()
        {
            aInput.Enable();
        }

        private void OnDisable()
        {
            aInput.Disable();
        }     

        public void InitUserInputProcessor(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
        }

        public void PerformUserInputCalculations()
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
            InputAction _CockpitSecondaryMouseInput = aInput.Pilot.pCockpitSecondaryclick;
            InputAction _CockpitAdjustInput = aInput.Pilot.pCockpitDirectional;

            Vector2 adjustDirectional = _CockpitAdjustInput.ReadValue<Vector2>();
            Mathf.Clamp(adjustDirectional.x, -1, 1);
            Mathf.Clamp(adjustDirectional.y, -1, 1);
            float cockpitSubmit = _CockpitSubmitInput.ReadValue<float>();

            _configBaseProcessor._PilotHIDSubmitInput = (cockpitSubmit == 0 ? false : true);
            _RightClick = (_CockpitSecondaryMouseInput.ReadValue<float>() == 0 ? false : true);
            _configBaseProcessor._PilotHIDAdjustmentInput = adjustDirectional;
        }


        #region Mouse / Cursor Methods        
        public void RecordMousePosition() { _RecordedMousePosition = new Vector2(GetCursorPosition().x, GetCursorPosition().y); } //Stores the position of the mouse/cursor on the display in a Vector2.
        public void MoveMousePosition(Vector2 position) { SetCursorPos(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)); } //Moves the mouse/cursor to a desired Vector2.
        public void MoveMousePositionToLast() { MoveMousePosition(_RecordedMousePosition); } //Moved the mouse/cursor to the last stored position.


        public void LockMouse() { Cursor.lockState = CursorLockMode.Locked; } //Hard-Locks the mouse/cursor to the center of the window.          
        public void LimitMouse() { Cursor.lockState = CursorLockMode.Confined; } //Constrains the mouse/cursor to the borders of the window.
        public void FreeMouse() { Cursor.lockState = CursorLockMode.Locked; } //Completely unconstrained mouse/cursor.


        public void ShowMouse(bool visibilityState) { Cursor.visible = visibilityState; CheckMouseLockingState(); } //Hides/Unhides the mouse/cursor.





        private void CheckMouseLockingState()
        {
            if(!Cursor.visible) { Cursor.lockState = CursorLockMode.Locked; }
            else { Cursor.lockState = CursorLockMode.None;}
        }

        #endregion

        public struct displaycursorposition//Struct to store cursor coordinates
        {
            public int x;
            public int y;
        }
        public displaycursorposition GetCursorPosition() //Function to get the current cursor position
        {
            GetCursorPos(out displaycursorposition currentPos);
            return currentPos;
        }
    }
}

