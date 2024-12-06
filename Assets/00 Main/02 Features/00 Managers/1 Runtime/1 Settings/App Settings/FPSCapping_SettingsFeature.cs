using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FPSCapping_SettingsFeature : MonoBehaviour
{
    private bool _FPSCapEnabled = false;
    private int _FPSCap = 30;

    public void OnSettingsRefresh(ApplicationSettings settings)
    {
        _FPSCapEnabled = settings.video_capframerate;
        _FPSCap = settings.video_maxframerate;
    }

    private void Update()
    {
        if(_FPSCapEnabled ) 
        {
            if (Application.targetFrameRate != _FPSCap) { Application.targetFrameRate = _FPSCap; }
        }
    }
}
