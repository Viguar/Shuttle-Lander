using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeManager : MonoBehaviour
{
    public AppIOManager _AppIOManager;
    public AppSettingsManager _AppSettingsManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        FetchPersistentRuntimeComponents();
    }
    
    private void FetchPersistentRuntimeComponents()
    {
        _AppIOManager = GetComponentInChildren<AppIOManager>();
        _AppSettingsManager = GetComponentInChildren<AppSettingsManager>();

        _AppIOManager.InitComponent(this);
        _AppSettingsManager.InitComponent(this);



        _AppSettingsManager.LoadApplicationSettings();
    }
}
