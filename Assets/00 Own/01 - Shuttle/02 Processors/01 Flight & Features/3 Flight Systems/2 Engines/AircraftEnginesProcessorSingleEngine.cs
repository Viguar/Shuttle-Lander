using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftEnginesProcessorSingleEngine : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;

        private bool EngineRunning;
        private bool EngineMalfunction;

        private float MaximumThrust;
        private float MaximumTogaThrust;
        private float CurrentThrust;
        private float RequestedThrust;
        private float RequestDifference;

        private float CurrentRpm;

        private Transform EnginePosition;

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();

            EnginePosition = _configBaseProcessor._SingleEnginePosition;
            MaximumThrust = _configBaseProcessor._MaxEngineThrust;
            MaximumTogaThrust = _configBaseProcessor._MaxTogaThrust;
            MasterSetSingleEngine(_configBaseProcessor._StartWithEngineRunning);
        }

        private void Update()
        {
            PerformSingleEngine();
        }

        public void PerformSingleEngine()
        {
            CheckForEngineStateChanges();
            ControlEngineThrustSetting();
            ControlEngineSpool();
            ControlThrustForce();
            CalculateEngineValues();
        }

        public void MasterToggleSingleEngine()
        {
            if (!EngineMalfunction) { _configBaseProcessor._MasterEnginesOn = !_configBaseProcessor._MasterEnginesOn; EngineRunning = _configBaseProcessor._MasterEnginesOn; }
            else { _configBaseProcessor._MasterEnginesOn = false; EngineRunning = false; }
        }
        public void MasterSetSingleEngine(bool masterEngineState)
        {
            if (!EngineMalfunction) { _configBaseProcessor._MasterEnginesOn = masterEngineState; EngineRunning = _configBaseProcessor._MasterEnginesOn; }
            else { _configBaseProcessor._MasterEnginesOn = false; EngineRunning = false; }
        }      
        public void MasterToggleSingleEngineToga()
        {
            if(EngineRunning && !EngineMalfunction) { _configBaseProcessor._TogaMode = !_configBaseProcessor._TogaMode; }   
            else { _configBaseProcessor._TogaMode = false; }
        }
        public void MasterSetSingleEngineToga(bool togaMode)
        {
            if (EngineRunning && !EngineMalfunction) { _configBaseProcessor._TogaMode = togaMode; }
            else { _configBaseProcessor._TogaMode = false; }
        }
        public void SetSingleEngineMalfunction(bool hasMalfunction)
        {
            EngineMalfunction = hasMalfunction;            
        }        

        private void CalculateEngineValues()
        {
            _configBaseProcessor._TotalEngineCount = 1;
            _configBaseProcessor._MaximumTotalThrust = _configBaseProcessor._MaxEngineThrust * _configBaseProcessor._TotalEngineCount;
            _configBaseProcessor._MaximumTotalTogaThrust = _configBaseProcessor._MaxTogaThrust * _configBaseProcessor._TotalEngineCount; 
            _configBaseProcessor._RequestedTotalThrust = RequestedThrust * _configBaseProcessor._TotalEngineCount;
            _configBaseProcessor._CurrentTotalThrust = CurrentThrust * _configBaseProcessor._TotalEngineCount;
        }
        private void CheckForEngineStateChanges()
        {
            if(EngineMalfunction) { EngineRunning = false; }
            if(_configBaseProcessor._SwitchTogaSetting) { MasterToggleSingleEngineToga(); }
        }
        private void ControlEngineThrustSetting()
        {           
            if(RequestedThrust < 0)
            {
                RequestedThrust = 0;
            }
            else
            {
                if (_configBaseProcessor._DetectedThrottleInput)
                {
                    RequestedThrust = (_configBaseProcessor._TogaMode ? MaximumTogaThrust * _configBaseProcessor._LeverThrottleSetting : MaximumThrust * _configBaseProcessor._LeverThrottleSetting);
                    RequestDifference = Mathf.Abs(RequestedThrust - CurrentThrust);
                }
             }
        }
        private void ControlEngineSpool()
        {
            if (EngineRunning)
            {
                if (!_configBaseProcessor._DetectedThrottleInput)
                {
                    if (CurrentThrust > RequestedThrust + 1)
                    {
                        float spoolRate = Mathf.Clamp01(_configBaseProcessor._EngineSpoolRate.Evaluate(CurrentThrust / MaximumThrust));
                        float engineSpool = (spoolRate == 0 ? 1f : spoolRate * RequestDifference / 2);
                        if (CurrentThrust - engineSpool < RequestedThrust)
                        {
                            CurrentThrust = RequestedThrust;
                        }
                        else
                        {
                            CurrentThrust -= engineSpool;
                        }
                    }
                    else if (CurrentThrust < RequestedThrust - 1)
                    {
                        float spoolRate = Mathf.Clamp01(_configBaseProcessor._EngineSpoolRate.Evaluate(CurrentThrust / MaximumThrust));
                        float engineSpool = (spoolRate == 0 ? 1f : spoolRate * RequestDifference / 2);
                        if (CurrentThrust + engineSpool > RequestedThrust)
                        {
                            CurrentThrust = RequestedThrust;
                        }
                        else
                        {
                            CurrentThrust += engineSpool;
                        }
                    }
                }                
            }
        }
        private void ControlThrustForce()
        {
            if (EngineRunning)
            {
                _aircraftRigidbody.AddForceAtPosition(transform.forward * CurrentThrust, EnginePosition.position);
            }
        }
    }
}
