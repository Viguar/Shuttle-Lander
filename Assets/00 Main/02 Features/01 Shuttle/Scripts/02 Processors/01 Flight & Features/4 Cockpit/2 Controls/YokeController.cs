using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class YokeController : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private UserInputProcessor _userInputProcessor;
        private bool hasClickedOnYoke = false;
        private bool hasActivatedCursor = true;       
        private float currentSteeringInputRoll;
        private float currentSteeringInputPitch;
        public void InitYokeController(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
            _userInputProcessor = _configBaseProcessor._userInputProcessor;
        }
        public void PerformYokeCalculations()
        {
            CheckForInputOnYoke();
            OnYokeInput();
        }
        private void CheckForInputOnYoke()
        {
            if (_configBaseProcessor._PilotHIDSubmitInput)
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.tag == "cockpitYokeWheel")
                    {
                        _userInputProcessor.RecordMousePosition();
                        hasClickedOnYoke = true;
                        hasActivatedCursor = false;
                    }
                }
            }
            else
            {
                hasClickedOnYoke = false;
                if (!hasActivatedCursor) 
                { 
                    Cursor.visible = true;
                }                
            }
        }
        private void OnYokeInput()
        {            
            if(hasClickedOnYoke)
            {
                currentSteeringInputRoll +=  _configBaseProcessor._PilotHIDAdjustmentInput.x * 0.005f;
                currentSteeringInputPitch += _configBaseProcessor._PilotHIDAdjustmentInput.y * 0.005f;
                currentSteeringInputRoll = Mathf.Clamp(currentSteeringInputRoll, -1, 1);
                currentSteeringInputPitch = Mathf.Clamp(currentSteeringInputPitch, -1, 1);
                _configBaseProcessor._aircraftControlInputProcessor.SetDirectionalSteeringInput(new Vector2(currentSteeringInputRoll, currentSteeringInputPitch));                
                Cursor.visible = false;
            }
            else
            {
                currentSteeringInputRoll = 0;
                currentSteeringInputPitch = 0;
                _configBaseProcessor._aircraftControlInputProcessor.SetDirectionalSteeringInput(Vector2.zero);
                hasActivatedCursor = true;                
            }
        }
    }
}
