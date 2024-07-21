using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.ControlPanels
{
   public class LocalizerPanelController : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private bool _PanelTurnedOn = false;
        private bool _PanelDefect = false;
        private bool _NoSignal = false;

        private void Start()
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
            HandleLocalizerPanelStates();
        }

        public void ToggleLocalizerPanel()
        {
            _PanelTurnedOn = !_PanelTurnedOn;
            HandleLocalizerPanelStates();
        }

        private void HandleLocalizerPanelStates()
        {
            if (_PanelTurnedOn && !_PanelDefect && !_NoSignal) { _configBaseProcessor._CockpitLocalizerPanelState = AircraftBaseProcessor.CockpitLocalizerPanelStates.On; }
            else if (_PanelTurnedOn && !_PanelDefect && _NoSignal) { _configBaseProcessor._CockpitLocalizerPanelState = AircraftBaseProcessor.CockpitLocalizerPanelStates.NoSignal; }            
            else if (_PanelTurnedOn && _PanelDefect) { _configBaseProcessor._CockpitLocalizerPanelState = AircraftBaseProcessor.CockpitLocalizerPanelStates.Defect; }
            else if (!_PanelTurnedOn) { _configBaseProcessor._CockpitLocalizerPanelState = AircraftBaseProcessor.CockpitLocalizerPanelStates.Off; }
        }
    }
}
