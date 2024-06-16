using UnityEngine;
using System;
[System.Serializable]

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
