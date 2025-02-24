using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;

public class SceneConfigManager : MonoBehaviour
{
    private RuntimeManager _RuntimeManager;
    public LoadingConfiguration _LoadingConfiguration;

    [Header("Scene Configuration")]
    public bool bladibla;
    
    [Header("Initialized Components")]
    public AircraftConfiguration _AircraftConfiguration;
    public AircraftBaseProcessor _AircraftBaseProcessor;

    [Space(10), Header("Fallback Prefabs")]
    public GameObject _SceneConfig_ShuttlePrefab;
    public GameObject _SceneConfig_WeatherSystemPrefab;


    public void InitComponent(RuntimeManager runtimeManager)
    {
        _RuntimeManager = runtimeManager;
    }

    public void LoadSceneConfiguration()
    {
        if (!_RuntimeManager._AppIOManager.FileExists("Loading Configuration"))
        {
            _LoadingConfiguration = new LoadingConfiguration();
            _RuntimeManager._AppIOManager.ExportToFile(_LoadingConfiguration, "Loading Configuration");
        }
        else 
        { 
            _LoadingConfiguration = _RuntimeManager._AppIOManager.ImportFromFile<LoadingConfiguration>("Loading Configuration");
        }     
    }
}
