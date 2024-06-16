using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftControlSurfacesProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;


        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
        }

        public void PerformControlSurfaceCalculations(float pitchInput, float rollInput, float yawInput, float airbrakeInput, bool flapInput)
        {
            CalculateElevatorState(pitchInput);
            CalculateAileronState(rollInput);
            CalculateRudderState(yawInput);
            CalculateAirbrakeState(airbrakeInput);
            CalculateFlapState(flapInput);
            
        }

        private void ExecuteTrimOrSomething()
        {
            //IMPLEMENT LATER
        }
        private void CalculateElevatorState(float pitchInput)
        {
            _configBaseProcessor._CurrentSetElevatorAmount = (_configBaseProcessor._HasElevator ? Mathf.Clamp(pitchInput, -1, 1) : 0);
        }
        private void CalculateAileronState(float rollInput)
        {
            _configBaseProcessor._CurrentSetAileronAmount = (_configBaseProcessor._HasAilerons ? Mathf.Clamp(-rollInput, -1, 1): 0);
        }
        private void CalculateRudderState(float yawInput)
        {
            _configBaseProcessor._CurrentSetRudderAmount = (_configBaseProcessor._HasRudder ? Mathf.Clamp(yawInput, -1, 1) : 0);
        }
        private void CalculateAirbrakeState(float airbrakeInput)
        {
            _configBaseProcessor._CurrentAirbrakeAmount = (_configBaseProcessor._HasAirbrakes ? Mathf.Clamp(airbrakeInput, 0, 1) : 0);
        }
        private void CalculateFlapState(bool flapInput)
        {
            if (_configBaseProcessor._HasFlaps)
            {
                if (flapInput)
                {
                    if (_configBaseProcessor._CurrentFlapSetting <= _configBaseProcessor._FlapSteps.Length)
                    {
                        _configBaseProcessor._CurrentFlapSetting += 1;
                    }
                    else
                    {
                        _configBaseProcessor._CurrentFlapSetting = 0;
                    }
                    _configBaseProcessor._CurrentFlapDegreeSetting = _configBaseProcessor._FlapSteps[_configBaseProcessor._CurrentFlapSetting];
                    CalculateFlapEffects();
                }               
            }
        }
        private void CalculateFlapEffects()
        {
            if (_configBaseProcessor._HasFlaps)
            {
                float currentSpeedPercentage = Mathf.Clamp01(_configBaseProcessor._ForwardSpeed / _configBaseProcessor._MaximumLiftSpeed);
                float currentFlapLift = _configBaseProcessor._FlapLiftOverSpeed.Evaluate(currentSpeedPercentage);
                float currentFlapDrag = _configBaseProcessor._FlapDragOverSpeed.Evaluate(currentSpeedPercentage);
                float currentRelativeFlapSetting = _configBaseProcessor._CurrentFlapDegreeSetting / 45;
                _configBaseProcessor._FlapLift = currentFlapLift * currentRelativeFlapSetting * _configBaseProcessor._FlapResponse;
                _configBaseProcessor._FlapDrag = currentFlapDrag * currentRelativeFlapSetting * _configBaseProcessor._FlapResponse;
            }
            else
            {
                _configBaseProcessor._FlapLift = 0;
                _configBaseProcessor._FlapDrag = 0;
            }
        }       
    }
}
