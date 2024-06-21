using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class DebugGUIProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
        }
        private void Update()
        {
            
        }

        private void OnDebugShutter()
        {
            if (_configBaseProcessor._DebugShutterInput) { CloseDebugWindow(); }
        }

        public void OpenDebugWindow()
        {
            _configBaseProcessor._DebugPanelActive = false;
        }

        public void CloseDebugWindow()
        {
            _configBaseProcessor._DebugPanelActive = false;
        }


    }
}
