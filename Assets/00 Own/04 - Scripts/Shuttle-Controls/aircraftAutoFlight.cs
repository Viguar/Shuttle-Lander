using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    public class aircraftAutoFlight : MonoBehaviour
    {
        /*
        private aircraftController m_Aeroplane;
        private aircraftInput m_AeroplaneInput;

        private float auto_targetSpeed;                            //The target speed of the autothrottle.
        private float auto_throttleAdjustmentRate = 0.1f;          //The rate of throttle change to maintain speed.
        private float auto_fineThrottleAdjustmentRate = 0.001f;    //The fine rate of throttle change to maintain speed.
        private float auto_currentThrottleAdjustmentRate;
        private bool auto_allowUseOfAirbrake;

        private float auto_targetAltitude = 100f;                  //The target altitude for the autopilot to aim for.
        private float auto_maxPitch = 0.1f;                        //How much pitch the autopilot is allowed to input into the controls.
        private float auto_maxVerticalRate = 5f;                   //The maximum vertical speed before the autopilot stops pitchting.


        public bool AP_AutoFlightLevel      { get; private set; }
        public bool AT_AutoCruise           { get; private set; }
        public float AutoCruiseSpeed        { get; private set; }
        public float AutoFlightLevelTarget  { get; private set; }
        public float AutoVerticalSpeedLimit { get; private set; }

        void Start()
        {
            m_Aeroplane = GetComponent<aircraftController>();
            m_AeroplaneInput = GetComponent<aircraftInput>();

            autoStartValues();
        }

        void Update()
        {
            autoMaintainSpeed();
            autoFlightlevel();
        }


        #region Set Flight Automatics Targets
        public void toggleAutomatics(bool ap_Flightlevel, bool at_CruiseSpeed)
        {
            if(ap_Flightlevel)
            {
                Auto_ToggleAutoFlightLevel();
            }
            if(at_CruiseSpeed)
            {
                Auto_ToggleAutoCruise();
            }
        }

        #region Autopilot
        public void Auto_SetTargetFlightLevel(float altitude, float maxVerticalSpeed)
        {
            AutoFlightLevelTarget = altitude;
            AutoVerticalSpeedLimit = maxVerticalSpeed;
        }

        public void Auto_MaintainCurrentFlightLevel()
        {
            Auto_SetTargetFlightLevel(m_Aeroplane.Altitude, auto_maxVerticalRate);
        }

        public void Auto_ToggleAutoFlightLevel()
        {
                AP_AutoFlightLevel = !AP_AutoFlightLevel;          
        }
        #endregion
        #region Autothrottle
        public void Auto_SetSpeed(float speed)
        {
            AutoCruiseSpeed = speed;
        }

        public void Auto_MaintainCurrentSpeed()
        {
            Auto_SetSpeed(m_Aeroplane.ForwardSpeed);
        }

        public void Auto_ToggleAutoCruise()
        {
                AT_AutoCruise = !AT_AutoCruise;        
        }
        #endregion

        #endregion

        private void autoMaintainSpeed()
        {
            if (AT_AutoCruise)
            {
                #region Rate Intensity
                if (Mathf.Abs(m_Aeroplane.ForwardSpeed - AutoCruiseSpeed) > 1) //If the speed difference is smaller than one, use fine adjustment values & do not use airbraking. Otherwise, allow.
                {
                    auto_currentThrottleAdjustmentRate = auto_throttleAdjustmentRate;
                    auto_allowUseOfAirbrake = true;
                }
                else
                {
                    auto_currentThrottleAdjustmentRate = auto_fineThrottleAdjustmentRate;
                    auto_allowUseOfAirbrake = false;
                }
                #endregion

                if (m_Aeroplane.ForwardSpeed < AutoCruiseSpeed)
                {
                    m_AeroplaneInput.m_autoThrottleSetting = Mathf.Clamp(m_AeroplaneInput.m_autoThrottleSetting + auto_currentThrottleAdjustmentRate, -1f, 1f);
                    m_AeroplaneInput.m_autoAirbrakeSetting = false;
                }
                if (m_Aeroplane.ForwardSpeed > AutoCruiseSpeed)
                {
                    m_AeroplaneInput.m_autoThrottleSetting = Mathf.Clamp(m_AeroplaneInput.m_autoThrottleSetting - auto_currentThrottleAdjustmentRate * 0.1f, -1f, 1f);
                    m_AeroplaneInput.m_autoAirbrakeSetting = auto_allowUseOfAirbrake;
                }
            }
            else
            {
                m_AeroplaneInput.m_autoThrottleSetting = 0f;
            }
        }

        private void autoFlightlevel()
        {
            if (AP_AutoFlightLevel)
            {
                if (Mathf.Abs(m_Aeroplane.Altitude - AutoFlightLevelTarget) > 5)
                {
                    if (m_Aeroplane.Altitude < AutoFlightLevelTarget)
                    {
                        if (AutoVerticalSpeedLimit > m_Aeroplane.VerticalSpeed)
                        {
                            m_AeroplaneInput.m_autoPitchSetting = -auto_maxPitch;
                        }
                        else
                        {
                            m_AeroplaneInput.m_autoPitchSetting = 0;
                        }
                    }
                    if (m_Aeroplane.Altitude > AutoFlightLevelTarget)
                    {
                        if (-AutoVerticalSpeedLimit < m_Aeroplane.VerticalSpeed)
                        {
                            m_AeroplaneInput.m_autoPitchSetting = auto_maxPitch;
                        }
                        else
                        {
                            m_AeroplaneInput.m_autoPitchSetting = 0;
                        }
                    }
                }
                else
                {
                    autoLevelOff();
                }
            }
            else
            {
                m_AeroplaneInput.m_autoPitchSetting = 0f;
            }
        }

        private void autoLevelOff()
        {
            m_AeroplaneInput.m_autoPitchSetting = -m_Aeroplane.PitchAngle * 10;             
        }

        private void autoStartValues()
        {
            AT_AutoCruise = false;
            AP_AutoFlightLevel = false;

            Auto_SetSpeed(auto_targetSpeed);
            Auto_SetTargetFlightLevel(auto_targetAltitude, auto_maxVerticalRate);           
        }
        */
        }
    }
    
