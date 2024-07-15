using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;

public class DebugCommander : MonoBehaviour
{
    public GameObject DebugPanel;
    public GameObject[] ToggleablePanels;
    public Color DefaultTextColor;
    public Color BoolTrueColor;
    public Color BoolFalseColor;
    [Space(10)]
    public Color StateStableColor;
    public Color StateCriticalColor;
    public Color StateUpsetColor;

    private ValueDisplayer[] displayers;
    private bool DebugUIActive = false;

    private void Start()
    {
        displayers = GetComponentsInChildren<ValueDisplayer>();
        DebugPanel.SetActive(DebugUIActive);
    }

    private void Update()
    {
        if (DebugUIActive)
        {
            foreach (ValueDisplayer displayer in displayers)
            {
                displayer.DisplayDebugValue();
            }
        }
        if(GetComponentInParent<AircraftBaseProcessor>()._DebugShutterInput) 
        {
            DebugUIActive = false;
            DebugPanel.SetActive(DebugUIActive);           
        }

    }

    public void ToggleDebugUI()
    {
        DebugUIActive = !DebugUIActive;
        DebugPanel.SetActive(DebugUIActive);
    }

    public void ToggleSubpanel(int panelIndex)
    {
        ToggleablePanels[panelIndex].SetActive(!ToggleablePanels[panelIndex].activeInHierarchy);
    }
}
