using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Inspector.PropertyFields;
public class RuntimeManager : MonoBehaviour
{
    //Info:
    //The runtime manager is always active in the game: It does not get destroyed during scene switches.
    //The runtime manager (and its children) handle main runtime of the game. It is not a game manager, however,
    //it commands game managers in certain cases to execute, pause or other.
    //

    [ReadOnly]public AppIOManager _AppIOManager;
    [ReadOnly]public AppSettingsManager _AppSettingsManager;

    private bool foundAppIOManager;
    private bool foundAppSettingsManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        FetchPersistentRuntimeComponents();
        InitPersistenRuntimeComponents();
    }
    
    private void FetchPersistentRuntimeComponents()
    {
        _AppIOManager = GetComponentInChildren<AppIOManager>();
        _AppSettingsManager = GetComponentInChildren<AppSettingsManager>();
        //Add more if needed later.

        //Checklist
        foundAppIOManager = _AppIOManager != null;
        foundAppSettingsManager = _AppSettingsManager != null;

        //Debug Logging
        Debug.Log(foundAppIOManager == true ? "Found App IO Manager." : "Failed to find App IO Manager!");
        Debug.Log(foundAppSettingsManager == true ? "Found App Settings Manager." : "Failed to find App Settings Manager!");
    }

    private void InitPersistenRuntimeComponents()
    {
        _AppIOManager.InitComponent(this);
        _AppSettingsManager.InitComponent(this);
        //Add more if needed later.


        _AppSettingsManager.LoadApplicationSettings();
    }
}
