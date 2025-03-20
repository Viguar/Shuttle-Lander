using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Viguar.Inspector.PropertyFields;

[Serializable]
public class SliderElementData //Data taken by the SettingsComponent to fill the blanks.
{
    public enum SliderTypes { IntSlider, FloatSlider, }    

    [Header("Slider Configuration")]
    public string SettingDisplayName;

    [Space(5)]
    [Header("Appearance")]
    public bool HasValueInputDisplayBox;
    [DrawIf("HasValueInputDisplayBox", true)] public bool IsEditableInputDisplay;
    [DrawIf("HasValueInputDisplayBox", true)] public bool HasUnitDisplayTextBox;
    [DrawIf("HasUnitDisplayTextBox", true)] public string DisplayedUnitText;

    [Space(10)]
    [Header("Variable Configuration")]
    public string ConnectedVariable;
    public SliderTypes SliderType;
    [Space(5)]
        //On IntSlider
    [DrawIf("SliderType", SliderTypes.IntSlider)] public int IntSliderCapMin;
    [DrawIf("SliderType", SliderTypes.IntSlider)] public int IntSliderCapMax;
        //On FloatSlider
    [DrawIf("SliderType", SliderTypes.FloatSlider)] public float FloatSliderCapMin;
    [DrawIf("SliderType", SliderTypes.FloatSlider)] public float FloatSliderCapMax;

    [Space(20)]
    public DrawOnSliderCaseData[] ConditionalElements;
}

[Serializable]
public class DrawOnSliderCaseData
{
    public enum DrawCases { IsAbove, IsBelow, AtMinimum, AtMaximum, }

    public GameObject Element;
    public DrawCases DrawOnCase;
    [DrawIf("DrawOnCase", DrawCases.IsAbove)] public float IsAboveThresholdValue;
    [DrawIf("DrawOnCase", DrawCases.IsBelow)] public float IsBelowThresholdValue;
}