using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AircraftDebugConsoleLogger : MonoBehaviour
{
    private int CurrentLogMessageCount;

    private void OnEnable()
    {
        UnityEngine.Application.logMessageReceived += LogCallback;
    }

    private void LogCallback(string logString, string stackTrace, LogType type)
    {
        CurrentLogMessageCount++;
        GetComponent<TMP_Text>().text += "[" + CurrentLogMessageCount + "] " + logString + "\r\n";
    }
}
