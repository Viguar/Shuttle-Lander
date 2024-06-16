using UnityEngine;
using System;
using Viguar.EditorTooling.GUITools.OverrideLabels;
using Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay;

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