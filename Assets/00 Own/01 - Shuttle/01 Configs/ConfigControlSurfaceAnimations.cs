using System;
using UnityEngine;
using UnityEngine.Events;
using Viguar.EditorTooling.InspectorUITools.ConditionalPropertyDisplay;
using Viguar.EditorTooling.InspectorUITools.OverrideLabels;

[Serializable]
public class ConfigControlSurfaceAnimations
{
    public ControlSurface[] _cControlSurfacesAnimations;
}

[Serializable]
public class ControlSurface
{
    
    public enum SurfaceAnimationType { Script, Animation }
    public enum RotationAxisType { XAxis, YAxis, ZAxis }

    public string AnimatedControlSurface;
    [Space(10)]
    [LabelOverride("Animate via")] public SurfaceAnimationType AnimationType;    
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public Transform RotationAnchor;
    [Space(10)]
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public RotationAxisType RotationAxis;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public string rotationValue;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public float RotationAmount;
    [DrawIf("AnimationType", SurfaceAnimationType.Script)] public int RotationSmoothing;

    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string startInStartAnimationState;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string triggeringBool;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public Animator CSAnimator;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string StartAnimationStateBoolName;
    [DrawIf("AnimationType", SurfaceAnimationType.Animation)] public string ToggleAnimationBoolName;
    public UnityEvent OnAnimationFinished = new UnityEvent();
    [HideInInspector] public Quaternion OriginalRotationAmount;
    [HideInInspector] public Quaternion RotationAxisQ;
    [HideInInspector] public AnimatorStateInfo CSAnimatorState;
    [HideInInspector] public float AnimationStateLength;
    [HideInInspector] public float PassedTime;
    [HideInInspector] public bool AnimationFinished;
    [HideInInspector] public bool AnimationStateA;
}

