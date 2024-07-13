using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft.Management;

namespace Viguar.Aircraft
{
    public class AircraftDebugCommunicator : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private GlobalDebugManager _globalDebugManager;

        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _globalDebugManager = GameObject.FindGameObjectWithTag("managerDebugManager").GetComponent<GlobalDebugManager>();
        }


        private void Update()
        {
            //DebugGUIProcessor
        }

        //Debug Window ON/OFF Pass to Shuttle
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

        //Finding & Executing GlobalDebugManager
        public void ShowDebugMeshes()
        {
            _globalDebugManager.ForceDebugMeshRenderingState(true);
        }
        public void HideDebugMeshes()
        {
            _globalDebugManager.ForceDebugMeshRenderingState(false);
        }
        public void ToggleDebugMeshes()
        {
            _globalDebugManager.ToggleDebugMeshRenderingState();
        }
    }
}
