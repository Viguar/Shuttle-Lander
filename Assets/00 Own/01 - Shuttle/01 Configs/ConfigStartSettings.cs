using UnityEngine;
using Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay;

[System.Serializable]
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
