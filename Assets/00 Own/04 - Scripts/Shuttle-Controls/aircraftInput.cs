using System;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof (aircraftController))]
    public class aircraftInput : MonoBehaviour
    {
        private aircraftDebugText m_debugController;             
        private aircraftController m_Aeroplane;
        //private aircraftAutoFlight m_Autopilot;       
        //public float m_autoThrottleSetting = 0f;
        //public float m_autoPitchSetting = 0f;
        //public bool m_autoAirbrakeSetting = false;
        
        private void Awake()
        {
           m_Aeroplane = GetComponent<aircraftController>();
           //m_Autopilot = GetComponent<aircraftAutoFlight>();
           m_debugController = GetComponent<aircraftDebugText>();
        }      

        private void Update()
        {
            //Flight Systen Controls Non Physics Based
            bool m_AirBrakes = Input.GetButton("Airbrake");
            bool m_Flaps = Input.GetButtonDown("Flaps");
            bool m_LandingGear = Input.GetButtonDown("LandingGear");
            bool m_TogaMode = Input.GetButtonDown("Toga");

            bool m_atToggle = Input.GetButtonDown("AutoThrottle");
            bool m_atMode = Input.GetButtonDown("AutoThrottleMode");
            bool m_apToggle = Input.GetButtonDown("AutoPilot");            

            bool m_atOverride = Input.GetButton("Throttle");
            
            bool m_apOverrideViaPitch = Input.GetButton("Pitch");
            bool m_apOverrideYawRoll = false;
            bool m_apOverrideViaYaw = Input.GetButton("Yaw");
            bool m_apOverrideViaRoll = Input.GetButton("Roll");
            if(m_apOverrideViaYaw || m_apOverrideViaRoll) { m_apOverrideYawRoll = true; }
                           
            bool m_Debug = Input.GetButtonDown("Debug1");
            //bool m_AutoThrottle = Input.GetButton("atAutoCruise");
            //bool m_AutoPilotAltitude = Input.GetButton("apAutoAltitude");

            //m_pitch = m_pitch + m_autoPitchSetting;
            //m_Throttle = m_Throttle + m_autoThrottleSetting;

            m_Aeroplane.ControlFlightSystems(m_AirBrakes, m_Flaps, m_LandingGear, m_TogaMode); 
            m_Aeroplane.ControlFlightAutomaticsAT(m_atToggle, m_atMode, m_atOverride);
            m_Aeroplane.ControlFlightAutomaticsAP(m_apToggle, m_apOverrideViaPitch, m_apOverrideYawRoll);
            m_debugController.toggleDebugPanel(m_Debug);                                 
        }

        private void FixedUpdate()
        {
            //Flight Controls Physics Based
            float m_roll = Input.GetAxis("Roll");
            float m_pitch = Input.GetAxis("Pitch");
            float m_Yaw = Input.GetAxis("Yaw");
            float m_Throttle = Input.GetAxis("Throttle");

            Mathf.Clamp(m_pitch, -1f, 1f);
            Mathf.Clamp(m_Throttle, -1f, 1f);

            m_Aeroplane.Move(m_roll, m_pitch, m_Yaw, m_Throttle);
        }

    }
}
