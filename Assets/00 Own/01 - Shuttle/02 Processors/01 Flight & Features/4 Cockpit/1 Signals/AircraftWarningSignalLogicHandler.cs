using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Viguar.Aircraft
{
    public class AircraftWarningSignalLogicHandler : MonoBehaviour
    {

        private AircraftBaseProcessor _configBaseProcessor;
        private bool CautionOn = false;
        private bool WarningOn = false;

        [SerializeField] private WarningSignalCategoryProperties[] MasterCautionSignalTriggers;
        [SerializeField] private WarningSignalCategoryProperties[] MasterWarningSignalTriggers;

        private void Start()
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
        }

        private void Update()
        {
            HandleMasterCautionSignal();
            HandleMasterWarningSignal();
            if(CautionOn) { _configBaseProcessor._CWPMasterCautionState = AircraftBaseProcessor.CWPCStateTypes.Warning; }
            else { _configBaseProcessor._CWPMasterCautionState = AircraftBaseProcessor.CWPCStateTypes.Clear; }
            if(WarningOn) { _configBaseProcessor._CWPMasterWarningState = AircraftBaseProcessor.CWPWStateTypes.Warning; }
            else { _configBaseProcessor._CWPMasterWarningState = AircraftBaseProcessor.CWPWStateTypes.Clear; }
        }

        private void HandleMasterCautionSignal()
        {
            int CautionCounter = 0;
            foreach(WarningSignalCategoryProperties cautionSignal in MasterCautionSignalTriggers)
            {
                _configBaseProcessor.DefineDebugDictionaryString();
                _configBaseProcessor.DebugStringDict.TryGetValue(cautionSignal.WarningSignalVariableName, out string VariableString);
                foreach(string trigger in cautionSignal.TriggeringConditions)
                {
                    if(trigger == VariableString) { CautionCounter += 1; }
                }
            }
            if (CautionCounter > 0) { CautionOn = true; }
            else { CautionOn = false; }
        }
        private void HandleMasterWarningSignal()
        {
            int WarningCounter = 0;
            foreach (WarningSignalCategoryProperties warningSignal in MasterWarningSignalTriggers)
            {
                _configBaseProcessor.DefineDebugDictionaryString();
                _configBaseProcessor.DebugStringDict.TryGetValue(warningSignal.WarningSignalVariableName, out string VariableString);
                foreach (string trigger in warningSignal.TriggeringConditions)
                {
                    if (trigger == VariableString) { WarningCounter += 1; }
                }
            }
            if (WarningCounter > 0) { WarningOn = true; }
            else { WarningOn = false; }
        }

    }

    [Serializable]
    public class WarningSignalCategoryProperties
    {
        public string WarningSignalVariableName;
        public string[] TriggeringConditions;
    }
}
