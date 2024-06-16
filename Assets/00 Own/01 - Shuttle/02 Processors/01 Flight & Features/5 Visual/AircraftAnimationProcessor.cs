using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viguar.Aircraft
{
    [RequireComponent(typeof(AircraftBaseProcessor))]
    public class AircraftAnimationProcessor : MonoBehaviour
    {
        private AircraftBaseProcessor _configBaseProcessor;



        private void Start()
        {
            _configBaseProcessor = GetComponent<AircraftBaseProcessor>();
            InitializeControlSurfaces();
        }
        private void Update()
        {
            MoveControlSurfaces();
            MoveAvionicsNeedles();
        }

        private void MoveControlSurfaces()
        {
            foreach (ControlSurface cs in _configBaseProcessor._CSA._cControlSurfacesAnimations)
            {
                switch(cs.AnimationType)
                {
                    case ControlSurface.SurfaceAnimationType.Script:
                        _configBaseProcessor.DefineControlSurfaceDictionary();
                        _configBaseProcessor.ControlSurfaceDict.TryGetValue(cs.rotationValue, out float rotVal);
                        RotateAroundControlSurfaceRotationAxis(rotVal * cs.RotationAmount, cs);
                        break;
                    case ControlSurface.SurfaceAnimationType.Animation:
                        _configBaseProcessor.DefineControlTriggersDictionary();
                        _configBaseProcessor.ControlTriggersDict.TryGetValue(cs.triggeringBool, out bool trigger);
                        RunAnimationOnBoolTrigger(cs, trigger);
                        break;
                }                                                                                       
            }
        }
        private void MoveAvionicsNeedles()
        {
            if(_configBaseProcessor._AnimateAvionics) { ProcessNeedleWithDisplayStyle(); }        
        }

        private void InitializeAvionicsNeedles()
        {
            foreach (AvionicsInstrument av in _configBaseProcessor._CAA._cAvionicsInstrument)
            {
                switch (av.dispType)
                {
                    case AvionicsInstrument.displayMedium.Analog:
                        break;
                    case AvionicsInstrument.displayMedium.Digital:
                        break;
                }
            }
        }
        private void InitializeControlSurfaces()
        {
            foreach (ControlSurface cs in _configBaseProcessor._CSA._cControlSurfacesAnimations)
            {
                switch (cs.AnimationType)
                {                    
                    case ControlSurface.SurfaceAnimationType.Script:                  
                        cs.OriginalRotationAmount = cs.RotationAnchor.transform.localRotation;
                        break;
                    case ControlSurface.SurfaceAnimationType.Animation:
                        _configBaseProcessor.StartingStateDict.TryGetValue(cs.startInStartAnimationState, out bool startsInState);
                        if (startsInState)
                        {
                            cs.CSAnimator.SetBool(cs.ToggleAnimationBoolName, false);
                            cs.CSAnimator.SetBool(cs.StartAnimationStateBoolName, true);
                            cs.CSAnimatorState = cs.CSAnimator.GetNextAnimatorStateInfo(0);
                            cs.AnimationStateA = true;
                        }
                        else
                        {
                            cs.CSAnimator.SetBool(cs.ToggleAnimationBoolName, true);
                            cs.CSAnimator.SetBool(cs.StartAnimationStateBoolName, false);
                            cs.CSAnimatorState = cs.CSAnimator.GetNextAnimatorStateInfo(0);
                            cs.AnimationStateA = false;
                        }
                        cs.AnimationFinished = true;
                        cs.OnAnimationFinished.Invoke();
                        break;
                }
            }
        }
        
        private void RunAnimationOnBoolTrigger(ControlSurface cs, bool triggered)
        {
            cs.CSAnimatorState = cs.CSAnimator.GetNextAnimatorStateInfo(0);
            if(triggered && cs.AnimationFinished)
            {
                if (cs.AnimationStateA)
                {
                    cs.CSAnimator.SetBool(cs.ToggleAnimationBoolName, true);
                    cs.PassedTime = 0;
                    cs.AnimationFinished = false;
                    cs.CSAnimatorState = cs.CSAnimator.GetCurrentAnimatorStateInfo(0);
                    cs.AnimationStateLength = cs.CSAnimatorState.length;
                    cs.AnimationStateA = false;
                }
                else
                {
                    cs.CSAnimator.SetBool(cs.ToggleAnimationBoolName, false);
                    cs.PassedTime = 0;
                    cs.AnimationFinished = false;
                    cs.CSAnimatorState = cs.CSAnimator.GetCurrentAnimatorStateInfo(0);
                    cs.AnimationStateLength = cs.CSAnimatorState.length;
                    cs.AnimationStateA = true;
                }
            }
            else
            {
                if(!cs.AnimationFinished)
                {
                    if(cs.PassedTime < cs.AnimationStateLength)
                    {
                        cs.PassedTime += Time.deltaTime;
                    }
                    else
                    {
                        cs.AnimationFinished = true;
                        cs.OnAnimationFinished.Invoke();
                    }
                }
            }
        }
        private void RotateAroundControlSurfaceRotationAxis(float value, ControlSurface consur)
        {
                switch(consur.AnimationType)
                {                    
                    case ControlSurface.SurfaceAnimationType.Script:
                        switch (consur.RotationAxis)
                        {                            
                            case ControlSurface.RotationAxisType.XAxis:
                            consur.RotationAxisQ = Quaternion.Euler(value, 0, 0);
                            consur.RotationAnchor.localRotation = Quaternion.Slerp(consur.RotationAnchor.localRotation, consur.RotationAxisQ * consur.OriginalRotationAmount, consur.RotationSmoothing * Time.deltaTime);
                                break;
                            case ControlSurface.RotationAxisType.YAxis:
                            consur.RotationAxisQ = Quaternion.Euler(0, value, 0);
                            consur.RotationAnchor.localRotation = Quaternion.Slerp(consur.RotationAnchor.localRotation, consur.RotationAxisQ * consur.OriginalRotationAmount, consur.RotationSmoothing * Time.deltaTime);
                                break;
                            case ControlSurface.RotationAxisType.ZAxis:
                            consur.RotationAxisQ = Quaternion.Euler(0, 0, value);
                            consur.RotationAnchor.localRotation = Quaternion.Slerp(consur.RotationAnchor.localRotation, consur.RotationAxisQ * consur.OriginalRotationAmount, consur.RotationSmoothing * Time.deltaTime);
                                break;
                        }
                        break;
                    case ControlSurface.SurfaceAnimationType.Animation:
                        break;
                }
            
        }

        private void ProcessNeedleWithDisplayStyle()
        {
            foreach (AvionicsInstrument av in _configBaseProcessor._CAA._cAvionicsInstrument)
            {
                switch (av.dispStyle)
                {
                    case AvionicsInstrument.displayStyle.AsymmetricalRotary:
                        AnimateAsymmetricRotary(av.RotaryNeedleConstraint, av);
                        break;
                    case AvionicsInstrument.displayStyle.SymmetricalRotary:
                        AnimateSymmetricRotaty(av.RotaryNeeldeConstraints, av.RotaryNeedleMaxTurn, av);
                        break;
                    case AvionicsInstrument.displayStyle.SymmetricalLinear:
                        break;
                    case AvionicsInstrument.displayStyle.SymmetricalDigits:
                        break;
                }
            }
        }
        private void AnimateAsymmetricRotary(float bottomLimit, AvionicsInstrument avInst)
        {
            //Turn the desired variable from the input string from the specific needle into a float.
            _configBaseProcessor.DefineAvionicsDictionary();
            _configBaseProcessor.AvionicsDict.TryGetValue(avInst.ReferenceVariableName, out float dVal);
            avInst.readValue = dVal;

            //Move every needle of the dial by its divisor factor.
            foreach (AvionicsInstrumentNeedle needle in avInst.InstrumentNeedle)
            {
                switch (needle.NeedleFactor)
                {
                    case AvionicsInstrumentNeedle.needleValueMultiplier.One:
                        needle.movingFactor = 1;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Ten:
                        needle.movingFactor = 10;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Hundred:
                        needle.movingFactor = 100;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Thousand:
                        needle.movingFactor = 1000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Tenthousand:
                        needle.movingFactor = 10000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Hundredthousand:
                        needle.movingFactor = 100000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Million:
                        needle.movingFactor = 1000000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Custom:
                        needle.movingFactor = needle.CustomNeedleFactor;
                        break;
                }
                needle.targetRotation = (avInst.readValue > bottomLimit ? (avInst.readValue / needle.movingFactor) * 360 : bottomLimit); //The target rotation
                switch (needle.NeedleOneAxis)
                {
                    case AvionicsInstrumentNeedle.needleAxisSingle.X:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(needle.targetRotation, 0, 0), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(needle.targetRotation, 0, 0);
                        break;
                    case AvionicsInstrumentNeedle.needleAxisSingle.Y:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(0, needle.targetRotation, 0), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(0, needle.targetRotation, 0);
                        break;
                    case AvionicsInstrumentNeedle.needleAxisSingle.Z:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(0, 0, needle.targetRotation), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(0, 0, needle.targetRotation);
                        break;
                }
            }            
        }
        private void AnimateSymmetricRotaty(Vector2 valueLimits, float maxTurnDegrees, AvionicsInstrument avInst)
        {
            //Turn the desired variable from the input string from the specific needle into a float.
            _configBaseProcessor.DefineAvionicsDictionary();
            _configBaseProcessor.AvionicsDict.TryGetValue(avInst.ReferenceVariableName, out float dVal);
            avInst.readValue = dVal;
            foreach (AvionicsInstrumentNeedle needle in avInst.InstrumentNeedle)
            {
                switch (needle.NeedleFactor)
                {
                    case AvionicsInstrumentNeedle.needleValueMultiplier.One:
                        needle.movingFactor = 1;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Ten:
                        needle.movingFactor = 10;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Hundred:
                        needle.movingFactor = 100;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Thousand:
                        needle.movingFactor = 1000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Tenthousand:
                        needle.movingFactor = 10000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Hundredthousand:
                        needle.movingFactor = 100000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Million:
                        needle.movingFactor = 1000000;
                        break;
                    case AvionicsInstrumentNeedle.needleValueMultiplier.Custom:
                        needle.movingFactor = needle.CustomNeedleFactor;
                        break;
                }
                float rotationMarginLimits = Mathf.InverseLerp(0, 360, maxTurnDegrees);

                if (avInst.readValue < valueLimits.x) { avInst.readValue = valueLimits.x; }
                if (avInst.readValue > valueLimits.y) { avInst.readValue = valueLimits.y; }                                       
                needle.targetRotation = (avInst.readValue / needle.movingFactor) * 360 * rotationMarginLimits;                              
                switch (needle.NeedleOneAxis)
                {
                    case AvionicsInstrumentNeedle.needleAxisSingle.X:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(needle.targetRotation, 0, 0), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(needle.targetRotation, 0, 0);
                        break;
                    case AvionicsInstrumentNeedle.needleAxisSingle.Y:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(0, needle.targetRotation, 0), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(0, needle.targetRotation, 0);
                        break;
                    case AvionicsInstrumentNeedle.needleAxisSingle.Z:
                        needle.Needle.localRotation = Quaternion.Slerp(needle.Needle.localRotation, Quaternion.Euler(0, 0, needle.targetRotation), avInst.AvionicsRotationSmoothing * Time.deltaTime);
                        //needle.Needle.localRotation = Quaternion.Euler(0, 0, needle.targetRotation);
                        break;
                }
            }
            
        }
        private void AnimateSymmetricLinear()
        {

        }
        private void AnimateSymmetricDigits()
        {

        }
     
    }
}
