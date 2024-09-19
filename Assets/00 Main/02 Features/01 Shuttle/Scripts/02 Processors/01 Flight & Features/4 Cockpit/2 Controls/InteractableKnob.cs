using UnityEngine;
using Viguar.Inspector.PropertyFields;

namespace Viguar.Aircraft
{
    public class InteractableKnob : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private UserInputProcessor _userInputProcessor;
        private bool isControllingKnob = false;
        private bool hasRecordedCursorPosition = false;
        private bool hasSetCursorPosition = true;
        private GameObject knobMesh;
        private float totalKnobTwistValue;
        private float currentKnobTwistValue;
        private AudioSource knobSoundSource;

        [SerializeField] private float sensitivity = 10;
        [SerializeField] private float OutputFactor = 1;
        [SerializeField] private string ConnectedValue;
        [SerializeField] private float DefaultValue;
        [SerializeField] private Vector2 ValueMinMaxClamp;
        [SerializeField] private bool hasConnectedDisplay;
        [DrawIf("hasConnectedDisplay", true)][SerializeField] private SegmentDisplay ConnectedDisplay;
        [SerializeField] private bool soundOnInteraction;
        //[SerializeField] private AudioClip[] interactionSounds;

        public void InitInteractableKnob(AircraftBaseProcessor aircraftBaseProcessor)
        {
            _configBaseProcessor = aircraftBaseProcessor;
            _userInputProcessor = _configBaseProcessor._userInputProcessor;
            foreach (Transform child in transform)
            {
                if (child.tag == "cockpitKnobModel") { knobMesh = child.gameObject; }
            }
            currentKnobTwistValue = DefaultValue;
            currentKnobTwistValue = Mathf.Clamp(currentKnobTwistValue, ValueMinMaxClamp.x, ValueMinMaxClamp.y);
            ConnectedDisplay.DisplayText(currentKnobTwistValue);
            if(soundOnInteraction) { knobSoundSource = GetComponent<AudioSource>(); }
        }

        public void PerformKnobCalculations()
        {
            OnKnobInput();
        }

        private void OnKnobInput()
        {
            //First: Check if the raycast at least hit the Knob once while holding click.
            //Once we let go, we do not control the knob anymore.
            if (_configBaseProcessor._PilotHIDSubmitInput)
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000) && hit.collider.gameObject == gameObject) { isControllingKnob = true; }
            }
            else
            {
                isControllingKnob = false;
            }

            //Second: Logic for controlling Yoke.
            if (isControllingKnob) //While click-dragging the yoke:
            {
                hasSetCursorPosition = false; //Reset the value to set the cursor position once we let go.

                //If we have not yet recorded the mouse position, record it, else make it invisible.
                if (!hasRecordedCursorPosition) { _userInputProcessor.RecordMousePosition(); hasRecordedCursorPosition = true; }
                else { Cursor.visible = false; _userInputProcessor.CheckMouseLockingState(); }

                //Finally Invoke the logic.
                TwistKnob();
            }
            else //Once let go from the yoke:
            {
                hasRecordedCursorPosition = false; //Reset the value so we can record the position again once we click on the yoke again.

                //If we have not yet set the mouse position yet, set it.
                if (!hasSetCursorPosition) { Cursor.visible = true; _userInputProcessor.CheckMouseLockingState(); _userInputProcessor.MoveMousePositionToLast(); hasSetCursorPosition = true; }
            } 
        }

        private void TwistKnob()
        {
            //Twist knob
            float rotationInput = _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * sensitivity;
            Vector3 currentRot = knobMesh.transform.localRotation.eulerAngles;
            knobMesh.transform.localRotation = Quaternion.Euler(currentRot.x, currentRot.y + _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * sensitivity, currentRot.z);

            //Run value of rotation
            currentKnobTwistValue += _configBaseProcessor._PilotHIDAdjustmentInput.normalized.x * OutputFactor;
            currentKnobTwistValue = Mathf.Clamp(currentKnobTwistValue, ValueMinMaxClamp.x, ValueMinMaxClamp.y);
            ConnectedDisplay.DisplayText(currentKnobTwistValue);

            //Play Audio every 5 degrees of rotation.
            totalKnobTwistValue += rotationInput;
            if (Mathf.Abs(totalKnobTwistValue) >= 15f && soundOnInteraction && knobSoundSource != null) { knobSoundSource.Play(); totalKnobTwistValue = 0f; }
        }
    }
}
