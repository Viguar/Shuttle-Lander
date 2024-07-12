using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viguar.Aircraft.ControlPanels
{
    public class ControlPanelController : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
        }
    }




    [Serializable]
    public class ControlPanelData
    {
        public string cpName;
    }

}
