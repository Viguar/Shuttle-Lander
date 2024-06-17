using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class AircraftWarningSignalCommander : MonoBehaviour
    {
        private AircraftWarningSignalProcessor[] WarningLights;
        public float lightFlashesPerSecondSlow;
        public float lightFlashesPerSecondFast;
        public float audioCuesPerSecondSlow;
        public float audioCuesPerSecondFast;

        private void Start()
        {
            WarningLights = GetComponentsInChildren<AircraftWarningSignalProcessor>();
            foreach (AircraftWarningSignalProcessor light in WarningLights)
            {
                light.InitialiseLight(lightFlashesPerSecondSlow, lightFlashesPerSecondFast);
                light.InitialiseAudio(audioCuesPerSecondSlow, audioCuesPerSecondFast);
            }
        }

        private void Update()
        {
            foreach (AircraftWarningSignalProcessor light in WarningLights)
            {
                light.HandleWarningSignals();
            }
        }

        public void MuteAlarmSignals()
        {
            foreach (AircraftWarningSignalProcessor light in WarningLights)
            {
                light.OverrideMute();
            }
        }

    }
}