using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.WeatherDynamics
{
    public class weatherWindZone : MonoBehaviour
    {
        [Header("Readout")]
        public float z_CurrentWindStrength; //The current windStrength;
        public float z_CurrentTurbulence;    //The current turbulence;

        [Header("General")]
        public float z_WindStrength;        //The general strength of wind into a direction.
        public float z_Turbulence;          //The how much wind strength can fluctuate: Windstrength += Turbulence / Windstrength -= Turbulence.

        [Header("Gusts")]
        public float z_WindGustStrength;    //The possible intensity of gusting wind. Added on top of the strength.
        public int z_WindGustsPerMinute;    //How often gusts can occur in a minute.
        public int z_WindGustDuration;      //The duration of gusting wind in seconds.
        public AnimationCurve z_GustShape = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f); //The gusting strength over time during gust.

        private bool gusting = false;
        private float gustTimer = 0f;
        private float occurrences;
        private float occurrenceTimer = 0f;
        private float windSpeed;

        public float ZoneWindStrength { get; set; }
        public float ZoneTurbulence { get; set; }
        public float ZoneGust { get; set; }


        void Start()
        {
            z_WindStrength = ZoneWindStrength;
            z_Turbulence = ZoneTurbulence;
            z_WindGustStrength = ZoneGust;

            setWindStrength(z_WindStrength);
            occurrences = 60 / z_WindGustsPerMinute;

            z_WindStrength *= 10;
            z_Turbulence *= 10;
            z_WindGustStrength *= 10;

        }

        void Update()
        {
            z_WindStrength = ZoneWindStrength;
            z_Turbulence = ZoneTurbulence;
            z_WindGustStrength = ZoneGust;
            calculateTurbulenceFluctuation(z_Turbulence);
            checkForGusting();
        }

        void setWindStrength(float amount)
        {
            z_CurrentWindStrength = amount; 
        }

        void calculateTurbulenceFluctuation(float turbulence)
        {            
            float turbulenceRatio;
            z_CurrentTurbulence = Random.Range(-turbulence, turbulence);
            turbulenceRatio = Mathf.InverseLerp(-turbulence, turbulence, z_CurrentTurbulence);
            setWindStrength(z_WindStrength + turbulenceRatio * z_CurrentTurbulence);
        }

        void checkForGusting()
        {
            calculateGustOccurrence();
            executeWindGust();
        }
        void calculateGustOccurrence()
        {
            occurrenceTimer += Time.deltaTime;
            if (occurrenceTimer > occurrences)
            {
                if (!gusting)
                {
                    gusting = true;
                }
                occurrenceTimer = 0f;
            }
        }
        void executeWindGust()
        {
            if (gusting)
            {
                gustTimer += Time.deltaTime;
                if (gustTimer < z_WindGustDuration)
                {
                    float currentGustStrength;
                    currentGustStrength = z_WindGustStrength * z_GustShape.Evaluate(Mathf.InverseLerp(0, z_WindGustDuration, gustTimer));
                    setWindStrength(z_WindStrength + currentGustStrength);
                }
                else
                {
                    gustTimer = 0f;
                    gusting = false;
                }
            }
        }

    }
}
