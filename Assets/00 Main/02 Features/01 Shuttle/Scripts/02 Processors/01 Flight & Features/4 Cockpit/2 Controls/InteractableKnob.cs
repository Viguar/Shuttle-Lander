using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.EditorTooling.InspectorUITools.ConditionalPropertyDisplay;

namespace Viguar.Aircraft
{
    public class InteractableKnob : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private UserInputProcessor _userInputProcessor;
        private bool hasClickedOnKnob = false;
        private bool hasActivatedCursor = true;
        private GameObject knobMesh;
        private float knobTwistValue;
        [SerializeField] private float sensitivity = 10;
        [SerializeField] private float OutputFactor = 1;
        [SerializeField] private string ConnectedValue;
        [SerializeField] private float DefaultValue;
        [SerializeField] private Vector2 ValueMinMaxClamp;
        [SerializeField] private bool hasConnectedDisplay;
        [DrawIf("hasConnectedDisplay", true)][SerializeField] private SegmentDisplay ConnectedDisplay;

        private void Start()
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
            _userInputProcessor = GetComponentInParent<UserInputProcessor>();
            foreach(Transform child in transform)
            {
                if(child.tag == "cockpitKnobModel") { knobMesh = child.gameObject; }
            }
            knobTwistValue = DefaultValue;
            knobTwistValue = Mathf.Clamp(knobTwistValue, ValueMinMaxClamp.x, ValueMinMaxClamp.y);
            ConnectedDisplay.DisplayText(knobTwistValue);
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
                    if (hit.collider.gameObject == gameObject)
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
            //Twist knob
            Vector3 currentRot = knobMesh.transform.localRotation.eulerAngles;
            knobMesh.transform.localRotation = Quaternion.Euler(currentRot.x, currentRot.y + _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * sensitivity, currentRot.z);

            //Run value of rotation
            knobTwistValue += _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * OutputFactor;
            knobTwistValue = Mathf.Clamp(knobTwistValue, ValueMinMaxClamp.x, ValueMinMaxClamp.y);
            ConnectedDisplay.DisplayText(knobTwistValue);
        }
    }
}
