using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftAerodynamicsProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;
        private Rigidbody _aircraftRigidbody;

        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            _aircraftRigidbody = GetComponent<Rigidbody>();

            _configBaseProcessor._DragStart = _aircraftRigidbody.drag;
            _configBaseProcessor._AngularDragStart = _aircraftRigidbody.angularDrag;
            _configBaseProcessor._LandingGearDragStart = _configBaseProcessor._LandingGearDrag;
            if(_configBaseProcessor._CustomCenterOfMass != null && _configBaseProcessor._CustomCOM)
            {
                Vector3 centerofmass = _configBaseProcessor._CustomCenterOfMass.localPosition;
                _aircraftRigidbody.centerOfMass = centerofmass;
            }
        }

        private void Update()
        {
            PerformAircraftAerodynamicCalculations();
        }

        public void PerformAircraftAerodynamicCalculations()
        {
            CalculateAircraftAerodynamicEffect();
            CalculateAircraftDrag();
            CalculateAircraftLinearForces();
            CalculateAircraftTorque();
        }
        private void CalculateAircraftAerodynamicEffect()
        {
            //"Aerodynamic" calculations. This is a very simple approximation of the effect that a plane will naturally try to align itself in the direction that it's facing when moving at speed. Without this, the plane would behave a bit like the asteroids spaceship!
            if (_aircraftRigidbody.velocity.magnitude > 0)
            {                
                _configBaseProcessor._FcgDirDifFactor = Vector3.Dot(transform.forward, _aircraftRigidbody.velocity.normalized); //Compare the direction we're pointing with the direction we're moving.
                _configBaseProcessor._FcgDirDifFactor *= _configBaseProcessor._FcgDirDifFactor; //Multipled by itself results in a desirable rolloff curve of the effect.
                //Calculate a new velocity by bending current velocity direction towards direction of plane facing, by amount based on aeroFactor
                var turnToVelocity = Vector3.Lerp(_aircraftRigidbody.velocity, transform.forward * _configBaseProcessor._ForwardSpeed, _configBaseProcessor._FcgDirDifFactor * _configBaseProcessor._ForwardSpeed * _configBaseProcessor._AerodynamicEffect * Time.deltaTime);
                _aircraftRigidbody.velocity = turnToVelocity;
                //ADD LATER: ATHMOSPHERIC EFFECT.
                //Also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up pointing downwards in a stall
                //The if condition prevents the aircraft to spazz arond like some retard when stationary.
                if (!_configBaseProcessor._StateGrounded) { _aircraftRigidbody.rotation = Quaternion.Slerp(_aircraftRigidbody.rotation, Quaternion.LookRotation(_aircraftRigidbody.velocity, transform.up), _configBaseProcessor._AerodynamicEffect * Time.deltaTime ); }
            }
        }
        private void CalculateAircraftDrag()
        {            
            float currentDrag = _configBaseProcessor._DragStart;
            float currentAngularDrag = _configBaseProcessor._AngularDragStart;
            float additionalSpeedDrag = _aircraftRigidbody.velocity.magnitude * _configBaseProcessor._DragOverSpeed; //Drag with the aircrafts velocity.
            float dragFactor = _configBaseProcessor._LiftSpeedFactorCurve.Evaluate(Mathf.InverseLerp(0, (_configBaseProcessor._MinMaxStableForwardSpeed.x + _configBaseProcessor._MinMaxStableForwardSpeed.x) / 2, _configBaseProcessor._ForwardSpeed));

            currentDrag += additionalSpeedDrag;
            currentDrag += (_configBaseProcessor._HasAirbrakes ? _configBaseProcessor._CurrentAirbrakeAmount * _configBaseProcessor._AirbrakeResponse * currentDrag : 0);
            currentDrag += (_configBaseProcessor._LandingGearInducesDrag ? _configBaseProcessor._LandingGearDrag * currentDrag : 0); //If aircraft landing gear induces drag, add landing gear drag.
            currentDrag += (_configBaseProcessor._HasFlaps ? _configBaseProcessor._FlapDrag : 0); //If aircraft has flaps, add current flap drag.
            currentDrag *= (_configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics ? _configBaseProcessor._DragPotential : 1); //If Atmosphere affects aerodynamics, multiply by potential.
            currentAngularDrag *= _configBaseProcessor._ForwardSpeed;
            currentAngularDrag += (_configBaseProcessor._HasAirbrakes ? _configBaseProcessor._CurrentAirbrakeAmount * _configBaseProcessor._AirbrakeResponse * currentAngularDrag: 0);
            currentAngularDrag += (_configBaseProcessor._LandingGearInducesDrag ? _configBaseProcessor._LandingGearDrag * currentAngularDrag : 0); //If aircraft landing gear induces drag, add landing gear drag.
            currentAngularDrag += (_configBaseProcessor._HasFlaps ? _configBaseProcessor._FlapDrag : 0); //If aircraft has flaps, add current flap drag.
            currentAngularDrag *= (_configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics ? _configBaseProcessor._DragPotential : 1); //If Atmosphere affects aerodynamics, multiply by potential.
            _aircraftRigidbody.drag = currentDrag * dragFactor;
            _aircraftRigidbody.angularDrag = currentAngularDrag;

            _configBaseProcessor._Drag = currentDrag;
            _configBaseProcessor._AngularDrag = currentAngularDrag;
        }
        private void CalculateAircraftLinearForces()
        {
            var lift = _configBaseProcessor._LiftSpeedFactorCurve.Evaluate(Mathf.InverseLerp(0, (_configBaseProcessor._MinMaxStableForwardSpeed.x + _configBaseProcessor._MinMaxStableForwardSpeed.x) / 2, _configBaseProcessor._ForwardSpeed));
            var appliedForces = Vector3.zero; //Accumulate forces into variable, add the engine power in the forward direction  
            var liftDirection = Vector3.Cross(_aircraftRigidbody.velocity, transform.right).normalized; //The direction that the lift force is applied is at right angles to the plane's velocity (usually, this is 'up'!)
            var liftOverSpeedPotential = Mathf.InverseLerp(_configBaseProcessor._MaximumLiftSpeed, 0, _configBaseProcessor._ForwardSpeed);
            var liftPower = (Mathf.Pow(_configBaseProcessor._ForwardSpeed, 2) * _configBaseProcessor._Lift * liftOverSpeedPotential * _configBaseProcessor._FcgDirDifFactor); //Calculate and add the lift power            
            var rollLiftFactor = Mathf.InverseLerp(2, 0.5f, Mathf.Abs(_configBaseProcessor._RollAngle));
            var windLift = _configBaseProcessor._WindLiftPotential;
            liftPower += (_configBaseProcessor._EnvironmentWindAffectsAerodynamics ? liftPower * windLift : 0);
            liftDirection.y *= rollLiftFactor;
            appliedForces += (_configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics ? (_configBaseProcessor._HasFlaps ?  (liftPower + (liftPower * _configBaseProcessor._FlapLift)) * liftDirection : liftPower * liftDirection) * _configBaseProcessor._LiftPotential : (_configBaseProcessor._HasFlaps ? (liftPower + _configBaseProcessor._FlapLift) * liftDirection : liftPower * liftDirection));
            appliedForces *= lift;
            _configBaseProcessor._LiftStrength = liftPower;
            _configBaseProcessor._LiftDirection = liftDirection;
            _configBaseProcessor._Forces = appliedForces;

            _aircraftRigidbody.AddForce(_configBaseProcessor._Forces);           
        }
        private void CalculateAircraftTorque()
        {
            //var rollTurn = (_configBaseProcessor._RollAngle == 0 || Mathf.Abs(_configBaseProcessor._RollAngle) > 2 ? 0 : Mathf.InverseLerp(0, 2, Mathf.Abs(_configBaseProcessor._RollAngle))); //Accumulate the amount of yaw the aircraft will do.   
            var rollTurn = Mathf.InverseLerp(0, 2, Mathf.Abs(_configBaseProcessor._RollAngle)); //Accumulate the amount of yaw the aircraft will do.   
            var inputTorque = Vector3.zero; //Accumulate torque from control surfaces into variable.
            var inputTorqueEffect = _configBaseProcessor._LiftSpeedFactorCurve.Evaluate(Mathf.InverseLerp(0, (_configBaseProcessor._MinMaxStableForwardSpeed.x + _configBaseProcessor._MinMaxStableForwardSpeed.x) / 2, _configBaseProcessor._ForwardSpeed));
            var torque = Vector3.zero; //Accumulate torque forces into variable.       

            inputTorque += _configBaseProcessor._CurrentSetElevatorAmount * _configBaseProcessor._ElevatorResponse * transform.right; //Add torque for the pitch based on the elevator setting. 
            inputTorque += _configBaseProcessor._CurrentSetAileronAmount * _configBaseProcessor._AileronResponse * transform.forward; //Add torque for the roll based on the aileron setting.
            inputTorque += _configBaseProcessor._CurrentSetRudderAmount * _configBaseProcessor._RudderResponse * transform.up; //Add torque for the yaw based on the rudder setting.
            inputTorque += Mathf.Sin(_configBaseProcessor._RollAngle) * _configBaseProcessor._BankingTurnResponse * transform.up; //Add torque for the banking turn response.            
            torque += inputTorque * inputTorqueEffect;
            torque += (_configBaseProcessor._RollAngle == 0 ? Vector3.zero : (transform.up * _configBaseProcessor._RollAngle / (rollTurn * _configBaseProcessor._RudderResponse * Mathf.InverseLerp(_configBaseProcessor._MaximumLiftSpeed, -0.001f, _configBaseProcessor._ForwardSpeed)) / 4)); //Turn the aircraft downwards when it is rolling. The divisor 4 stands for the rudder amount quadrupled. This is to counteract the possibility of being able to coutner steer this effect with the rudder
            torque *= (_configBaseProcessor._EnvironmentAtmosphereAffectsAerodynamics ? _configBaseProcessor._ForwardSpeed * _configBaseProcessor._FcgDirDifFactor * _configBaseProcessor._TorquePotential : _configBaseProcessor._ForwardSpeed * _configBaseProcessor._FcgDirDifFactor); //The total torque is multiplied by the forward speed, so the controls have more effect at high speed, and little effect at low speed, or when not moving in the direction of the nose of the plane (i.e. falling while stalled)
            torque += (_configBaseProcessor._EnvironmentWindAffectsAerodynamics ? transform.forward * _configBaseProcessor._AltitudeWindStrength * Mathf.RoundToInt(Random.Range(-1f, 1f)) : Vector3.zero); //If wind effects aerodynamics, turn the aircraft around the forward axis in a random direction.

            _configBaseProcessor._Torque = torque;
            _aircraftRigidbody.AddTorque(_configBaseProcessor._Torque);            
        }
    }
}
