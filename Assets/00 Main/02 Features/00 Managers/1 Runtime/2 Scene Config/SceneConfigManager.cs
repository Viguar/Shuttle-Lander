using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;

public class SceneConfigManager : MonoBehaviour
{
    [Header("Scene Configuration")]
    public bool bladibla;
    
    [Header("Initialized Components")]
    public AircraftConfiguration _AircraftConfiguration;
    public AircraftBaseProcessor _AircraftBaseProcessor;

    [Space(10), Header("Fallback Prefabs")]
    public GameObject _SceneConfig_ShuttlePrefab;
    public GameObject _SceneConfig_WeatherSystemPrefab;


}
