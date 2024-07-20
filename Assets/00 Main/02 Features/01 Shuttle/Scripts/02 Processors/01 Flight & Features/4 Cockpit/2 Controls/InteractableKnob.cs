using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class InteractableKnob : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private UserInputProcessor _userInputProcessor;
        private bool hasClickedOnKnob = false;
        private bool hasActivatedCursor = true;
        private GameObject knobMesh;
        [SerializeField] private float sensitivity = 10;
        [SerializeField] private string ConnectedValue;

        private void Start()
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
            _userInputProcessor = GetComponentInParent<UserInputProcessor>();
            foreach(Transform child in transform)
            {
                if(child.tag == "cockpitKnobModel") { knobMesh = child.gameObject; }
            }
        }

        private void FixedUpdate()
        {
            CheckForInputOnKnob();
            OnKnobInput();
        }

        private void CheckForInputOnKnob()
        {
            if (_configBaseProcessor._PilotHIDSubmitInput)
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.tag == "cockpitInteractableKnob")
                    {
                        _userInputProcessor.RecordMousePosition();
                        hasClickedOnKnob = true;
                        hasActivatedCursor = false;
                    }
                }
            }
            else
            {
                hasClickedOnKnob = false;
                if (!hasActivatedCursor)
                {
                    Cursor.visible = true;
                }
            }
        }

        private void OnKnobInput()
        {
            if (hasClickedOnKnob)
            {
                TwistKnob();
                Cursor.visible = false;
            }
            else
            {
                hasActivatedCursor = true;
            }
        }

        private void TwistKnob()
        {
            Vector3 currentRot = knobMesh.transform.localRotation.eulerAngles;
            knobMesh.transform.localRotation = Quaternion.Euler(currentRot.x, currentRot.y + _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * sensitivity, currentRot.z);            
        }

        private void CalculateValue()
        {

        }
    }
}
