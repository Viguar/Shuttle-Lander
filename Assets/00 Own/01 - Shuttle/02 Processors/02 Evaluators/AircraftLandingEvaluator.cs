using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class AircraftLandingEvaluator : MonoBehaviour
    {
        //Script resets itself after either climbing above landing detection altitude or coming to complete stop.
        private AircraftBaseProcessor _configBaseProcessor;

        private bool TouchdownDetected = false;
        private bool BounceDetected = false;
        private int TotalTouches = 0;

        private float TouchdownSpeed;
        private float TouchdownVerticalSpeed;



        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
        }

        private void Update()
        {
           RecordForcesAtFirstTouchdown();
        }


        private void RecordForcesAtFirstTouchdown()
        {
            if (!TouchdownDetected)
            {
                foreach (WheelCollider wheel in _configBaseProcessor._LandingGearWheels)
                {

                    if (wheel.GetGroundHit(out WheelHit hitinfo))
                    {
                        TotalTouches = 1;
                        TouchdownSpeed = _configBaseProcessor._ForwardSpeed;
                        TouchdownVerticalSpeed = _configBaseProcessor._VerticalSpeed;
                        TouchdownDetected = true;
                    }
                }
            }
        }

        private void ResetRecordedForces()
        {
            TouchdownSpeed = 0;
            TouchdownVerticalSpeed = 0;
        }
        private void ResetRecordedBounces()
        {
            TotalTouches = 0;
        }

    }
}
