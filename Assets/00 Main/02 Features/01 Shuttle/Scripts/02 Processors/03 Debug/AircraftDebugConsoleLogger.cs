using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AircraftDebugConsoleLogger : MonoBehaviour
{
    public void UpdateConsole(string consoleText)
    {
        GetComponent<TMP_Text>().text = consoleText;
    }
}
