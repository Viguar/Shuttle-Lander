using UnityEngine;

namespace Viguar.Aircraft
{
    public class AircraftCockpitComponentProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        private bool hasInteractableButtons;
        private bool hasInteractableKnobs;
        private bool hasInteractableYokes;

        private InteractableCockpitButton[] interactableButtons;
        private InteractableKnob[] interactableKnobs;
        private YokeController[] interactableYokes;

        public void InitCockpitComponentProcessor(AircraftBaseProcessor baseProcessor)
        {
            _configBaseProcessor = baseProcessor;
            if(FindObjectsByType<InteractableCockpitButton>(FindObjectsSortMode.None) != null) { hasInteractableButtons = true; }
            if(FindObjectsByType<InteractableKnob>(FindObjectsSortMode.None) != null) { hasInteractableKnobs = true; }
            if(FindObjectsByType<YokeController>(FindObjectsSortMode.None) != null) { hasInteractableYokes = true; }

            if(hasInteractableButtons) 
            { 
                interactableButtons = FindObjectsByType<InteractableCockpitButton>(FindObjectsSortMode.None);
                foreach (InteractableCockpitButton button in interactableButtons) { button.InitInteractableButton(baseProcessor); }
            }

            if(hasInteractableKnobs) 
            { 
                interactableKnobs = FindObjectsByType<InteractableKnob>(FindObjectsSortMode.None);
                foreach (InteractableKnob knob in interactableKnobs) { knob.InitInteractableKnob(baseProcessor); }
            }

            if(hasInteractableYokes) 
            { 
                interactableYokes = FindObjectsByType<YokeController>(FindObjectsSortMode.None);
                foreach (YokeController yoke in interactableYokes) { yoke.InitYokeController(baseProcessor); }
            }            
        }
        
        public void PerformCockpitComponentsFixedUpdate()
        {
            if(hasInteractableButtons) { foreach (InteractableCockpitButton button in interactableButtons) { button.PerformInteractableButtonCalculations(); } }
            if(hasInteractableKnobs) { foreach (InteractableKnob knob in interactableKnobs) { knob.PerformKnobCalculations(); } }
            if(hasInteractableYokes) { foreach (YokeController yoke in interactableYokes) { yoke.PerformYokeCalculations(); } }
        }

    }
}
