using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Rendering;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;

namespace Viguar.Aircraft.Runways
{
    [ExecuteInEditMode]
    public class RunwayZoneDetector : MonoBehaviour
    {        
        private RunwayZone runwayZone;

        private void Start()
        {            
            runwayZone = GetComponentInParent<RunwayZone>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "aircraft") { runwayZone.TransmitDetectionInfo(true); }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "aircraft") { runwayZone.TransmitDetectionInfo(false); }
        }
    }
}
