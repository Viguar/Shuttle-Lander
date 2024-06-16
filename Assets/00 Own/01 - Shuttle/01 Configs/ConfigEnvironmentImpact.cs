using UnityEngine;
using Viguar.EditorTooling.DataContainers.Curve;
[System.Serializable]

public class ConfigEnvironmentImpact
{
    public bool _cAtmosphericEffect;
    public bool _cWindEffect;
    public bool _cPrecipitationEffect;

    public float _cTemperatureAltitudeFalloff;
    public float _cHumidityAltitudeFalloff;
    public float _cWindAltitudeIncrease;
    public float _cWindStrengthMultiplier;

    public AnimationCurve _cAltitudeResponseEfficiency;
    public AnimationCurve _cTemperatureResponseEfficiency;
    public AnimationCurve _cDensityResponseEfficiency;
    public CurveContainer _cOverrideAltiCurve;
    public CurveContainer _cOverrideTempCurve;
    public CurveContainer _cOverrideDensityCurve;

    public int _cFallbackSeaLevelAirTemperature = 15;
    public int _cFallbackSeaLevelAirPressure = 1013;
    public int _cFallbackSeaLevelRelativeHumidity = 50;
    public int _cFallbackSeaLevelWindStrength = 5;
}
