using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class YokeController : MonoBehaviour
    {     
        private AircraftBaseProcessor _configBaseProcessor;
        private UserInputProcessor _userInputProcessor;
        private bool isControllingYoke = false;
        private bool hasRecordedCursorPosition = false;
        private bool hasSetCursorPosition = true;
        private float currentSteeringInputRoll;
        private float currentSteeringInputPitch;

        public void InitYokeController(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
            _userInputProcessor = _configBaseProcessor._userInputProcessor;
        }
        public void PerformYokeCalculations()
        {
            OnYokeInput();
        }
        private void OnYokeInput()
        {
            //First: Check if the raycast at least hit the Yoke once while holding click.
            //Once we let go, we do not control the yoke anymore.
            if (_configBaseProcessor._PilotHIDSubmitInput)
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000) && hit.collider.tag == "cockpitYokeWheel") { isControllingYoke = true; }
            }
            else
            {
                isControllingYoke = false;
            }

            //Second: Logic for controlling Yoke.
            if(isControllingYoke) //While click-dragging the yoke:
            {
                hasSetCursorPosition = false; //Reset the value to set the cursor position once we let go.

                //If we have not yet recorded the mouse position, record it, else make it invisible.
                if (!hasRecordedCursorPosition) { _userInputProcessor.RecordMousePosition(); hasRecordedCursorPosition = true; }
                else { Cursor.visible = false; _userInputProcessor.CheckMouseLockingState(); }

                //Set the steering based on mouse-delate and parse it to the controlInputProcessor.
                currentSteeringInputRoll += _configBaseProcessor._PilotHIDAdjustmentInput.x * 0.005f;
                currentSteeringInputPitch += _configBaseProcessor._PilotHIDAdjustmentInput.y * 0.005f;
                currentSteeringInputRoll = Mathf.Clamp(currentSteeringInputRoll, -1, 1);
                currentSteeringInputPitch = Mathf.Clamp(currentSteeringInputPitch, -1, 1);
                _configBaseProcessor._aircraftControlInputProcessor.SetDirectionalSteeringInput(new Vector2(currentSteeringInputRoll, currentSteeringInputPitch));
            }
            else //Once let go from the yoke:
            {
                hasRecordedCursorPosition = false; //Reset the value so we can record the position again once we click on the yoke again.

                //If we have not yet set the mouse position yet, set it.
                if (!hasSetCursorPosition) { Cursor.visible = true; _userInputProcessor.CheckMouseLockingState(); _userInputProcessor.MoveMousePositionToLast(); hasSetCursorPosition = true ; }

                //Set the steering to zero, as we are not moving the yoke anymore.
                currentSteeringInputRoll = 0;
                currentSteeringInputPitch = 0;
                _configBaseProcessor._aircraftControlInputProcessor.SetDirectionalSteeringInput(Vector2.zero);
            }
        }
    }
}
