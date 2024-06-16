using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using Viguar.EditorTooling.DataContainers.Curve;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
using Viguar.EditorTooling.DataContainers.Curve;
#endif

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftConfiguration : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        public ConfigAerodynamics _cAerodynamics;
        public ConfigConstraintsValues _cConstaints;
        public ConfigConstraintsStateValues _cConState;
        public ConfigControlSurfaces _cControlSurfaces;
        public ConfigEngines _cEngines;
        public ConfigLandingGear _cLandingGear;
        public ConfigFuel _cFuel;
        public ConfigEnvironmentImpact _cEnvironment;
        public ConfigConstraintsAutomatics _cAutomatics;
        public ConfigStartSettings _cStart;
        public ConfigAvionics _cAvionics;
        public ConfigControlSurfaceAnimations _cCSAnimations;

        public bool _configAerodynamics;
        public bool _configControlSurfaces;
        public bool _configEngines;
        public bool _configLandingGear;
        public bool _configEnvironment;
        public bool _configAutomatics;        

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            ConfigureAircraft();            
        }

        void ConfigureAircraft()
        {
            if (_configAerodynamics) { ConfigApplyAerodynamics(); }
            if (_configControlSurfaces) { ConfigApplyControlSurfaces(); }
            if (_configEngines) { ConfigApplyEngines(); }
            if (_cFuel._cEnginesBurnFuel) { ConfigApplyFuel(); }
            if (_configLandingGear) { ConfigApplyLandingGear(); }        
            if (_configEnvironment) { ConfigApplyEnvironment(); }
            if (_configAutomatics) { ConfigApplyAutoFlightSystem(); }

            ConfigApplyConstraints();
            ConfigApplyConstraintsFlightState();
            ConfigApplyConstraintsFlightStateCategories();
            ConfigApplyConstraintsAutomatics();
            ConfigApplyAnimations();
            _configBaseProcessor.ProcessStartConfiguation(_cStart._cEngineOn, _cStart._cCustomFuelAmount, _cStart._cStartFuel, _cStart._cCustomFuelAmount, _cStart._cStartPos, _cStart._cVelocity, _cStart._cStartVelocity, _cStart._cGearDown);            
        }

        void ConfigApplyAerodynamics()
        {
            _configBaseProcessor.ProcessAerodynamicConfiguration(_cAerodynamics._cYSeaLevel, _cAerodynamics._cMaxAltitude, _cAerodynamics._cMaxLiftSpeed, _cAerodynamics._cAerodynamicEffect, _cAerodynamics._cLift, _cAerodynamics._cDragOverSpeed, _cAerodynamics._cCustomCOM, _cAerodynamics._cCustomCOMPos, _cAerodynamics._cLiftSpeedFactor);
        }
        void ConfigApplyControlSurfaces()
        {
            if(_cControlSurfaces._cOverrideFlapLiftCurve != null) { _cControlSurfaces._cFlapLiftOverSpeed = _cControlSurfaces._cOverrideFlapLiftCurve.Curve; } //Override if the field is not empty.
            if (_cControlSurfaces._cOverrideFlapDragCurve != null) { _cControlSurfaces._cFlapDragOverSpeed = _cControlSurfaces._cOverrideFlapDragCurve.Curve; }
            _configBaseProcessor.ProcessControlSurfaceConfiguration(_cControlSurfaces._cHasSurfaceElevator, _cControlSurfaces._cElevatorResponse, _cControlSurfaces._cHasSurfaceRudder, _cControlSurfaces._cRudderResponse, _cControlSurfaces._cHasSurfaceAilerons, _cControlSurfaces._cAileronResponse, _cControlSurfaces._cBankingTurnResponse, _cControlSurfaces._cHasSurfaceAirbrakes, _cControlSurfaces._cAirbrakeResponse, _cControlSurfaces._cHasSurfaceFlaps, _cControlSurfaces._cFlapResponse, _cControlSurfaces._cFlapSteps, _cControlSurfaces._cFlapLiftOverSpeed, _cControlSurfaces._cFlapDragOverSpeed);
        }
        void ConfigApplyEngines()
        {
            if (_cEngines._cOverrideSpoolCurve != null) { _cEngines._cEngineSpoolCurve = _cEngines._cOverrideSpoolCurve.Curve; } //Override if the field is not empty.
            if (_cEngines._cEngineConfig == ConfigEngines._cEngineConfiguration.SingleEngine)
            {
                _configBaseProcessor.ProcessSingleEngineConfiguration(_cEngines._cEngineConfig, _cEngines._cPropulsion, _cEngines._cMaxEngineThrust, _cEngines._cMaxTogaThrust, _cEngines._cEngineSpoolCurve, _cEngines._cSingleEnginePosition);
            }
            else
            {
                _configBaseProcessor.ProcessMultiEngineConfiguration(_cEngines._cEngineConfig, _cEngines._cPropulsion, _cEngines._cMaxEngineThrust, _cEngines._cMaxTogaThrust, _cEngines._cEngineSpoolCurve, _cEngines._cMultiEngineProperties);
            }
        }
        void ConfigApplyFuel()
        {
            if (_cFuel._cOverrideFuelBurnCurve != null) { _cFuel._cFuelBurnAtEngineThrust = _cFuel._cOverrideFuelBurnCurve.Curve; } //Override if the field is not empty.
            _configBaseProcessor.ProcessFuelConfiguration(_cFuel._cEnginesBurnFuel, _cFuel._cMaximumFuelCapacity, _cFuel._cFuelBurnAtEngineThrust);
        }
        void ConfigApplyLandingGear()
        {
            _configBaseProcessor.ProcessLandingGearConfiguration(_cLandingGear._cLandingGearType , _cLandingGear._cLandingGearLoweredDrag, _cLandingGear._cLandingGearBrakeStrength, _cLandingGear._cLandingGearMaxSteeringAngle, _cLandingGear._cLandingGearSteeringColumn, _cLandingGear._cWheelBrakeTempSec);
        }
        void ConfigApplyEnvironment()
        {
            if(_cEnvironment._cOverrideAltiCurve != null) { _cEnvironment._cAltitudeResponseEfficiency = _cEnvironment._cOverrideAltiCurve.Curve; }
            if (_cEnvironment._cOverrideTempCurve != null) { _cEnvironment._cTemperatureResponseEfficiency = _cEnvironment._cOverrideTempCurve.Curve; }
            if (_cEnvironment._cOverrideDensityCurve != null) { _cEnvironment._cDensityResponseEfficiency = _cEnvironment._cOverrideDensityCurve.Curve; }
            _configBaseProcessor.ProcessEnvironmentConfiguration(_configEnvironment, _cEnvironment._cAtmosphericEffect, _cEnvironment._cWindEffect, _cEnvironment._cPrecipitationEffect, _cEnvironment._cTemperatureAltitudeFalloff, _cEnvironment._cHumidityAltitudeFalloff, _cEnvironment._cWindAltitudeIncrease, _cEnvironment._cWindStrengthMultiplier, _cEnvironment._cAltitudeResponseEfficiency, _cEnvironment._cTemperatureResponseEfficiency, _cEnvironment._cDensityResponseEfficiency);
        }
        void ConfigApplyAutoFlightSystem()
        {
            _configBaseProcessor.ProcessAutomaticsConfiguration();
        }

        void ConfigApplyConstraints()
        {
            _configBaseProcessor.ProcessConstraintsConfiguration(_cConstaints._cCriticalWarningPercentage, _cConstaints._cMaxStablePitchAngleDeg, _cConstaints._cMaxStableRollAngleDeg, _cConstaints._cMinMaxStableForwardSpeed, _cConstaints._cMaxStableVerticalSpeed, _cConstaints._cMinStableAltitude);
        }
        void ConfigApplyConstraintsFlightState()
        {
            _configBaseProcessor.ProcessConstraintsFlightStatesConfiguration(_cConState._cStallSensitivity, _cConState._cStationaryDetectionSpeedMinMax, _cConState._cTakeoffDetectionSpeedMinMax, _cConState._cLandingDetectionSpeedMinMax, _cConState._cLandingDetectionVSpeedMinMax, _cConState._cLandingDetectionAltitudeMax);
        }
        void ConfigApplyConstraintsFlightStateCategories()
        {
            _configBaseProcessor.ProcessConstraintsFlightStateCategoriesConfiguration(_cConState._cLandingCategories, _cConState._cStableAttitudeCategories, _cConState._cStableSpeedCategories);
        }
        void ConfigApplyConstraintsAutomatics()
        {
            _configBaseProcessor.ProcessConstraintsAutomatics(_cAutomatics._cAPVSpeedLimit, _cAutomatics._cAPPitchAngleLimit, _cAutomatics._cAPRollAngleLimit, _cAutomatics._cATForwardSpeedLimit);
        }

        void ConfigApplyAnimations()
        {
            _configBaseProcessor.ProcessAnimationConfiguration(_cControlSurfaces._cAnimateControlSurfaces, _cAvionics._cAnimateAvionics, _cCSAnimations, _cAvionics);
        }
    }
}

#if UNITY_EDITOR
namespace Viguar.Aircraft
{
    [CustomEditor(typeof(AircraftConfiguration)), InitializeOnLoadAttribute]
    public class AircraftConfigurationEditor : Editor
    {
        bool showFeatureConfig = false;
        bool showAerodynamics = false;
        bool showConstraints = false;
        bool showConstraintsStates = false;
        bool showControlSurfaces = false;
        bool showEngines = false;
        bool showLandingGear = false;
        bool showFuel = false;
        bool showEnvironment = false;
        bool showEnvFallback = false;
        bool showAutomatics = false;
        bool showAnimations = false;
        bool showStart = false;

        AircraftConfiguration sts;
        SerializedObject SerSTS;

        private void OnEnable()
        {
            sts = (AircraftConfiguration)target;
            SerSTS = new SerializedObject(sts);
        }
        public override void OnInspectorGUI()
        {
            SerSTS.Update();

            #region Aircraft Features
            GUILayout.Label("Aircraft Features", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle      
            showFeatureConfig = EditorGUILayout.Foldout(showFeatureConfig, "Aircraft Features");
            if (showFeatureConfig)
            {
                sts._configAerodynamics = EditorGUILayout.ToggleLeft(new GUIContent("Aerodynamic Configuration", "Toggle for aerodynamic behavior configuration."), sts._configAerodynamics);
                sts._configControlSurfaces = EditorGUILayout.ToggleLeft(new GUIContent("Control Surface Configuration", "Toggle for control surface configuration."), sts._configControlSurfaces);
                sts._configEngines = EditorGUILayout.ToggleLeft(new GUIContent("Engine Configuration", "Toggle for engine configuration."), sts._configEngines);
                sts._configLandingGear = EditorGUILayout.ToggleLeft(new GUIContent("Landing Gear Configuration", "Toggle for landing gear configuation."), sts._configLandingGear);
                sts._configEnvironment = EditorGUILayout.ToggleLeft(new GUIContent("Environment Impact", "Toggle for environment impacting flight."), sts._configEnvironment);
                sts._configAutomatics = EditorGUILayout.ToggleLeft(new GUIContent("Automatic Flight Systems", "Toggle for having a working autopilot."), sts._configAutomatics);
                EditorGUILayout.Space();
            }
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            #endregion

            #region Flight Config
            GUILayout.Label("Flight Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle      
            if (sts._configAerodynamics)
            {
                showAerodynamics = EditorGUILayout.Foldout(showAerodynamics, "[Flight Config.] Aerodynamics");
                if (showAerodynamics)
                {
                    EditorGUI.indentLevel++;
                    sts._cAerodynamics._cYSeaLevel = EditorGUILayout.Slider(new GUIContent("World Space Y Sea Level", "The Y coordinate representing sea level."), sts._cAerodynamics._cYSeaLevel, -10000f, 10000f);
                    sts._cAerodynamics._cMaxAltitude = EditorGUILayout.Slider(new GUIContent("Max. Altitude", "The maximum altitude at which the aircraft generates lift."), sts._cAerodynamics._cMaxAltitude, 0f, 100000f);
                    sts._cAerodynamics._cMaxLiftSpeed = EditorGUILayout.Slider(new GUIContent("Max. Lift Generating Speed", "The maximum speed at which the aircraft generates lift."), sts._cAerodynamics._cMaxLiftSpeed, 0f, 1000f);
                    sts._cAerodynamics._cAerodynamicEffect = EditorGUILayout.Slider(new GUIContent("Aerodynamic Effect", "How much the aircraft bends its velocity towards its facing direction."), sts._cAerodynamics._cAerodynamicEffect, 0f, 10f);
                    sts._cAerodynamics._cLift = EditorGUILayout.Slider(new GUIContent("Lift", "A multiplier factored into the lift/speed."), sts._cAerodynamics._cLift, 0f, 100f);
                    sts._cAerodynamics._cDragOverSpeed = EditorGUILayout.Slider(new GUIContent("Drag / Speed", "The increase of drag relative to aircraft speed."), sts._cAerodynamics._cDragOverSpeed, 0f, 1f);
                    sts._cAerodynamics._cCustomCOM = EditorGUILayout.Toggle(new GUIContent("Custom Center Of Mass", "Overrides the Rigidbodies' center of mass."), sts._cAerodynamics._cCustomCOM);
                    if(sts._cAerodynamics._cCustomCOM)
                    {
                        sts._cAerodynamics._cCustomCOMPos = EditorGUILayout.ObjectField(new GUIContent("Center Of Mass", "The override position of the center of mass."), sts._cAerodynamics._cCustomCOMPos, typeof(Transform), true) as Transform;
                    }             
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                EditorGUI.EndFoldoutHeaderGroup();                
            }
            showConstraintsStates = EditorGUILayout.Foldout(showConstraintsStates, "[Flight Config.] Flight State Constraints/Detection");
            if (showConstraintsStates)
            {
                sts._cConstaints._cCriticalWarningPercentage = EditorGUILayout.Slider(new GUIContent("Stable / Critical Percentage", "At which point a stable attitude turns into a critical one."), sts._cConstaints._cCriticalWarningPercentage, 0f, 1f);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;                
                GUILayout.Label("Flight Op. Detection", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                sts._cConstaints._cMaxStablePitchAngleDeg = EditorGUILayout.Slider(new GUIContent("Max. Stable Pitch Angle", "The maximum safe pitch angle of the aircraft."), sts._cConstaints._cMaxStablePitchAngleDeg, 0f, 90);
                sts._cConstaints._cMaxStableRollAngleDeg = EditorGUILayout.Slider(new GUIContent("Max. Stable Roll Angle", "The maximum safe roll angle of the aircraft."), sts._cConstaints._cMaxStableRollAngleDeg, 0f, 90);
                sts._cConstaints._cMinMaxStableForwardSpeed = EditorGUILayout.Vector2Field(new GUIContent("Stable Airspeed Regime", "The safe airspeed regimes of the aircraft."), sts._cConstaints._cMinMaxStableForwardSpeed);
                sts._cConstaints._cMaxStableVerticalSpeed = EditorGUILayout.Slider(new GUIContent("Max. Stable Vertical Speed", "The safe descent/climb rate of the aircraft."), sts._cConstaints._cMaxStableVerticalSpeed, 0f, 100);
                sts._cConstaints._cMinStableAltitude = EditorGUILayout.Slider(new GUIContent("Min. Stable Altitude", "The lowest safest altitude of the aircraft."), sts._cConstaints._cMinStableAltitude, 0f, 100);
                sts._cConState._cStallSensitivity = EditorGUILayout.Slider(new GUIContent("Stall Sensitivity", "."), sts._cConState._cStallSensitivity, 0f, 1f);
                EditorGUILayout.Space();

                GUILayout.Label("Ground Op. Detection", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                sts._cConState._cStationaryDetectionSpeedMinMax = EditorGUILayout.Vector2Field(new GUIContent("Stationary Speed Regime", "The speed regime for the aircraft to be considered stationary."), sts._cConState._cStationaryDetectionSpeedMinMax);
                sts._cConState._cTakeoffDetectionSpeedMinMax = EditorGUILayout.Vector2Field(new GUIContent("Takeoff Speed Regime", "The speed regime for the aircraft to be considered taking off."), sts._cConState._cTakeoffDetectionSpeedMinMax);
                EditorGUILayout.Space();

                GUILayout.Label("Landing Detection", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                sts._cConState._cLandingDetectionOnlyNearRunways = EditorGUILayout.Toggle(new GUIContent("Land. Det. Only At Runways", "Landing approach only gets detected when near a runway zone."), sts._cConState._cLandingDetectionOnlyNearRunways);
                sts._cConState._cLandingDetectionAltitudeMax = EditorGUILayout.Slider(new GUIContent("Landing Altitude Regime", "The minimum altitude for landing approach detection."), sts._cConState._cLandingDetectionAltitudeMax, 0f, 500);
                sts._cConState._cLandingDetectionSpeedMinMax = EditorGUILayout.Vector2Field(new GUIContent("Landing Detection Speed Regime", "The speed regime for landing approach detection."), sts._cConState._cLandingDetectionSpeedMinMax);
                sts._cConState._cLandingDetectionVSpeedMinMax = EditorGUILayout.Vector2Field(new GUIContent("Landing Detection Vertical Speed Regime", "The descent rate for landing approach detection."), sts._cConState._cLandingDetectionVSpeedMinMax);    
                EditorGUILayout.Space();

                GUILayout.Label("Flight Stability Detection", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(SerSTS.FindProperty("_cConState._cStableAttitudeCategories"), true);
                EditorGUILayout.PropertyField(SerSTS.FindProperty("_cConState._cStableSpeedCategories"), true);
                EditorGUILayout.PropertyField(SerSTS.FindProperty("_cConState._cLandingCategories"), true);
                EditorGUI.indentLevel--;
            }
            showStart = EditorGUILayout.Foldout(showStart, "[Flight Config.] Start Settings");
            if (showStart)
            {
                sts._cStart._cEngineOn = EditorGUILayout.ToggleLeft(new GUIContent("Engine On At Start", "."), sts._cStart._cEngineOn);
                sts._cStart._cCustomFuelAmount = EditorGUILayout.ToggleLeft(new GUIContent("Custom Fuel Amount At Start", "Allows to set a custom amount of starting fuel for the aircraft."), sts._cStart._cCustomFuelAmount);
                if(sts._cStart._cCustomFuelAmount)
                {
                    sts._cStart._cStartFuel = EditorGUILayout.Slider(new GUIContent("Amount", "The amount of fuel at start."), sts._cStart._cStartFuel, 0, 10000);
                }
                sts._cStart._cCustomPosition = EditorGUILayout.ToggleLeft(new GUIContent("Start At Custom Position", "Allows to set a custom starting Transform position for the aircraft."), sts._cStart._cCustomPosition);
                if(sts._cStart._cCustomPosition)
                {
                    sts._cStart._cStartPos = EditorGUILayout.ObjectField(new GUIContent("Start Position", "The starting position of the aircraft."), sts._cStart._cStartPos, typeof(Transform), true) as Transform;
                }
                sts._cStart._cVelocity = EditorGUILayout.ToggleLeft(new GUIContent("Start With Custom Velocity", "Allows to set a custom starting velocity for the aircraft."), sts._cStart._cVelocity);
                if(sts._cStart._cVelocity)
                {
                    sts._cStart._cStartVelocity = EditorGUILayout.Vector3Field(new GUIContent("Start Velocity", "The starting velocity of the aircraft."), sts._cStart._cStartVelocity);
                }
                sts._cStart._cGearDown = EditorGUILayout.ToggleLeft(new GUIContent("Start With Gear Extended", "Sets the aircrafts landing gear to either retracted or extended."), sts._cStart._cGearDown);
            }
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            #endregion
            #region Flight Controls Config
            GUILayout.Label("Flight Controls Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle      
            if (sts._configControlSurfaces)
            {
                showControlSurfaces = EditorGUILayout.Foldout(showControlSurfaces, "[Aircraft Config.] Control Surfaces");
                if(showControlSurfaces)
                {
                    EditorGUI.indentLevel++;
                    sts._cControlSurfaces._cAnimateControlSurfaces = EditorGUILayout.Toggle(new GUIContent("Animated", "."), sts._cControlSurfaces._cAnimateControlSurfaces);
                    EditorGUI.indentLevel--;
                    GUILayout.Label("Directional Control Surface Properties", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel++;
                    sts._cControlSurfaces._cHasSurfaceElevator = EditorGUILayout.Toggle(new GUIContent("Has Elevators", "."), sts._cControlSurfaces._cHasSurfaceElevator);
                    sts._cControlSurfaces._cHasSurfaceRudder = EditorGUILayout.Toggle(new GUIContent("Has Rudder", "."), sts._cControlSurfaces._cHasSurfaceRudder);
                    sts._cControlSurfaces._cHasSurfaceAilerons = EditorGUILayout.Toggle(new GUIContent("Has Ailerons", "."), sts._cControlSurfaces._cHasSurfaceAilerons);
                    EditorGUILayout.Space();
                    if (sts._cControlSurfaces._cHasSurfaceElevator)
                    {
                        sts._cControlSurfaces._cElevatorResponse = EditorGUILayout.Slider(new GUIContent("Elevator (Pitch) Response", "."), sts._cControlSurfaces._cElevatorResponse, 0f, 5000f);                        
                    }                                                          
                    if (sts._cControlSurfaces._cHasSurfaceRudder)
                    {
                        sts._cControlSurfaces._cRudderResponse = EditorGUILayout.Slider(new GUIContent("Rudder (Yaw) Response", "."), sts._cControlSurfaces._cRudderResponse, 0f, 5000f);
                    }                   
                    if(sts._cControlSurfaces._cHasSurfaceAilerons)
                    {
                        sts._cControlSurfaces._cAileronResponse = EditorGUILayout.Slider(new GUIContent("Aileron (Roll) Response", "."), sts._cControlSurfaces._cAileronResponse, 0f, 5000f);
                        sts._cControlSurfaces._cBankingTurnResponse = EditorGUILayout.Slider(new GUIContent("Banking Turn Response", "."), sts._cControlSurfaces._cBankingTurnResponse, 0f, 5000f);
                    }                                                                 
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    
                    GUILayout.Label("Lift Spoiling Control Surface Properties", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel++;
                    sts._cControlSurfaces._cHasSurfaceAirbrakes = EditorGUILayout.ToggleLeft(new GUIContent("Has Airbrake", "Does the Aircraft have an Airbrake?"), sts._cControlSurfaces._cHasSurfaceAirbrakes);
                    if(sts._cControlSurfaces._cHasSurfaceAirbrakes)
                    {
                        sts._cControlSurfaces._cAirbrakeResponse = EditorGUILayout.Slider(new GUIContent("Airbrake Response", "Increases Current Drag by += Airbrake Response * Current Drag"), sts._cControlSurfaces._cAirbrakeResponse, 0f, 1f);
                    }                  
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();

                    GUILayout.Label("Lift Aiding Control Surface Properties", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 11 }, GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel++;
                    sts._cControlSurfaces._cHasSurfaceFlaps = EditorGUILayout.ToggleLeft(new GUIContent("Has Flaps / Slats", "."), sts._cControlSurfaces._cHasSurfaceFlaps);
                    if(sts._cControlSurfaces._cHasSurfaceFlaps)
                    {
                        sts._cControlSurfaces._cFlapResponse = EditorGUILayout.Slider(new GUIContent("Flap & Slat Response", "."), sts._cControlSurfaces._cFlapResponse, 1f, 5f);
                        EditorGUILayout.PropertyField(SerSTS.FindProperty("_cControlSurfaces._cFlapSteps"), true);
                        sts._cControlSurfaces._cFlapLiftOverSpeed = EditorGUILayout.CurveField(new GUIContent("Flap Lift / Speed", "."), sts._cControlSurfaces._cFlapLiftOverSpeed);
                        EditorGUI.indentLevel++;
                        sts._cControlSurfaces._cOverrideFlapLiftCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cControlSurfaces._cOverrideFlapLiftCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                        sts._cControlSurfaces._cFlapDragOverSpeed = EditorGUILayout.CurveField(new GUIContent("Flap Drag / Speed", "."), sts._cControlSurfaces._cFlapDragOverSpeed);
                        EditorGUI.indentLevel++;
                        sts._cControlSurfaces._cOverrideFlapDragCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cControlSurfaces._cOverrideFlapDragCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                    }                   
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                EditorGUI.EndFoldoutHeaderGroup();
            }
            if (sts._configEngines)
            {
                showEngines = EditorGUILayout.Foldout(showEngines, "[Aircraft Config.] Engines");
                if (showEngines)
                {                   
                    EditorGUI.indentLevel++;
                    GUILayout.Label("Aircraft Engine Layout Properties", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 11 }, GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel++;
                    sts._cFuel._cEnginesBurnFuel = EditorGUILayout.Toggle(new GUIContent("Burn Fuel", "."), sts._cFuel._cEnginesBurnFuel);
                    sts._cEngines._cEngineConfig = (ConfigEngines._cEngineConfiguration)EditorGUILayout.EnumPopup(sts._cEngines._cEngineConfig);
                    sts._cEngines._cPropulsion = (ConfigEngines._cPropulsionType)EditorGUILayout.EnumPopup(sts._cEngines._cPropulsion);
                    sts._cEngines._cMaxEngineThrust = EditorGUILayout.Slider(new GUIContent("Max. Thrust per Engine", "."), sts._cEngines._cMaxEngineThrust, 0f, 100000f);
                    sts._cEngines._cMaxTogaThrust = EditorGUILayout.Slider(new GUIContent("Max. Add. Toga Thrust per Engine", "."), sts._cEngines._cMaxTogaThrust, 0f, 100000f);
                    sts._cEngines._cEngineSpoolCurve = EditorGUILayout.CurveField(new GUIContent("Engine Spoolup Curve", "."), sts._cEngines._cEngineSpoolCurve);
                    EditorGUI.indentLevel++;
                    sts._cEngines._cOverrideSpoolCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cEngines._cOverrideSpoolCurve, typeof(CurveContainer), true) as CurveContainer;
                    EditorGUI.indentLevel--;
                    if (sts._cEngines._cEngineConfig == ConfigEngines._cEngineConfiguration.SingleEngine)
                    {
                        sts._cEngines._cSingleEnginePosition = EditorGUILayout.ObjectField(new GUIContent("Engine Position", "."), sts._cEngines._cSingleEnginePosition, typeof(Transform), true) as Transform;
                    }
                    if(sts._cEngines._cEngineConfig == ConfigEngines._cEngineConfiguration.MultiEngine)
                    {
                        EditorGUILayout.PropertyField(SerSTS.FindProperty("_cEngines._cMultiEngineProperties"), true);
                    }
                    EditorGUILayout.Space();
                    if(sts._cFuel._cEnginesBurnFuel)
                    {
                        sts._cFuel._cMaximumFuelCapacity = EditorGUILayout.Slider(new GUIContent("Maximum Fuel Capacity", "."), sts._cFuel._cMaximumFuelCapacity, 0f, 100000f);
                        sts._cFuel._cFuelBurnAtEngineThrust = EditorGUILayout.CurveField(new GUIContent("Fuel Burn Over Engine Thrust", "."), sts._cFuel._cFuelBurnAtEngineThrust);
                        EditorGUI.indentLevel++;
                        sts._cFuel._cOverrideFuelBurnCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cFuel._cOverrideFuelBurnCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                EditorGUI.EndFoldoutHeaderGroup();
            }
            if (sts._configLandingGear)
            {
                showLandingGear = EditorGUILayout.Foldout(showLandingGear, "[Aircraft Config.] Landing Gear");
                if (showLandingGear)
                {                
                    EditorGUI.indentLevel++;               
                    sts._cLandingGear._cLandingGearType = (ConfigLandingGear._cLandingGearTypes)EditorGUILayout.EnumPopup(sts._cLandingGear._cLandingGearType);
                    if (sts._cLandingGear._cLandingGearType == ConfigLandingGear._cLandingGearTypes.RetractableGear)
                    {
                        //sts._cLandingGear._cAnimateLandingGear = EditorGUILayout.Toggle(new GUIContent("Animated", "."), sts._cLandingGear._cAnimateLandingGear);
                        sts._cLandingGear._cLandingGearLoweredDrag = EditorGUILayout.Slider(new GUIContent("Extra Drag On Extended", "Increases Current Drag by += Landing Gear Drag * Current Drag"), sts._cLandingGear._cLandingGearLoweredDrag, 0f, 1f);
                    }
                    sts._cLandingGear._cLandingGearBrakeStrength = EditorGUILayout.Slider(new GUIContent("Braking Strength per Wheel", "."), sts._cLandingGear._cLandingGearBrakeStrength, 0f, 250f);
                    sts._cLandingGear._cWheelBrakeTempSec = EditorGUILayout.Slider(new GUIContent("Brake Temperature Increase / Second", "."), sts._cLandingGear._cWheelBrakeTempSec, 0f, 150f);
                    sts._cLandingGear._cLandingGearMaxSteeringAngle = EditorGUILayout.Slider(new GUIContent("Maximum Steering Angle", "."), sts._cLandingGear._cLandingGearMaxSteeringAngle, 0f, 90f);
                    sts._cLandingGear._cLandingGearSteeringColumn = EditorGUILayout.ObjectField(new GUIContent("Steering Column", "."), sts._cLandingGear._cLandingGearSteeringColumn, typeof(GameObject), true) as GameObject;                                        
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                EditorGUI.EndFoldoutHeaderGroup();
            }
            #endregion

            GUILayout.Label("Environment Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle      
            if (sts._configEnvironment)
            {
                showEnvironment = EditorGUILayout.Foldout(showEnvironment, "[Aircraft Config.] Environment Impact");
                if(showEnvironment)
                {
                    sts._cEnvironment._cAtmosphericEffect = EditorGUILayout.Toggle(new GUIContent("Atmospheric Effect", "."), sts._cEnvironment._cAtmosphericEffect);
                    sts._cEnvironment._cWindEffect = EditorGUILayout.Toggle(new GUIContent("Wind Effect", "."), sts._cEnvironment._cWindEffect);
                    sts._cEnvironment._cPrecipitationEffect = EditorGUILayout.Toggle(new GUIContent("Precipitation Effect", "."), sts._cEnvironment._cPrecipitationEffect);
                    EditorGUILayout.Space();
                    if (sts._cEnvironment._cAtmosphericEffect)
                    {
                        EditorGUI.indentLevel++;
                        GUILayout.Label("Atmospheric Effects", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                        sts._cEnvironment._cTemperatureAltitudeFalloff = EditorGUILayout.Slider(new GUIContent("Temperature Falloff / 100m Altitude", "."), sts._cEnvironment._cTemperatureAltitudeFalloff, 0f, 1f);
                        sts._cEnvironment._cHumidityAltitudeFalloff = EditorGUILayout.Slider(new GUIContent("Humidity Falloff / 100m Altitude", "."), sts._cEnvironment._cHumidityAltitudeFalloff, 0f, 1f);
                        if(sts._cEnvironment._cWindEffect)
                        {                           
                            sts._cEnvironment._cWindAltitudeIncrease = EditorGUILayout.Slider(new GUIContent("Wind Increase / 100m Altitude", "."), sts._cEnvironment._cWindAltitudeIncrease, 0f, 1f);                            
                        }
                        EditorGUILayout.Space();
                        sts._cEnvironment._cAltitudeResponseEfficiency = EditorGUILayout.CurveField(new GUIContent("Aerodynamic Response / Altitude", "."), sts._cEnvironment._cAltitudeResponseEfficiency);
                        EditorGUI.indentLevel++;
                        sts._cEnvironment._cOverrideAltiCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cEnvironment._cOverrideAltiCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                        sts._cEnvironment._cTemperatureResponseEfficiency = EditorGUILayout.CurveField(new GUIContent("Aerodynamic Response / Air Temperature", "."), sts._cEnvironment._cTemperatureResponseEfficiency);
                        EditorGUI.indentLevel++;
                        sts._cEnvironment._cOverrideTempCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cEnvironment._cOverrideTempCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                        sts._cEnvironment._cDensityResponseEfficiency = EditorGUILayout.CurveField(new GUIContent("Aerodynamic Response / Air Density", "."), sts._cEnvironment._cDensityResponseEfficiency);
                        EditorGUI.indentLevel++;
                        sts._cEnvironment._cOverrideDensityCurve = EditorGUILayout.ObjectField(new GUIContent("Override Curve", "."), sts._cEnvironment._cOverrideDensityCurve, typeof(CurveContainer), true) as CurveContainer;
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                    }                   
                    if(sts._cEnvironment._cWindEffect)
                    {
                        GUILayout.Label("Wind Effects", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                        EditorGUI.indentLevel++;
                        sts._cEnvironment._cWindStrengthMultiplier = EditorGUILayout.Slider(new GUIContent("Wind Strength Multiplier", "."), sts._cEnvironment._cWindStrengthMultiplier, 0f, 100f);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();
                    }                   
                    EditorGUI.indentLevel++;
                    showEnvFallback = EditorGUILayout.Foldout(showEnvFallback, "[Environment Config.] Environment Standard Values");
                    if(showEnvFallback)
                    {
                        sts._cEnvironment._cFallbackSeaLevelAirTemperature = EditorGUILayout.IntSlider(new GUIContent("Standard Sea Level Air Temperature (C°)", "."), sts._cEnvironment._cFallbackSeaLevelAirTemperature, -60, 60);
                        sts._cEnvironment._cFallbackSeaLevelAirPressure = EditorGUILayout.IntSlider(new GUIContent("Standard Sea Level Air Pressure (hPa)", "."), sts._cEnvironment._cFallbackSeaLevelAirPressure, 870, 1084);
                        sts._cEnvironment._cFallbackSeaLevelRelativeHumidity = EditorGUILayout.IntSlider(new GUIContent("Standard Sea Level Relative Humidity (%)", "."), sts._cEnvironment._cFallbackSeaLevelRelativeHumidity, 0, 100);
                        sts._cEnvironment._cFallbackSeaLevelWindStrength = EditorGUILayout.IntSlider(new GUIContent("Standard Sea Level Wind Strength", "."), sts._cEnvironment._cFallbackSeaLevelWindStrength, 0, 100);
                    }
                    EditorGUI.EndFoldoutHeaderGroup();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                EditorGUI.EndFoldoutHeaderGroup();
            }

            GUILayout.Label("Automatic Flight Systems Configuration", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle                
            if (sts._configAutomatics)
            {
                showAutomatics = EditorGUILayout.Foldout(showAutomatics, "A/T, A/P & A/L Configuration");
                if (showAutomatics)
                {
                    EditorGUI.indentLevel++;
                    sts._cAutomatics._cAPVSpeedLimit = EditorGUILayout.Slider(new GUIContent("A/P: Maximum Vertical Speed", "."), sts._cAutomatics._cAPVSpeedLimit, 0f, 100f);
                    sts._cAutomatics._cAPPitchAngleLimit = EditorGUILayout.Vector2Field(new GUIContent("A/P: Maximum Pitch Angles (Nose Up/Down)", "."), sts._cAutomatics._cAPPitchAngleLimit);
                    sts._cAutomatics._cAPRollAngleLimit = EditorGUILayout.Vector2Field(new GUIContent("A/P: Maximum Bank Angles (Bank Left/Right)", "."), sts._cAutomatics._cAPRollAngleLimit);
                    sts._cAutomatics._cATForwardSpeedLimit = EditorGUILayout.Vector2Field(new GUIContent("A/T: Alllowed Selectable Safe Speed Range (Min/Max)", "."), sts._cAutomatics._cATForwardSpeedLimit);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();                   
                }
                EditorGUI.EndFoldoutHeaderGroup();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            GUILayout.Label("Animation Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 12 }, GUILayout.ExpandWidth(true)); //Section SubTitle                
            if (sts._cControlSurfaces._cAnimateControlSurfaces)
            {
                GUILayout.Label("Control Surface Animation Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(SerSTS.FindProperty("_cCSAnimations._cControlSurfacesAnimations"), true);
                EditorGUILayout.Space();
            }
            sts._cAvionics._cAnimateAvionics = EditorGUILayout.Toggle(new GUIContent("Animate Avionics", "."), sts._cAvionics._cAnimateAvionics);
            if (sts._cAvionics._cAnimateAvionics)
            {
                GUILayout.Label("Avionics Animation Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Normal, fontSize = 12 }, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(SerSTS.FindProperty("_cAvionics._cAvionicsInstrument"), true);
                EditorGUILayout.Space();
            }      

            if (GUI.changed)
            {
                EditorUtility.SetDirty(sts);
                Undo.RecordObject(sts, "STS Change");
                SerSTS.ApplyModifiedProperties();
            }
        }
    }
}
#endif