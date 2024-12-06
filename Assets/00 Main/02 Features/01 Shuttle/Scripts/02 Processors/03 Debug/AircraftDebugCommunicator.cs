using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft.Management;

namespace Viguar.Aircraft
{
    public class AircraftDebugCommunicator : MonoBehaviour
    {
        //Base Components
        private AircraftBaseProcessor _configBaseProcessor;

        //Debug UI Toggling
        private bool foundDebugUICanvas;
        private GameObject[] _debugUIPanels;

        //Debug UI Text & Value Handling
        public Color _defaultColor;
        public Color _positiveColor;
        public Color _negativeColor;
        private ValueDisplayer[] _displayers;
        private float SlowDisplayTicker = 0f;
        private float DisplayRefreshRatePerSecond = 5;

        //Console Handling
        private bool foundConsole;
        private int _currentLogMessageCount;
        private int _currentLogMessageCountTotal;
        private int _maxLogMessages = 200;
        private string _currentConsoleString;
        private AircraftDebugConsoleLogger _loggerComponent;

        //Mesh Handling
        private DebugMeshManager[] debugMeshes;

        //Methods
        private void OnEnable()
        {
            UnityEngine.Application.logMessageReceived += LogCallback;
        }
        private void OnDisable()
        {
            UnityEngine.Application.logMessageReceived -= LogCallback;
        }
        private void Start()
        {
            _configBaseProcessor = FindAnyObjectByType<AircraftBaseProcessor>();
            
            InitDebugValueDisplays();
            InitLogger();
            InitDebugPanels();
            InitDebugMeshRendering();
        }
        private void Update()
        {
            UpdateDebugValueDisplays();
        }

        //Debug Window ON/OFF Pass to Shuttle
        private void OnDebugShutter()
        {
            if (_configBaseProcessor._DebugShutterInput) { CloseDebugWindow(); }
        }
        public void OpenDebugWindow()
        {
            _configBaseProcessor._DebugPanelActive = true;
        }
        public void CloseDebugWindow()
        {
            _configBaseProcessor._DebugPanelActive = false;
        }

        //Debug UI Toggling
        private void InitDebugPanels()
        {
            _debugUIPanels = GameObject.FindGameObjectsWithTag("debugPanel");
            CloseAllDebugPanels();
        }
        public void OpenAllDebugPanels()
        {
            foreach (GameObject Panel in _debugUIPanels) { Panel.SetActive(true); }
        }
        public void CloseAllDebugPanels()
        {
            foreach (GameObject Panel in _debugUIPanels) { Panel.SetActive(false); }
        }

        //Debug UI Value Handling
        private void InitDebugValueDisplays()
        {
            _displayers = GameObject.FindObjectsByType<ValueDisplayer>(FindObjectsSortMode.None);
            foreach (ValueDisplayer _displayer in _displayers)
            {
                _displayer.InitDisplayer(_defaultColor, _positiveColor, _negativeColor);
            }
        }
        private void UpdateDebugValueDisplays()
        {
            SlowDisplayTicker += Time.deltaTime;
            if (SlowDisplayTicker >= 1 / DisplayRefreshRatePerSecond)
            {
                foreach (ValueDisplayer _displayer in _displayers)
                {
                    switch(_displayer.displayValue)
                    {
                        case ValueDisplayer.valueType.Float:
                            _configBaseProcessor.DefineDebugDictionaryFloat();
                            _configBaseProcessor.DebugFloatDict.TryGetValue(_displayer.variableName, out float VariableFloat);
                            _displayer.SetText(VariableFloat.ToString());
                            break;
                        case ValueDisplayer.valueType.Bool:
                            _configBaseProcessor.DefineDebugDictionaryBool();
                            _configBaseProcessor.DebugBoolDict.TryGetValue(_displayer.variableName, out bool VariableBool);
                            _displayer.SetText(VariableBool.ToString());
                            break;
                        case ValueDisplayer.valueType.Vector:
                            _configBaseProcessor.DefineDebugDictionaryVector();
                            _configBaseProcessor.DebugVectorDict.TryGetValue(_displayer.variableName, out Vector3 VariableVector);
                            _displayer.SetText(VariableVector.ToString());
                            break;
                        case ValueDisplayer.valueType.String:
                            _configBaseProcessor.DefineDebugDictionaryString();
                            _configBaseProcessor.DebugStringDict.TryGetValue(_displayer.variableName, out string VariableString);
                            _displayer.SetText(VariableString);
                            break;
                    }                   
                }
                SlowDisplayTicker = 0f;
            }
        }        

        //Debug UI Console Handling
        private void InitLogger()
        {
            if (GameObject.FindAnyObjectByType<AircraftDebugConsoleLogger>().GetComponent<AircraftDebugConsoleLogger>() != null) { _loggerComponent = GameObject.FindAnyObjectByType<AircraftDebugConsoleLogger>().GetComponent<AircraftDebugConsoleLogger>(); }
        }
        private void LogCallback(string logString, string stackTrace, LogType type)
        {
            _currentLogMessageCountTotal++;
            if (_currentLogMessageCount < _maxLogMessages)
            {
                _currentLogMessageCount++;
                _currentConsoleString += "[" + _currentLogMessageCountTotal + "] " + logString + "\r\n";
            }
            else
            {
                _currentLogMessageCount = 0;
                _currentConsoleString = "[" + _currentLogMessageCountTotal + "] " + logString + "\r\n";
            }
            _loggerComponent.UpdateConsole(_currentConsoleString);
        }

        //Finding & Executing GlobalDebugManager
        private void InitDebugMeshRendering()
        {
            debugMeshes = FindObjectsByType<DebugMeshManager>(FindObjectsSortMode.None);
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.InitDebugMeshManager();
            }
        }
        public void ShowDebugMeshes()
        {
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.ForceDebugMeshRendererState(true);
            }
        }        
        public void HideDebugMeshes()
        {
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.ForceDebugMeshRendererState(false);
            }
        }
        public void ToggleDebugMeshes()
        {
            foreach (DebugMeshManager debugMesh in debugMeshes)
            {
                debugMesh.ToggleDebugMeshRenderer();
            }
        }
    }
}






