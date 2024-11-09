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
        if(!_RuntimeManager._AppIOManager.FileExists("Settings")) 
        {
            //If there is no settings file, let's make a new one and save it directly!
            Debug.Log("Could not find Settings File. Creating generic Settings file.");
            _AppSettings = new ApplicationSettings();
            _RuntimeManager._AppIOManager.ExportToFile(_AppSettings, "Settings");
        }
        else
        {
            Debug.Log("Found Settings File.");
            _AppSettings = _RuntimeManager._AppIOManager.ImportFromFile<ApplicationSettings>("Settings");
        }
    }
}
