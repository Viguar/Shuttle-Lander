using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Viguar.Aircraft;


namespace Viguar.UserInterfaceSystem
{
    public class UiSwapOnEvent : MonoBehaviour
    {
        private aircraftController m_Aeroplane;       
        [SerializeField] private swapOnCase[] swapToOnEvent;
        [SerializeField] private GameObject defaultImage;

        void Start()
        {
            m_Aeroplane = GameObject.FindGameObjectWithTag("aircraft").GetComponent<aircraftController>();
        }

        void Update()
        {
            if(m_Aeroplane != null)
            {
                foreach (var swapCase in swapToOnEvent)
                {
                    switch (swapCase.ct)
                    {
                        //Airplane Warnings
                        #region Master Alerts
                        case swapOnCase.onCaseType.MasterCaution:

                            break;

                        case swapOnCase.onCaseType.MasterAlert:

                            break;
                        #endregion

                        #region Engine & Fire Warnings
                        case swapOnCase.onCaseType.CabinFireWarning:

                            break;

                        case swapOnCase.onCaseType.EngineFireWarning:

                            break;

                        case swapOnCase.onCaseType.EngineLoadWarning:

                            break;

                        case swapOnCase.onCaseType.EngineFailWarning:

                            break;

                        case swapOnCase.onCaseType.ElectricalFaultWarning:

                            break;
                        #endregion

                        #region Fuel Warnings
                        case swapOnCase.onCaseType.FuelLowWarning:

                            break;

                        case swapOnCase.onCaseType.FuelDepletedWarning:

                            break;
                        #endregion

                        #region Flight Control Warnings
                        case swapOnCase.onCaseType.LandingGearFailWarning:
                            
                            break;
                        #endregion

                        #region Upset Flight State Warnings 
                        case swapOnCase.onCaseType.OverspeedWarning:
                            
                            break;

                        case swapOnCase.onCaseType.VerticalSpeedWarning:

                            break;

                        case swapOnCase.onCaseType.BankAngleWarning:

                            break;

                        case swapOnCase.onCaseType.AltitudeWarning:

                            break;

                        case swapOnCase.onCaseType.StallInboundWarning:

                            break;

                        case swapOnCase.onCaseType.StallWarning: //Turn on the Stall Warning image when m_Aeroplane.AircraftStateStalled returns true.
                            swapToCaseImage(swapCase.swapToImage, m_Aeroplane.AircraftStateStalled);    
                            break;
                        #endregion


                        //Airplane States
                        #region Automatics
                        case swapOnCase.onCaseType.AutoPilot:

                            break;

                        case swapOnCase.onCaseType.AutoThrottle:

                            break;
                        #endregion

                        #region Acceleration & Deceleration
                        case swapOnCase.onCaseType.SpeedBrake: //Turn on the Speed Brake image when m_Aeroplane.Airbrake returns true.
                            swapToCaseImage(swapCase.swapToImage, m_Aeroplane.AirBrakes);
                            break;

                        case swapOnCase.onCaseType.TogaMode:

                            break;
                        #endregion

                        #region Ground Operation
                        case swapOnCase.onCaseType.LandingGear: //Turn on the Landing Gear image when m_Aeroplane.GearExtended returns true.
                            swapToCaseImage(swapCase.swapToImage, m_Aeroplane.GearExtended);            
                            break;
                            #endregion

                    }
                }
            }
        }

        private void swapToCaseImage(GameObject stateImage, bool isCase)
        {
            defaultImage.SetActive(!isCase);
            stateImage.SetActive(isCase);
        }

        [Serializable]
        public class swapOnCase
        {
            public enum onCaseType
            {
                //Master
                MasterCaution,
                MasterAlert,
                //Engine & Fire Warnings
                CabinFireWarning,
                EngineFireWarning,
                EngineLoadWarning,
                EngineFailWarning,
                ElectricalFaultWarning,
                //Fuel Warnings
                FuelLowWarning,
                FuelDepletedWarning,
                //Flight Control Warnings
                LandingGearFailWarning,
                //Upset FLight State Warnings
                OverspeedWarning,
                VerticalSpeedWarning,
                BankAngleWarning,
                AltitudeWarning,
                StallInboundWarning,
                StallWarning,

                _,

                //Automatics
                AutoPilot,
                AutoThrottle,
                //Accel & Decel
                SpeedBrake,
                TogaMode,
                //Ground Operation
                LandingGear,
            }
            public onCaseType ct;
            public GameObject swapToImage;
        }
    }
}
