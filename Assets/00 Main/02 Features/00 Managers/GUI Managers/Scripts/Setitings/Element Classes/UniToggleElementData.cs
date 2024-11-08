using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Viguar.Inspector.PropertyFields;

[Serializable]
public class UniToggleElementData //Data taken by the SettingsComponent to fill the blanks.
{
    [Header("UniToggle/Bool Configuration")]
    public string SettingDisplayName;
    public string ConnectedVariable;
    public bool DefaultValue;

    //[Space(10)]
    //public bool HasConditionalElements;

    [Space(20)]
    //[DrawIf("UniToggleConfiguration.HasConditionalElements", true)]
    public DrawOnUniToggleData[] ConditionalElements;
}

[Serializable]
public class DrawOnUniToggleData
{
    public enum DrawCases { IsOn, IsOff, }

    public GameObject Element;
    public DrawCases DrawOnCase;
}