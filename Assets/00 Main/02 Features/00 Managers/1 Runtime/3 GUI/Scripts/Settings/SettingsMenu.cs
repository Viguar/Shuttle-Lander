using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private void OnEnable()
    {
        FindFirstObjectByType<AppSettingsManager>().InitSettingsComponents();
    }
}
