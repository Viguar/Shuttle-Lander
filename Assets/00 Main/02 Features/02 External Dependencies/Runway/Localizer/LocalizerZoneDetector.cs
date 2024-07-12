using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft.Runways
{
    public class LocalizerZoneDetector : MonoBehaviour
    {
        private RunwayLocalizer localizer;
        public enum LocalizerTypes { Offset, Center, }
        public LocalizerTypes LocalizerType;
        private void Start()
        {
            localizer = GameObject.FindGameObjectWithTag("runwayLocalizer").GetComponent<RunwayLocalizer>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "aircraft")
            {
                if(LocalizerType == LocalizerTypes.Center)
                {
                    localizer._Centered = true;
                }
                else
                {
                    localizer._Offset = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "aircraft")
            {
                if (LocalizerType == LocalizerTypes.Center)
                {
                    localizer._Centered = false;
                }
                else
                {
                    localizer._Offset = false;
                }
            }
        }
    }
}
