using UnityEngine;
using System;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;
using Viguar.EditorTooling.DataContainers.Curve;

[System.Serializable]
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


