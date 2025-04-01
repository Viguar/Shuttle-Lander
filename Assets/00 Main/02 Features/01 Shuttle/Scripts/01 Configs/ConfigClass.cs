using UnityEngine;
using System;
using Viguar.Inspector.PropertyFields;
using Viguar.EditorTooling.DataContainers;
using UnityEngine.Events;

public class ConfigClass
{

}

#region States & Config
[Serializable]
public class ConfigStartSettings
{
    public bool _cEngineOn;
    public bool _cCustomFuelAmount;
    [DrawIf("_cCustomFuelAmount", true)] public float _cStartFuel;
    public bool _cCustomPosition;
    [DrawIf("_cCustomPosition", true)] public Transform _cStartPos;
    public bool _cVelocity;
    [DrawIf("_cVelocity", true)] public Vector3 _cStartVelocity;
    public bool _cGearDown;
}

[System.Serializable]
public class ConfigConstraintsStateValues
{
    public ConfigConstraintsLandingCategories[] _cLandingCategories;
    public ConfigConstraintsAttitudeStabilityCategories[] _cStableAttitudeCategories;
    public ConfigConstraintsSpeedStabilityCategories[] _cStableSpeedCategories;

    public bool _cLandingDetectionOnlyNearRunways;
    public float _cLandingDetectionAltitudeMax;
    public Vector2 _cLandingDetectionSpeedMinMax;
    public Vector2 _cLandingDetectionVSpeedMinMax;
    public Vector2 _cTakeoffDetectionSpeedMinMax;
    public Vector2 _cStationaryDetectionSpeedMinMax;
    public float _cStallSensitivity;
}

[Serializable]
public class ConfigConstraintsLandingCategories
{
    public enum _cLandType { Normal, Crash, Hard, Fast, Soft, Slow, Good, Perfect, Butter, }
    [LabelOverride("Landing Category")]
    public _cLandType _cLandingCategoryType;
    [LabelOverride("Landing Category Name")]
    public string _cLandingCategoryReferenceName;
    [LabelOverride("Max. Diff. Forward Speed")]
    public float _cLandingCategoryMaxSpeedDifference;
    [LabelOverride("Max. Diff. Vertical Speed")]
    public float _cLandingCategoryMaxVSpeedDifference;
}

[Serializable]
public class ConfigConstraintsAttitudeStabilityCategories
{
    public enum _cAttType { StablePitchAngleRegime, StableBankAngleRegime, StablePitchRate, StableRollRate }
    [LabelOverride("Attitude Category")]
    public _cAttType _cAttitudeCategoryType;
    [LabelOverride("Attitude Category Regime")]
    public Vector2 _cAttitudeCategoryMinMaxRange;
}

[Serializable]
public class ConfigConstraintsSpeedStabilityCategories
{
    public enum _cSpdType { StableFlightSpeedRegime, StableVerticalSpeedRegime, UpsetStallSpeedRegime, UpsetOverspeedRegime }
    [LabelOverride("Speed Category")]
    public _cSpdType _cSpeedCategoryType;
    [LabelOverride("Speed Category Regime")]
    public Vector2 _cSpeedCategoryMinMaxRange;
}

[Serializable]
public class ConfigConstraintsValues
{
    public float _cCriticalWarningPercentage;
    public float _cMaxStablePitchAngleDeg;
    public float _cMaxStableRollAngleDeg;
    public float _cMinStableAltitude;
    public Vector2 _cMinMaxStableForwardSpeed;
    public float _cMaxStableVerticalSpeed;
}


#endregion

#region Aerodynamics
[Serializable]
public class ConfigAerodynamics
{
    public float _cYSeaLevel;
    public float _cMaxAltitude;
    public float _cMaxLiftSpeed;
    public float _cAerodynamicEffect;
    public float _cLift;
    public float _cDragOverSpeed;
    public bool _cCustomCOM;
    public Transform _cCustomCOMPos;

    public AnimationCurve _cLiftSpeedFactor = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
}
[Serializable]
public class ConfigEnvironmentImpact
{
    public bool _cAtmosphericEffect;
    public bool _cWindEffect;
    public bool _cPrecipitationEffect;

    public float _cTemperatureAltitudeFalloff;
    public float _cHumidityAltitudeFalloff;
    public float _cWindAltitudeIncrease;
    public float _cWindStrengthMultiplier;

    public AnimationCurve _cAltitudeResponseEfficiency;
    public AnimationCurve _cTemperatureResponseEfficiency;
    public AnimationCurve _cDensityResponseEfficiency;
    public CurveContainer _cOverrideAltiCurve;
    public CurveContainer _cOverrideTempCurve;
    public CurveContainer _cOverrideDensityCurve;

    public int _cFallbackSeaLevelAirTemperature = 15;
    public int _cFallbackSeaLevelAirPressure = 1013;
    public int _cFallbackSeaLevelRelativeHumidity = 50;
    public int _cFallbackSeaLevelWindStrength = 5;
}
#endregion

#region Avionics
[Serializable]
public class ConfigAvionics
{
    public bool _cAnimateAvionics;
    public AvionicsInstrument[] _cAvionicsInstrument;
}
[Serializable]
public class AvionicsInstrument
{
    public enum displayMedium { Analog, Digital }
    public enum displayStyle { AsymmetricalRotary, SymmetricalRotary, SymmetricalLinear, SymmetricalDigits }
    public string InstrumentName;
    [LabelOverride("Instrument Type")]
    public displayMedium dispType;
    [LabelOverride("Instrument Layout")]
    public displayStyle dispStyle;
    public string ReferenceVariableName;

    [DrawIf("dispStyle", displayStyle.AsymmetricalRotary)]
    public float RotaryNeedleConstraint;
    [DrawIf("dispStyle", displayStyle.SymmetricalRotary)]
    public Vector2 RotaryNeeldeConstraints;
    [DrawIf("dispStyle", displayStyle.SymmetricalRotary)]
    public float RotaryNeedleMaxTurn;
    [DrawIf("dispStyle", displayStyle.SymmetricalLinear)]
    public Vector2 LinearNeedleConstraints;
    [DrawIf("dispStyle", displayStyle.SymmetricalDigits)]
    public Vector2 DigitConstraints; //todo

    public int AvionicsRotationSmoothing;
    public AvionicsInstrumentNeedle[] InstrumentNeedle;
    [HideInInspector] public float readValue;
}
[Serializable]
public class AvionicsInstrumentNeedle
{
    public enum needleValueMultiplier
    {
        [InspectorName("x1")] One,
        [InspectorName("x10")] Ten,
        [InspectorName("x100")] Hundred,
        [InspectorName("x1000")] Thousand,
        [InspectorName("x10,000")] Tenthousand,
        [InspectorName("x100,000")] Hundredthousand,
        [InspectorName("x1,000,000")] Million,
        [InspectorName("Custom")] Custom,
    }
    public enum needleAxisSingle { X, Y, Z }
    public Transform Needle;
    public needleValueMultiplier NeedleFactor;
    [DrawIf("NeedleFactor", needleValueMultiplier.Custom)]
    public float CustomNeedleFactor;
    [Space(10)]

    [LabelOverride("Axis")] public needleAxisSingle NeedleOneAxis;
    [LabelOverride("Amount On Axis")] public float NeedleOneAxisAmount;
    [HideInInspector] public float movingFactor;
    [HideInInspector] public float targetRotation;
}
#endregion

#region Control Surfaces & Gear
[Serializable]
public class ConfigControlSurfaces
{
    public bool _cAnimateControlSurfaces;
    public bool _cHasSurfaceElevator;
    public bool _cHasSurfaceRudder;
    public bool _cHasSurfaceAilerons;
    public bool _cHasSurfaceAirbrakes;
    public bool _cHasSurfaceFlaps;

    public float _cElevatorResponse;
    public float _cRudderResponse;
    public float _cAileronResponse;
    public float _cBankingTurnResponse;

    public float _cAirbrakeResponse;

    public float _cFlapResponse;
    public int[] _cFlapSteps;
    public AnimationCurve _cFlapLiftOverSpeed = AnimationCurve.Linear(0f, 1f, 1f, 0f);
    public AnimationCurve _cFlapDragOverSpeed = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public CurveContainer _cOverrideFlapLiftCurve;
    public CurveContainer _cOverrideFlapDragCurve;


    public float _cThrottleLeverResponse;
}
[Serializable]
public class ConfigControlSurfaceAnimations
{
    public ControlSurface[] _cControlSurfacesAnimations;
}
[Serializable]
public class ControlSurface
{
    public enum SurfaceAnimationType { Script, Animation }
    public enum RotationAxisType { XAxis, YAxis, ZAxis }

    public string AnimatedControlSurface;
    [Space(10)]
    [LabelOverride("Animate via")] public SurfaceAnimationType AnimationType;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public Transform RotationAnchor;
    [Space(10)]
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public RotationAxisType RotationAxis;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public string rotationValue;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public float RotationAmount;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public int RotationSmoothing;

    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string startInStartAnimationState;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string triggeringBool;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public Animator CSAnimator;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string StartAnimationStateBoolName;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string ToggleAnimationBoolName;
    public UnityEvent OnAnimationFinished = new UnityEvent();
    [HideInInspector] public Quaternion OriginalRotationAmount;
    [HideInInspector] public Quaternion RotationAxisQ;
    [HideInInspector] public AnimatorStateInfo CSAnimatorState;
    [HideInInspector] public float AnimationStateLength;
    [HideInInspector] public float PassedTime;
    [HideInInspector] public bool AnimationFinished;
    [HideInInspector] public bool AnimationStateA;
}
[Serializable]
public class ConfigLandingGear
{
    public enum _cLandingGearTypes { FixedGear, RetractableGear, }
    public _cLandingGearTypes _cLandingGearType;
    public float _cLandingGearLoweredDrag;
    public float _cLandingGearBrakeStrength;
    public GameObject _cLandingGearSteeringColumn;
    public float _cLandingGearMaxSteeringAngle;
    public bool _cAnimateLandingGear;
    public float _cWheelBrakeTempSec;
}
#endregion

#region Propulsion & Fuel
[Serializable]
public class ConfigEngines
{
    public enum _cEngineConfiguration { SingleEngine, MultiEngine, }
    public _cEngineConfiguration _cEngineConfig;
    public enum _cPropulsionType { Turbofan, Turbopropeller, Propeller, AfterburnerJet, }
    public _cPropulsionType _cPropulsion;

    public float _cMaxEngineThrust;
    public float _cMaxTogaThrust;
    public AnimationCurve _cEngineSpoolCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public CurveContainer _cOverrideSpoolCurve;

    public Transform _cSingleEnginePosition;
    public _configVarMultiEngineProperty[] _cMultiEngineProperties;
}
[Serializable]
public class _configVarMultiEngineProperty
{
    [LabelOverride("Engine No.")]
    public int _cEngineNumber;
    [LabelOverride("Engine Thrust Pos.")]
    public Transform _cEnginePosition;
    [HideInInspector] public bool _cEngineOnOverride;
    [HideInInspector] public bool _cEngineMalfunction;
    [HideInInspector] public float _cEngineCurrentThrust;
    [HideInInspector] public float _cEngineRequestedThrust;
}
[Serializable]
public class ConfigFuel
{
    public bool _cEnginesBurnFuel;
    public float _cMaximumFuelCapacity;
    public AnimationCurve _cFuelBurnAtEngineThrust = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public CurveContainer _cOverrideFuelBurnCurve;
}
#endregion

#region Systems
[Serializable]
public class ConfigConstraintsAutomatics
{
    public float _cAPVSpeedLimit;
    public Vector2 _cAPPitchAngleLimit;
    public Vector2 _cAPRollAngleLimit;
    public Vector2 _cATForwardSpeedLimit;
}

[Serializable]
public class ConfigRadioTransmitter
{
    public TransmitterData[] _cRadioTransmitters; //The amount of transmitters! NOT (!) channels. An Airliner usually has 2 or 3. (I think). 
}
[Serializable]
public class TransmitterData
{
    [LabelOverride("Default Transmitter Frequency")]
    public int _cDefaultFrequency;
}
#endregion