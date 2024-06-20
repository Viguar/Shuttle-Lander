using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftFuelProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;

        void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            InitFuel();
        }
        private void Update()
        {
            PerformFuelCalculations();
        }

        public void PerformFuelCalculations()
        {
            BurnFuel();
            CheckFuelTankCapacity();
            OnTankEmpty();
        }

        public void InitFuel()
        {
            _configBaseProcessor._CurrentFuelAmount = 0;
        }
        public void ModifyFuelAmount(float amount)
        {
            _configBaseProcessor._CurrentFuelAmount += amount;
            Mathf.Clamp(_configBaseProcessor._CurrentFuelAmount, 0, _configBaseProcessor._MaximumFuelCapacity);
            _configBaseProcessor._CurrentFuelAmountPercentage = _configBaseProcessor._CurrentFuelAmount / _configBaseProcessor._MaximumFuelCapacity ;
        }

        private void BurnFuel()
        {
            if (_configBaseProcessor._MasterEnginesOn)
            {
                if (_configBaseProcessor._LeverThrottleSetting < 0.02f) { BurnFuelEngineIdle(); }
                else { BurnFuelEngineThrust(); }
            }
        }
        private void BurnFuelEngineIdle()
        {
            float currentFuelConsumption = _configBaseProcessor._FuelConsumptionAtThrust.Evaluate(0.02f);
            currentFuelConsumption *= Time.deltaTime;
            ModifyFuelAmount(-currentFuelConsumption);
        }
        private void BurnFuelEngineThrust()
        {
            float currentFuelConsumption = _configBaseProcessor._FuelConsumptionAtThrust.Evaluate(_configBaseProcessor._LeverThrottleSetting);
            currentFuelConsumption *= (_configBaseProcessor._TogaMode ? 1.5f * Time.deltaTime : Time.deltaTime);
            ModifyFuelAmount(-currentFuelConsumption);
        }
        private void CheckFuelTankCapacity()
        {
            if(_configBaseProcessor._CurrentFuelAmount == _configBaseProcessor._MaximumFuelCapacity) { _configBaseProcessor._FuelTankFull = true; }
            else { _configBaseProcessor._FuelTankFull = false; }                           
            if(_configBaseProcessor._CurrentFuelAmount == 0) { _configBaseProcessor._FuelTankDepleted = true; }
            else { _configBaseProcessor._FuelTankDepleted = false; }
        }
        private void OnTankEmpty()
        {
            if(_configBaseProcessor._FuelTankDepleted) { _configBaseProcessor._MasterEnginesOn = false; }
        }
    }
}
