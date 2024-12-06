using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is the one that assigns and holds the game settings.
public class AppSettingsManager : MonoBehaviour
{
    private RuntimeManager _RuntimeManager;
    public ApplicationSettings _AppSettings;


    public void InitComponent(RuntimeManager runtimeManager)
    {
        _RuntimeManager = runtimeManager;
    }
    public void LoadApplicationSettings()
    {
        if (!_RuntimeManager._AppIOManager.FileExists("Application Settings"))
        {
            //If there is no settings file, let's make a new one and save it directly!
            Debug.Log("Could not find Settings File. Creating generic Settings file.");
            _AppSettings = new ApplicationSettings();
            SaveApplicationSettings();
        }
        else
        {
            Debug.Log("Found Settings File.");
            _AppSettings = _RuntimeManager._AppIOManager.ImportFromFile<ApplicationSettings>("Application Settings");
        }
    }
    public void SaveApplicationSettings()
    {
        _RuntimeManager._AppIOManager.ExportToFile(_AppSettings, "Application Settings");
    }
    public void InitSettingsComponents()
    {
        foreach (GUISettingsComponent settingsElement in FindObjectsByType<GUISettingsComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None)) 
        {
            settingsElement.InitComponent(this);        
        }
    }
    public void RefreshSettingsFeatures()
    {
        if (GetComponentInChildren<FPSCapping_SettingsFeature>() != null) { GetComponentInChildren<FPSCapping_SettingsFeature>().OnSettingsRefresh(_AppSettings); }
    }
}
