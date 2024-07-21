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
        
        private void Start()
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
            _userInputProcessor = GetComponentInParent<UserInputProcessor>();
        }

        private void FixedUpdate()
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
                _configBaseProcessor.GetComponent<AircraftControlInputProcessor>().SetDirectionalSteeringInput(_configBaseProcessor._PilotHIDAdjustmentInput);                
                Cursor.visible = false;
            }
            else
            {
                _configBaseProcessor.GetComponent<AircraftControlInputProcessor>().SetDirectionalSteeringInput(Vector2.zero);
                hasActivatedCursor = true;                
            }
        }
    }
}
