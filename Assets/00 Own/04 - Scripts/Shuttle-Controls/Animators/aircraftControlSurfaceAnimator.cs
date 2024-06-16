using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace Viguar.Aircraft
{
    public class aircraftControlSurfaceAnimator : MonoBehaviour
    {
        [SerializeField] private ControlSurface[] m_ControlSurfaces; // Collection of control surfaces.       
        [SerializeField] private WheelColliderMover[] m_wheelColliderMover;

        [SerializeField] private Animator gearAnimator;
        private AnimatorStateInfo gearAnimationState;
        private float gearAnimationStateLength;
        private float passedGearAnimationTime = 0f;
        private bool gearExtended;
        private bool gearStartExtended;
        private bool gearAnimationHasFinished;

        private aircraftController m_Plane; // Reference to the aeroplane controller.

        private void Start()
        {
            // Get the reference to the aeroplane controller.
            m_Plane = GetComponent<aircraftController>();
            // Store the original local rotation of each surface, so we can rotate relative to this
            foreach (var surface in m_ControlSurfaces)
            {
                surface.originalLocalRotation = surface.transform.localRotation;
            }
            initLandingGearState();
        }

        private void Update()
        {
            foreach (var surface in m_ControlSurfaces)
            {
                switch (surface.type)
                {
                    case ControlSurface.Type.Aileron:
                        {
                            // Ailerons rotate around the x axis, according to the plane's roll input
                            Quaternion rotation = Quaternion.Euler(surface.amount * m_Plane.RollInput, 0f, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.Elevator:
                        {
                            // Elevators rotate negatively around the x axis, according to the plane's pitch input
                            Quaternion rotation = Quaternion.Euler(surface.amount * -m_Plane.PitchInput, 0f, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.Rudder:
                        {
                            // Rudders rotate around their y axis, according to the plane's yaw input
                            Quaternion rotation = Quaternion.Euler(0f, surface.amount * m_Plane.YawInput, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.RuddervatorPositive:
                        {
                            // Ruddervators are a combination of rudder and elevator, and rotate
                            // around their z axis by a combination of the yaw and pitch input
                            float r = m_Plane.YawInput + m_Plane.PitchInput;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, surface.amount * r);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.RuddervatorNegative:
                        {
                            // ... and because ruddervators are "special", we need a negative version too. >_<
                            float r = m_Plane.YawInput - m_Plane.PitchInput;
                            Quaternion rotation = Quaternion.Euler(0f, 0f, surface.amount * r);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.Airbrake:
                        {
                            int airbrakesTrue = 0;
                            if (m_Plane.AirBrakes)
                            {
                                airbrakesTrue = 1;
                            }
                            else
                            {
                                airbrakesTrue = 0;
                            }
                            Quaternion rotation = Quaternion.Euler(surface.amount * airbrakesTrue * 100, 0f, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.Flap:
                        {
                            Quaternion rotation = Quaternion.Euler(surface.amount * m_Plane.Flaps, 0f, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                    case ControlSurface.Type.SteeringWheel:
                        {
                            Quaternion rotation = Quaternion.Euler(0f, surface.amount * m_Plane.YawInput, 0f);
                            RotateSurface(surface, rotation);
                            break;
                        }
                }
            }

            toggleLandingGear(m_Plane.LandingGearInput);
            doWheelColliderBehavior();
        }

        #region Control Surface Functions        
        private void RotateSurface(ControlSurface surface, Quaternion rotation)
        {
            // Create a target which is the surface's original rotation, rotated by the input.
            Quaternion target = surface.originalLocalRotation * rotation;

            // Slerp the surface's rotation towards the target rotation.
            surface.transform.localRotation = Quaternion.Slerp(surface.transform.localRotation, target,
                                                               surface.movementSmoothing * Time.deltaTime);
        }
        #endregion

        #region Animation Player Functions

        void initLandingGearState()
        {
            gearStartExtended = m_Plane.m_landingGearAtStart;
            
            if (gearStartExtended)
            {
                gearAnimator.SetBool("toGearRetracted", false);
                gearAnimator.SetBool("startGearExtended", true);
                gearAnimationState = gearAnimator.GetNextAnimatorStateInfo(0);
                gearExtended = true;
            }
            else
            {
                gearAnimator.SetBool("toGearRetracted", true);
                gearAnimator.SetBool("startGearExtended", false);
                gearAnimationState = gearAnimator.GetNextAnimatorStateInfo(0);
                gearExtended = false;
            }
            gearAnimationHasFinished = true;
            m_Plane.setLandingGear(gearStartExtended);
        }

        void toggleLandingGear(bool gearInputDetected)
        {
            gearAnimationState = gearAnimator.GetNextAnimatorStateInfo(0);
            if (gearInputDetected && gearAnimationHasFinished)
            {
                if (gearExtended)
                {
                    gearAnimator.SetBool("toGearRetracted", true);                   
                    passedGearAnimationTime = 0;
                    gearAnimationHasFinished = false;
                    gearAnimationState = gearAnimator.GetCurrentAnimatorStateInfo(0);
                    gearAnimationStateLength = gearAnimationState.length;
                    gearExtended = false;
                }
                else
                {
                    gearAnimator.SetBool("toGearRetracted", false);
                    passedGearAnimationTime = 0;
                    gearAnimationHasFinished = false;
                    gearAnimationState = gearAnimator.GetCurrentAnimatorStateInfo(0);
                    gearAnimationStateLength = gearAnimationState.length;
                    gearExtended = true;
                }
            }
            else
            {
                checkForGearAnimationFinished();
            }
        }

        void checkForGearAnimationFinished()
        {
            if (!gearAnimationHasFinished)
            {
                if (passedGearAnimationTime < gearAnimationStateLength)
                {
                    passedGearAnimationTime += Time.deltaTime;
                }
                else
                {
                    gearAnimationHasFinished = true;
                    m_Plane.toggleLandingGear();
                }
            }
        }

        void playAnimation(Animator animationController, string animationName)
        {
            //animationController.CrossFade(animationName, 0.25f, 0);
        }




        #endregion

        #region Wheel Functions
        private void doWheelColliderBehavior() //moves the wheel cluster along the collider.
        {
            foreach (WheelColliderMover cMover in m_wheelColliderMover)
            {
                cMover.GearCluster.transform.position = cMover.TargetTransform.transform.position;
            }
        }
        #endregion

        #region Serializable Data
        // This class presents a nice custom structure in which to define each of the plane's contol surfaces to animate.
        // They show up in the inspector as an array.
        [Serializable]
        public class ControlSurface // Control surfaces represent the different flaps of the aeroplane.
        {
            public enum Type // Flaps differ in position and rotation and are represented by different types.
            {
                Aileron, // Horizontal flaps on the wings, rotate on the x axis.
                Elevator, // Horizontal flaps used to adjusting the pitch of a plane, rotate on the x axis.
                Rudder, // Vertical flaps on the tail, rotate on the y axis.
                RuddervatorNegative, // Combination of rudder and elevator.
                RuddervatorPositive, // Combination of rudder and elevator.
                Airbrake,
                Flap,
                SteeringWheel,
            }

            public Transform transform; // The transform of the control surface.
            public float amount; // The amount by which they can rotate.
            public Type type; // The type of control surface.
            public int movementSmoothing;
            //public bool rotateXAxis;
            //public bool rotateYAxis;
            //public bool rotateZAxis;

            [HideInInspector] public Quaternion originalLocalRotation; // The rotation of the surface at the start.
        }

        [Serializable]
        public class WheelColliderMover
        {
            public Transform GearCluster;
            public Transform TargetTransform;
        }

        [Serializable]
        public class WheelClusterAnimator
        {
            public Transform wheelCluster;
            public WheelCollider wheelClusterReferenceWheel;
            public Transform wheelMeshToTurn;
        }
        #endregion
    }
}
