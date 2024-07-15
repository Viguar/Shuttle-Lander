using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class AircraftHIDController : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        private void Start()
        {
            _configBaseProcessor.GetComponent<AircraftBaseProcessor>();
        }

        public void controlOverridePitch()
        {

        }
        public void controlOverrideYaw()
        {

        }
        public void controlOverrideRoll()
        {

        }
        public void controlOverrideAirbrake()
        {

        }
    }
}
