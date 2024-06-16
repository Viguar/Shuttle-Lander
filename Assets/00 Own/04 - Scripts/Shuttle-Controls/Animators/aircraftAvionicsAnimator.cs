using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft //rewrite this script a bit cleaner one day pls yesyes
{
    public class aircraftAvionicsAnimator : MonoBehaviour
    {
        [SerializeField] private avionicsTypeData[] m_types;
        [SerializeField] private Transform m_artificialHorizon;
        private aircraftController m_Plane;
        
        void Start()
        {
            m_Plane = GetComponent<aircraftController>();                         
        }

        void Update()
        {
            foreach (var mainNeedleToTurn in m_types)
            {
                switch (mainNeedleToTurn.avType)
                {
                    case avionicsTypeData.avionicsType.airSpeedGauge:                                             
                            turnAirSpeedNeedles(mainNeedleToTurn);
                            break;
                                      
                    case avionicsTypeData.avionicsType.verticalSpeedGauge:
                        
                            turnVerticalSpeedNeedle(mainNeedleToTurn);
                            break;

                    case avionicsTypeData.avionicsType.altimeterGauge:
                        
                            turnAltimeterNeedles(mainNeedleToTurn);                            
                            break;
                     
                    case avionicsTypeData.avionicsType.engineGauge:
                           turnEnginePowerOutputNeedles(mainNeedleToTurn);
                            break;

                    case avionicsTypeData.avionicsType.fuelGauge:
                            turnFuelNeedle(mainNeedleToTurn);
                            break;

                    case avionicsTypeData.avionicsType.atmosphericTemperatureGauge:
                            //turnAtmosphericTemperatureNeedle
                            break;

                    case avionicsTypeData.avionicsType.atmosphericPressureGauge:
                            //turnAtmosphericPressureNeedle
                            break;

                    case avionicsTypeData.avionicsType.headingGauge:
                            //turnHeadingNeedle
                            break;



                }
            }
            handleArtificialHorizon();
        }

        #region Handle Case Types
        private void turnAirSpeedNeedles(avionicsTypeData mainNeedleToTurn) //todo, make it turn cool
        {
                float currentSpeed = m_Plane.ForwardSpeed;
                float normalizedSpeed = Mathf.InverseLerp(0f, 360f, currentSpeed);
                float targetRotation = 720 * normalizedSpeed;  //to have it clamp at only 1 turn put the factor to 360                              
                mainNeedleToTurn.mainNeedle.localRotation = Quaternion.Euler(0f, 0f, targetRotation);

                if (mainNeedleToTurn.needleConfig != null)
                foreach (NeedleData fNeedle in mainNeedleToTurn.needleConfig)
                {
                    float factorNeedleTarget = targetRotation * fNeedle.needleFactor;
                    fNeedle.tNeedle.localRotation = Quaternion.Euler(0f, 0f, factorNeedleTarget);
                }                          
        }
        
        private void turnVerticalSpeedNeedle(avionicsTypeData mainNeedleToTurn)
        {
                float verticalSpeed = m_Plane.VerticalSpeed;
                float normalizedVerticalSpeed = Mathf.InverseLerp(-30, 30, verticalSpeed);
                float targetRotation = 340 * normalizedVerticalSpeed;
                mainNeedleToTurn.mainNeedle.localRotation = Quaternion.Euler(180, 180, targetRotation);                
        }

        private void turnAltimeterNeedles(avionicsTypeData mainNeedleToTurn)
        {
                float altitude = m_Plane.Altitude;
                float normalizedAltitude = Mathf.InverseLerp(0f, 1000, altitude);
                float targetRotation = 360f * normalizedAltitude;
                mainNeedleToTurn.mainNeedle.localRotation = Quaternion.Euler(0f, 0f, targetRotation);
                foreach (var fNeedle in mainNeedleToTurn.needleConfig)
                {
                    float factorNeedleTarget = targetRotation * fNeedle.needleFactor;
                    fNeedle.tNeedle.localRotation = Quaternion.Euler(0f, 0f, factorNeedleTarget);
                }            
        }

        private void turnEnginePowerOutputNeedles(avionicsTypeData mainNeedleToTurn)
        {
                float enginePowerOutput = m_Plane.EnginePower;
                float normalizedEnginePowerOutput = Mathf.InverseLerp(0f, m_Plane.MaxEnginePower, enginePowerOutput);
                float targetRotation = 342f * normalizedEnginePowerOutput;
                mainNeedleToTurn.mainNeedle.localRotation = Quaternion.Euler(0f, 0, targetRotation);
                foreach (var fNeedle in mainNeedleToTurn.needleConfig)
                {
                    float requestedInput = m_Plane.Throttle;
                    float normalizedRequestedInput = Mathf.InverseLerp(0f, 1f, requestedInput);
                    float requestedInputTargetRotation = 342f * normalizedRequestedInput;                    
                    fNeedle.tNeedle.localRotation = Quaternion.Euler(0f, 0f, requestedInputTargetRotation);
                }            
        }

        private void turnFuelNeedle(avionicsTypeData mainNeedleToTurn)
        {
            float fuelLevel = m_Plane.m_currentFuel;
            float normalizedCurrentFuel = Mathf.InverseLerp(0f, m_Plane.m_fuelTankSize, fuelLevel);
            float targetRotation = 190 * normalizedCurrentFuel;
            mainNeedleToTurn.mainNeedle.localRotation = Quaternion.Euler(0f, 0, targetRotation);
            foreach (var fNeedle in mainNeedleToTurn.needleConfig)
            {
                //Add Code for the Fuel Burn Rate once it exists
            }
        }      

        private void handleArtificialHorizon()
        {
            float bankAngle = Vector3.Dot(m_Plane.transform.right, Vector3.up) * Mathf.Rad2Deg;
            float pitchAngle = Vector3.Dot(m_Plane.transform.forward, Vector3.up) * Mathf.Rad2Deg;

            Quaternion screenRotation = Quaternion.Euler(pitchAngle, 0f, bankAngle);
            m_artificialHorizon.transform.localRotation = screenRotation;

            // Vector3 targetPosition = new Vector3(0f, pitchAngle, 0f);
            // m_artificialHorizon.position = targetPosition;
        }
        #endregion





        [Serializable]
        public class avionicsTypeData 
        {
            public enum avionicsType 
            {
                RateGauge,
                ValueGauge,

                airSpeedGauge,
                verticalSpeedGauge,
                altimeterGauge,
                headingGauge,
                timeGauge,

                fuelGauge,
                engineGauge,
                atmosphericTemperatureGauge,                
                atmosphericPressureGauge,
                cabinPressureGauge,

            }
            public avionicsType avType;
            public Transform mainNeedle;            
            public NeedleData[] needleConfig;


        }

        [Serializable]
        public class NeedleData
        {
            public enum NeedleType
            {
                Needle,
                FactoredNeedle,
            }
            public NeedleType nType;            
            public Transform tNeedle;
            public float needleFactor;
            //public int RateGaugeClampMin = 0;
            //public int RateGaugeClampMax = 16;

        }

    }
}

