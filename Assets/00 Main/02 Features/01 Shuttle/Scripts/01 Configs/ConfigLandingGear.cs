using UnityEngine;
[System.Serializable]

public class ConfigLandingGear
{
    public enum _cLandingGearTypes { FixedGear, RetractableGear,}
    public _cLandingGearTypes _cLandingGearType;
    public float _cLandingGearLoweredDrag;
    public float _cLandingGearBrakeStrength;
    public GameObject _cLandingGearSteeringColumn;
    public float _cLandingGearMaxSteeringAngle;
    public bool _cAnimateLandingGear;
    public float _cWheelBrakeTempSec;
}
