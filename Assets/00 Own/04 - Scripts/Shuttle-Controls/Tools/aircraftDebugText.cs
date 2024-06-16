using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using Unity.UI;

namespace Viguar.Aircraft
{
    public class aircraftDebugText : MonoBehaviour
    {
        public GameObject debugPanel;        

        [HideInInspector] public AircraftInfoField aircraftInfo;
        GameObject[] displayFieldObjs;
        
        private aircraftController m_plane;
        private bool toggled = false;

        public Color textNormalColor;
        public Color textTrueColor;
        public Color textFalseColor;
        
        

        void Start()
        {
            m_plane = GetComponent<aircraftController>();
            displayFieldObjs = GameObject.FindGameObjectsWithTag("debugTextDisplay");
            debugPanel.SetActive(false);
        }

        void Update()
        {          
            setDisplayfieldValues();
        }     

        public void setDisplayfieldValues()
        {
            foreach (GameObject dispField in displayFieldObjs)
            {
                debugDispField ddispField;
                string ddispText;
                ddispField = dispField.GetComponent<debugDispField>();
                               
                switch (ddispField.propertyDisplay)
                {
                    #region User Inputs
                    case AircraftInfoField.InfoProperty.USERINPUTS:
                        break;
                    case AircraftInfoField.InfoProperty.PitchInput:
                        ddispText = m_plane.PitchInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.YawInput:
                        ddispText = m_plane.YawInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.RollInput:
                        ddispText = m_plane.RollInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ThrottleInput:
                        ddispText = m_plane.ThrottleInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AirbrakesInput:
                        ddispText = m_plane.AirBrakes.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.FlapsInput:
                        ddispText = m_plane.FlapsInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.LandingGearInput:
                        ddispText = m_plane.LandingGearInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ToGaInput:
                        ddispText = m_plane.TogaInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Aircraft Position & Orientation
                    case AircraftInfoField.InfoProperty.AIRCRAFTTRANSFORM:
                        break;
                    case AircraftInfoField.InfoProperty.Pitch:
                        ddispText = m_plane.PitchAngle.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Yaw:
                        ddispText = m_plane.transform.rotation.y.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Roll:
                        ddispText = m_plane.RollAngle.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Aircraft Velocities
                    case AircraftInfoField.InfoProperty.AIRCRAFTDIR:
                        break;
                    case AircraftInfoField.InfoProperty.Altitude:
                        ddispText = m_plane.Altitude.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;                     
                    case AircraftInfoField.InfoProperty.ForwardSpeed:
                        ddispText = m_plane.ForwardSpeed.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.VerticalSpeed:
                        ddispText = m_plane.VerticalSpeed.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Engine Stats
                    case AircraftInfoField.InfoProperty.ENGINESYSINFO:
                        break;
                    case AircraftInfoField.InfoProperty.EngineOn:
                        ddispText = m_plane.EngineOn.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ToGaMode:
                        ddispText = m_plane.TogaMode.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ThrottleSetting:
                        ddispText = m_plane.Throttle.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.RequestedEnginePowerOutput:
                        float temporaryplaceholderfloat;
                        temporaryplaceholderfloat = m_plane.Throttle * m_plane.m_MaxEnginePower;
                        ddispText = temporaryplaceholderfloat.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.CurrentEnginePowerOutput:
                        ddispText = m_plane.EnginePower.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.EngineEfficiencyFactor:
                        ddispText = m_plane.EngineEfficiency.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Fuel Stats
                    case AircraftInfoField.InfoProperty.FUELSYSINFO:
                        break;
                    case AircraftInfoField.InfoProperty.FuelRemaining:
                        break;
                    case AircraftInfoField.InfoProperty.FuelBurnRate:
                        break;
                    #endregion
                    #region Flaps Stats
                    case AircraftInfoField.InfoProperty.FLAPSYSINFO:
                        break;
                    case AircraftInfoField.InfoProperty.FlapSetting:
                        ddispText = m_plane.Flaps.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.CurrentFlapLift:
                        ddispText = m_plane.FlapLift.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.CurrentFlapDrag:
                        ddispText = m_plane.FlapDrag.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Landing Gear Stats
                    case AircraftInfoField.InfoProperty.GEARSYSINFO:
                        break;
                    case AircraftInfoField.InfoProperty.LandingGearExtended:
                        ddispText = m_plane.GearExtended.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.WheelAmount:
                        ddispText = m_plane.WheelsAmount.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Physics
                    case AircraftInfoField.InfoProperty.AIRCRAFTPHYSICS:
                        break;
                    case AircraftInfoField.InfoProperty.Forces:
                        ddispText = m_plane.Forces.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.LiftPower:
                        ddispText = m_plane.LiftPower.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.LiftDirection:
                        ddispText = m_plane.LiftDirection.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Flight States
                    case AircraftInfoField.InfoProperty.FLIGHTSTATES:
                        break;
                    case AircraftInfoField.InfoProperty.Flying:
                        ddispText = m_plane.AircraftStateFlying.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Stall:
                        ddispText = m_plane.AircraftStateStalled.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Landing:
                        ddispText = m_plane.AircraftStateLanding.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Takeoff:
                        ddispText = m_plane.AircraftStateTakingOff.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Ground:
                        ddispText = m_plane.AircraftStateGrounded.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Crash:
                        ddispText = m_plane.AircraftStateCrashed.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Stationary:
                        ddispText = m_plane.AircraftStateStationary.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.Parked:
                        ddispText = m_plane.AircraftStateParked.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    #endregion
                    #region Environment Stats
                    case AircraftInfoField.InfoProperty.ENVIRONMENTCONDITIONS:
                        break;
                    case AircraftInfoField.InfoProperty.SealevelAirtemperature:
                        ddispText = m_plane.CurrentSeaLevelTemperature.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.SealevelRelativeHumidity:
                        ddispText = m_plane.CurrentSeaLevelHumidity.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.SealevelAirpressure:
                        ddispText = m_plane.CurrentSeaLevelAirpressure.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.SealevelAirdensity:
                        ddispText = m_plane.CurrentSeaLevelAirDensity.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.FalloffRateAirtemperature:
                        ddispText = m_plane.TemperatureFalloffRate.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.FalloffRateRelativeHumidity:
                        ddispText = m_plane.HumidityFalloffRate.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AltitudeAirtemperature:
                        ddispText = m_plane.AltitudeTemperature.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AltitudeRelativeHumidity:
                        ddispText = m_plane.AltitudeHumidity.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AltitudeAirpressure:
                        ddispText = m_plane.AltitudeAirpressure.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AltitudeAirdensity:
                        ddispText = m_plane.AltitudeAirdensity.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.AltitudeWindFactor:
                        ddispText = m_plane.AltitudeWindFactor.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.WindStrengthFactor:
                        ddispText = m_plane.WindStrength.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ZoneWindStrength:
                        ddispText = m_plane.ZoneWindStrength.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ZoneWindDirection:
                        break;
                    #endregion
                    #region Automatics
                    case AircraftInfoField.InfoProperty.ATOnline:
                        ddispText = m_plane.AtOnline.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATEngaged:
                        ddispText = m_plane.AtInCommand.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATModeMaintain:
                        ddispText = m_plane.AtModeMaintain.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATModeThrust:
                        ddispText = m_plane.AtModeThrust.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATTarget:
                        ddispText = m_plane.AtTarget.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATToggleInput:
                        ddispText = m_plane.AtToggleInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATModeInput:
                        ddispText = m_plane.AtModeInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.ATOverrideInput:
                        ddispText = m_plane.AtOverrideInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APOnline:
                        ddispText = m_plane.ApOnline.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APEngaged:
                        ddispText = m_plane.ApInCommand.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APModeAltitude:
                        ddispText = m_plane.ApModeAltitude.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APModeHeading:
                        ddispText = m_plane.ApModeHeading.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APAltitudeTarget:
                        ddispText = m_plane.ApAltitudeTarget.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APVerticalSpeedTarget:
                        ddispText = Mathf.Abs(m_plane.ApVsTarget).ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APHeadingTarget:
                        ddispText = m_plane.ApHeadingTarget.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                    case AircraftInfoField.InfoProperty.APToggleInput:
                        ddispText = m_plane.ApToggleInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;
                        case AircraftInfoField.InfoProperty.APOverrideInput:
                        ddispText = m_plane.ApOverrideInput.ToString();
                        setDisplayText(ddispText, ddispField);
                        break;

                        #endregion
                }

            }            
        }

        private void setDisplayText(string valueText, debugDispField valueField)
        {            
            Color ddispColor;
            if (valueText == "True")
            {
                ddispColor = textTrueColor;
            }
            else if(valueText == "False")
            {
                ddispColor = textFalseColor;
            }
            else
            {
                ddispColor = textNormalColor;
            }
            valueField.displayDebugValue(valueText, ddispColor);
        }
        
            public void toggleDebugPanel(bool input)
        {
            if(input)
            {
                toggled = !toggled;
                debugPanel.SetActive(toggled);
            }       
        }
             

        [Serializable]
        public class AircraftInfoField
        {
            public enum InfoProperty
            {
                [InspectorName("")]
                USERINPUTS,
                PitchInput,
                YawInput,
                RollInput,
                ThrottleInput,
                AirbrakesInput,
                FlapsInput,
                LandingGearInput,
                ToGaInput,

                [InspectorName("")]
                AIRCRAFTTRANSFORM,
                Pitch,
                Yaw,
                Roll,

                [InspectorName("")]
                AIRCRAFTDIR,
                Altitude,
                ForwardSpeed,
                VerticalSpeed,

                [InspectorName("")]
                ENGINESYSINFO,
                EngineOn,
                ToGaMode,
                ThrottleSetting,
                RequestedEnginePowerOutput,
                CurrentEnginePowerOutput,
                EngineEfficiencyFactor,

                [InspectorName("")]
                FUELSYSINFO,
                FuelRemaining,
                FuelBurnRate,

                [InspectorName("")]
                FLAPSYSINFO,
                FlapSetting,
                CurrentFlapLift,
                CurrentFlapDrag,

                [InspectorName("")]
                GEARSYSINFO,
                LandingGearExtended,
                WheelAmount,

                [InspectorName("")]
                AIRCRAFTPHYSICS,
                Forces,
                LiftPower,
                LiftDirection,

                [InspectorName("")]
                FLIGHTSTATES,
                Flying,
                Stall,
                Landing,
                Takeoff,
                Ground,
                Crash,
                Stationary,
                Parked,

                [InspectorName("")]
                ENVIRONMENTCONDITIONS,
                SealevelAirtemperature,
                SealevelRelativeHumidity,
                SealevelAirpressure,
                SealevelAirdensity,
                FalloffRateAirtemperature,
                FalloffRateRelativeHumidity,
                AltitudeAirtemperature,
                AltitudeRelativeHumidity,
                AltitudeAirpressure,
                AltitudeAirdensity,
                AltitudeWindFactor,
                WindStrengthFactor,
                ZoneWindStrength,
                ZoneWindDirection,

                [InspectorName("")]
                AUTOMATICS,
                ATOnline,
                ATEngaged,
                ATModeMaintain,
                ATModeThrust,
                ATTarget,
                ATToggleInput,
                ATModeInput,
                ATOverrideInput,
                APOnline,
                APEngaged,
                APModeAltitude,
                APModeHeading,
                APAltitudeTarget,
                APVerticalSpeedTarget,
                APHeadingTarget,
                APToggleInput,
                APOverrideInput,
            }
            public InfoProperty p_info;
        }
    }



}
