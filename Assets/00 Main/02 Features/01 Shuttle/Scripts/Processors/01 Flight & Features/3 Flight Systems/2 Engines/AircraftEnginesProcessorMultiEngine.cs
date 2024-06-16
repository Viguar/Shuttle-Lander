using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftEnginesProcessorMultiEngine : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;

        private bool EngineRunning;
        private bool EngineMalfunction;
        private float TotalThrust;


        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();
            MasterSetMultiEngine(_configBaseProcessor._StartWithEngineRunning);
        }

        private void Update()
        {
            PerformMultiEngine();
        }

        public void PerformMultiEngine()
        {
            CalculateEngineValues();
            CheckForEngineStateChanges();
            ControlEngines();
        }

        public void MasterToggleMultiEngine()
        {
            if (!EngineMalfunction) { _configBaseProcessor._MasterEnginesOn = !_configBaseProcessor._MasterEnginesOn; EngineRunning = _configBaseProcessor._MasterEnginesOn; }
            else { _configBaseProcessor._MasterEnginesOn = false; EngineRunning = false; }
        }
        public void MasterSetMultiEngine(bool masterEngineState)
        {
            if (!EngineMalfunction) { _configBaseProcessor._MasterEnginesOn = masterEngineState; EngineRunning = _configBaseProcessor._MasterEnginesOn; }
            else { _configBaseProcessor._MasterEnginesOn = false; EngineRunning = false; }
        }
        public void MasterToggleMultiEngineToga()
        {
            if (EngineRunning && !EngineMalfunction) { _configBaseProcessor._TogaMode = !_configBaseProcessor._TogaMode; }
            else { _configBaseProcessor._TogaMode = false; }
        }
        public void MasterSetMultiEngineToga(bool togaMode)
        {
            if (EngineRunning && !EngineMalfunction) { _configBaseProcessor._TogaMode = togaMode; }
            else { _configBaseProcessor._TogaMode = false; }
        }
        public void SetMultiEngineMalfunction(bool hasMalfunction)
        {
            EngineMalfunction = hasMalfunction;
        }

        private void CalculateEngineValues()
        {
            _configBaseProcessor._TotalEngineCount = _configBaseProcessor._EngineProperties.Length;
            _configBaseProcessor._MaximumTotalThrust = _configBaseProcessor._MaxEngineThrust * _configBaseProcessor._TotalEngineCount;
            _configBaseProcessor._MaximumTotalTogaThrust = _configBaseProcessor._MaxTogaThrust * _configBaseProcessor._TotalEngineCount;
            _configBaseProcessor._CurrentTotalThrust = TotalThrust;
        }
        private void CheckForEngineStateChanges()
        {
            if (EngineMalfunction) { EngineRunning = false; }
        }
        private void ControlEngines()
        {         
            foreach(_configVarMultiEngineProperty engine in _configBaseProcessor._EngineProperties)
            {
                engine._cEngineRequestedThrust = (_configBaseProcessor._TogaMode ? _configBaseProcessor._MaxTogaThrust * _configBaseProcessor._LeverThrottleSetting : _configBaseProcessor._MaxEngineThrust * _configBaseProcessor._LeverThrottleSetting);
                if(EngineRunning)
                {
                    if(engine._cEngineCurrentThrust < engine._cEngineRequestedThrust)
                    {
                        float spoolRate = _configBaseProcessor._EngineSpoolRate.Evaluate(_configBaseProcessor._LeverThrottleSetting);
                        float engineSpool = spoolRate * 100;
                        engine._cEngineCurrentThrust += engineSpool * 0.1f;
                    }
                    if (engine._cEngineCurrentThrust > engine._cEngineRequestedThrust)
                    {
                        float spoolRate = _configBaseProcessor._EngineSpoolRate.Evaluate(1 - _configBaseProcessor._LeverThrottleSetting);
                        float engineSpool = spoolRate * 100;
                        engine._cEngineCurrentThrust -= engineSpool * 0.1f;
                    }
                    _aircraftRigidbody.AddForceAtPosition(transform.forward * engine._cEngineCurrentThrust, engine._cEnginePosition.transform.localPosition);
                    TotalThrust += engine._cEngineCurrentThrust;
                }
            }
            TotalThrust = 0;
        }
    }
}
