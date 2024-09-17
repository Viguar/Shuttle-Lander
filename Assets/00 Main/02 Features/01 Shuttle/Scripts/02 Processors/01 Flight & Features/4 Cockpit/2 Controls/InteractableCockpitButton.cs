using UnityEngine;
using UnityEngine.Events;

namespace Viguar.Aircraft
{
    public class InteractableCockpitButton : MonoBehaviour
    { 
        [SerializeField] private bool isMouseResponsive;
        [SerializeField] private bool isVirtualRealityResponsive;        
        [Space(10)]
        [SerializeField] private UnityEvent OnButtonPressed = new UnityEvent();
        [Space(10)]
        [SerializeField] private bool soundOnInteraction;
        [SerializeField] private AudioClip[] interactionSounds;        

        private AudioSource buttonSoundSource;
        private Transform pushablePart;
        private Vector3 MouseActionPushableOriginalLocation;
        private Vector3 mouseActionPushableTarget;
        private float mouseInteractionPushDepth = -0.004f;
        private int mouseInteractionMovementSmoothing = 20;
        private bool mouseInteracted = false;
        private bool invokeLock = false;
        private AircraftBaseProcessor _configBaseProcessor;
   
        public void InitInteractableButton(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
            foreach (Transform child in transform)
            {
                if (child.tag == "cockpitButtonPad") { pushablePart = child; }
            }
            if (soundOnInteraction) { buttonSoundSource = GetComponent<AudioSource>(); }
            MouseActionPushableOriginalLocation = pushablePart.localPosition;
            if (interactionSounds == null) { soundOnInteraction = false; }
        }
        public void PerformInteractableButtonCalculations()
        {
            if (isMouseResponsive) { handleMouseInteraction(); }
        }
        private void handleMouseInteraction()
        {
            if (_configBaseProcessor._PilotHIDSubmitInput)
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.gameObject == gameObject)
                    {                       
                        mouseInteracted = true;
                    }
                }                
            }

            if (mouseInteracted && !invokeLock)
            {               
                mouseActionPushableTarget.y = MouseActionPushableOriginalLocation.y + mouseInteractionPushDepth;
                pushablePart.transform.localPosition = Vector3.Slerp(pushablePart.transform.localPosition, mouseActionPushableTarget, mouseInteractionMovementSmoothing * Time.deltaTime);
                if (pushablePart.transform.localPosition == mouseActionPushableTarget)
                {
                    if (soundOnInteraction) { buttonSoundSource.clip = interactionSounds[Random.Range(0, interactionSounds.Length)]; buttonSoundSource.Play(); }
                    OnButtonPressed.Invoke();
                    invokeLock = true;                   
                }
            }

            if (invokeLock && !_configBaseProcessor._PilotHIDSubmitInput)
            {               
                mouseActionPushableTarget.y = MouseActionPushableOriginalLocation.y;
                pushablePart.transform.localPosition = Vector3.Slerp(pushablePart.transform.localPosition, mouseActionPushableTarget, mouseInteractionMovementSmoothing * Time.deltaTime);
                if (pushablePart.transform.localPosition == mouseActionPushableTarget)
                {
                    invokeLock = false;
                    mouseInteracted = false;
                }
            }
        }

        private void handleVirtualRealityInteraction()
        {

        }

        public void printPushMessage()
        {
            print("Hello!!! I work.");
        }
    }
}
