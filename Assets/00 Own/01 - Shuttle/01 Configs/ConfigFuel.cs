using UnityEngine;
using System;
[System.Serializable]

public class ConfigFuel
{
    public bool _cEnginesBurnFuel;
    public float _cMaximumFuelCapacity;
    public AnimationCurve _cFuelBurnAtEngineThrust = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public Viguar.Tooling.CurveContainer _cOverrideFuelBurnCurve;
}
