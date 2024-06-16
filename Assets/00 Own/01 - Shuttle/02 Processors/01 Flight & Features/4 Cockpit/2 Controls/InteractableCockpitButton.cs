using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay;


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
        [DrawIf("soundOnInteraction", true)][SerializeField] private AudioClip interactionSound;

        private AudioSource buttonSoundSource;
        private Transform pushablePart;
        private Vector3 MouseActionPushableOriginalLocation;
        private Vector3 mouseActionPushableTarget;
        private float mouseInteractionPushDepth = -0.00435f;
        private int mouseInteractionMovementSmoothing = 3;
        private bool mouseInteracted = false;
        private AircraftBaseProcessor _configBaseProcessor;

        void Start()
        {
            foreach(Transform child in transform)
            {
                if(child.tag == "cockpitButtonPad") { pushablePart = child; }
            }
            if(soundOnInteraction) { buttonSoundSource = GetComponent<AudioSource>(); }
            MouseActionPushableOriginalLocation = pushablePart.localPosition;
            _configBaseProcessor = gameObject.GetComponentInParent<AircraftBaseProcessor>();
        }

        void Update()
        {
            if (isMouseResponsive) { handleMouseInteraction(); }
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = _configBaseProcessor._DebugActiveCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 1000))
                {
                    if(hit.collider.gameObject == gameObject)
                    {                        
                        OnButtonPressed.Invoke();
                        mouseInteracted = true;
                    }
                }
            }
        }       
        private void handleMouseInteraction()
        {               
            if (mouseInteracted)
            {
                mouseActionPushableTarget.y = MouseActionPushableOriginalLocation.y + mouseInteractionPushDepth;
                pushablePart.transform.localPosition = Vector3.Slerp(pushablePart.transform.localPosition, mouseActionPushableTarget, mouseInteractionMovementSmoothing * Time.deltaTime);
                if(pushablePart.transform.localPosition == mouseActionPushableTarget)
                {
                    mouseInteracted = false;
                }
            }
            else
            {
                mouseActionPushableTarget.y = MouseActionPushableOriginalLocation.y;
                pushablePart.transform.localPosition = Vector3.Slerp(pushablePart.transform.localPosition, mouseActionPushableTarget, mouseInteractionMovementSmoothing * Time.deltaTime);
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
