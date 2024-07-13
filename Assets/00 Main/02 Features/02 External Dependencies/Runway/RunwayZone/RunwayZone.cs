using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;

namespace Viguar.Aircraft.Runways
{
    [ExecuteInEditMode]
    public class RunwayZone : MonoBehaviour
    {
        [Header("Runway Zone Creator")]
        [Header("Runway Zone Bounds")]
        [LabelOverride("Runway Zone Length")] public float _RunwayZoneLength = 10f;
        [LabelOverride("Runway Zone Width")] public float _RunwayZoneWidth = 10f;
        [LabelOverride("Runway Zone Height")] public float _RunwayZoneHeight = 10f;

        private AircraftBaseProcessor _configBaseProcessor;

        private void OnValidate()
        {
            GetComponentInChildren<RunwayZoneDrawer>().OnRunwayZoneValidate();
        }
        private void Start()
        {
            GetComponentInChildren<RunwayZoneDrawer>().OnRunwayZoneValidate();
            _configBaseProcessor = GameObject.FindGameObjectWithTag("aircraft").GetComponent<AircraftBaseProcessor>();
        }

        public void TransmitDetectionInfo(bool detected)
        {
            if (detected) { _configBaseProcessor._RunwayZoneReceiverInfoState = AircraftBaseProcessor.RunwayZoneReceiverInfoTypes.WithinRange; }
            else if (!detected) { _configBaseProcessor._RunwayZoneReceiverInfoState = AircraftBaseProcessor.RunwayZoneReceiverInfoTypes.OutOfRange; }
            else { _configBaseProcessor._RunwayZoneReceiverInfoState = AircraftBaseProcessor.RunwayZoneReceiverInfoTypes.Unknown; }            
        }
    }
}
