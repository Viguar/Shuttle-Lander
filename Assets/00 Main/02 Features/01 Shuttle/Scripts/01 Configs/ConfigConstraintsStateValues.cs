using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;

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
    public enum _cAttType { StablePitchAngleRegime, StableBankAngleRegime, StablePitchRate, StableRollRate}
    [LabelOverride("Attitude Category")]
    public _cAttType _cAttitudeCategoryType;
    [LabelOverride("Attitude Category Regime")]
    public Vector2 _cAttitudeCategoryMinMaxRange;
}

[Serializable]
public class ConfigConstraintsSpeedStabilityCategories
{
    public enum _cSpdType { StableFlightSpeedRegime, StableVerticalSpeedRegime, UpsetStallSpeedRegime, UpsetOverspeedRegime}
    [LabelOverride("Speed Category")]
    public _cSpdType _cSpeedCategoryType;
    [LabelOverride("Speed Category Regime")]
    public Vector2 _cSpeedCategoryMinMaxRange;
}


