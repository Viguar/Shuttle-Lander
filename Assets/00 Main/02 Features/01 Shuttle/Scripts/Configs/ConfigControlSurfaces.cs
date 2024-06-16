using UnityEngine;
using System;
using Viguar.EditorTooling.DataContainers.Curve;
[System.Serializable]

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
