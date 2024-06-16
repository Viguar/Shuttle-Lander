using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.EditorTooling.GUITools.ConditionalPropertyDisplay;

namespace Viguar.Aircraft
{
    public class AircraftWarningSignalProcessor : MonoBehaviour
    {

        //Signal Properties Constructor
        private AircraftBaseProcessor _configBaseProcessor;
        [SerializeField] private bool hasLightSignal;
        [SerializeField] private bool hasAudioSignal;
        [DrawIf("hasAudioSignal", true)][SerializeField] private bool respondsToMuteButton;
        
        
        private string lastStateString;
              
        //Light Signal Constructor
        private enum lightColors { White, Green, Red, }
        private float blinkingFrequencySlow;
        private float blinkingFrequencyFast;
        private float blinkTimerTarget;
        private float blinkTimer;
        private MeshFilter MeshFilterOff;
        private MeshFilter MeshFilterOn;
        private MeshRenderer MeshRendererOn;
        private MeshRenderer MeshRendererOff;
        [SerializeField] private string VariableName;

        [Space(10)]
        [DrawIf("hasLightSignal", true)][SerializeField] private lightColors lightColor;
        [DrawIf("hasLightSignal", true)][SerializeField] private Mesh white;
        [DrawIf("hasLightSignal", true)][SerializeField] private Mesh green;
        [DrawIf("hasLightSignal", true)][SerializeField] private Mesh red;

        //Audio Signal Constructor        
        private float cueLoopFrequencySlow;
        private float cueLoopFrequencyFast;
        private float cueLoopTimerTarget;
        private float cueLoopTimer;
        private bool cueOnceLock;
        private AudioSource cuePlaybackSource;
        private bool acknowledgedMuteOverride = false;
        
        //BlinkProperties
        [Space(10)]
        [SerializeField] private WarningSignalProperties[] SignalProperties;


        public void InitialiseLight(float slow, float fast)
        {
            _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();

            foreach(Transform child in transform)
            {
                if(child.tag == "cockpitLightOff") { MeshFilterOff = child.gameObject.GetComponent<MeshFilter>(); MeshRendererOff = child.gameObject.GetComponent<MeshRenderer>(); }
                if(child.tag == "cockpitLightOn") { MeshFilterOn = child.gameObject.GetComponent<MeshFilter>(); MeshRendererOn = child.gameObject.GetComponent<MeshRenderer>(); }
            }         

            switch (lightColor)
            {
                case lightColors.White:
                    MeshFilterOff.mesh = white;
                    MeshFilterOn.mesh = white;
                    break;
                case lightColors.Green:
                    MeshFilterOff.mesh = green;
                    MeshFilterOn.mesh = green;
                    break;
                case lightColors.Red:
                    MeshFilterOff.mesh = red;
                    MeshFilterOn.mesh = red;
                    break;
            }
            MeshRendererOn.enabled = false;
            MeshRendererOff.enabled = true;
            blinkingFrequencySlow = 1 / slow;
            blinkingFrequencyFast = 1 / fast;
            blinkTimer = 0.0f;
        }

        public void InitialiseAudio(float clfs, float clff)
        {
            cuePlaybackSource = GetComponent<AudioSource>();
            if (!hasAudioSignal)
            {
                cuePlaybackSource.enabled = false;
            }
            else
            {
                cuePlaybackSource.enabled = true;
            }
            cueLoopFrequencySlow = 1 / clfs;
            cueLoopFrequencyFast = 1 / clff;
        }

        public void HandleWarningSignals()
        {
            _configBaseProcessor.DefineDebugDictionaryString();
            _configBaseProcessor.DebugStringDict.TryGetValue(VariableName, out string VariableString);


            foreach (WarningSignalProperties SignalProperty in SignalProperties)
            {
                switch (SignalProperty.BlinkSpeed) //Handle the current frequency of light flashing.
                {
                    case Aircraft.WarningSignalProperties.BlinkSpeeds.Continuous:
                        if (VariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = 0; CheckForStateChange(VariableString); }
                        break;
                    case Aircraft.WarningSignalProperties.BlinkSpeeds.Slow:
                        if (VariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = blinkingFrequencySlow; CheckForStateChange(VariableString); }                                                                          
                        break;
                    case Aircraft.WarningSignalProperties.BlinkSpeeds.Fast:
                        if (VariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = blinkingFrequencyFast; CheckForStateChange(VariableString); }              
                        break;
                    case Aircraft.WarningSignalProperties.BlinkSpeeds.Off:                        
                        if (VariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = -1; CheckForStateChange(VariableString); }
                        break;
                }
                switch (SignalProperty.CueSpeed) //Handle the current playback of audio.
                {
                    case Aircraft.WarningSignalProperties.CueSpeeds.CueContinuously:
                        if (VariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = 0; cuePlaybackSource.resource = SignalProperty.CueClip; CheckForStateChange(VariableString); }
                        break;
                    case Aircraft.WarningSignalProperties.CueSpeeds.CueSlowLoop:
                        if (VariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = cueLoopFrequencySlow; cuePlaybackSource.resource = SignalProperty.CueClip; CheckForStateChange(VariableString); }
                        break;
                    case Aircraft.WarningSignalProperties.CueSpeeds.CueFastLoop:
                        if (VariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = cueLoopFrequencyFast; cuePlaybackSource.resource = SignalProperty.CueClip; CheckForStateChange(VariableString); }
                        break;
                    case Aircraft.WarningSignalProperties.CueSpeeds.Once:
                        if (VariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = -2; cuePlaybackSource.resource = SignalProperty.CueClip; CheckForStateChange(VariableString); }
                        break;
                    case Aircraft.WarningSignalProperties.CueSpeeds.Off:
                        if (VariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = -1; cuePlaybackSource.resource = null; CheckForStateChange(VariableString); }
                        break;
                }
            }
            HandleLightLogic();
            HandleAudioLogic();
        }

        public void OverrideMute()
        {
            if(hasAudioSignal && respondsToMuteButton)
            {
                acknowledgedMuteOverride = true;
            }
        }

        public void OverrideUnmute()
        {
            if (hasAudioSignal && respondsToMuteButton)
            {
                acknowledgedMuteOverride = false;
            }
        }

        private void HandleLightLogic()
        {
            if (blinkTimerTarget == -1) //If the light is supposed to be on continuously.
            {
                MeshRendererOn.enabled = false;
                MeshRendererOff.enabled = true;
                blinkTimer = 0.0f;
            }
            else if (blinkTimerTarget == 0) //If the light is supposed to be off.
            {
                MeshRendererOn.enabled = true;
                MeshRendererOff.enabled = false;
                blinkTimer = 0.0f;
            }
            else //Light flash logic.
            {
                if (blinkTimer > blinkTimerTarget)
                {
                    MeshRendererOn.enabled = !MeshRendererOn.enabled;
                    MeshRendererOff.enabled = !MeshRendererOff.enabled;
                    blinkTimer = 0.0f;
                }
                else
                {
                    blinkTimer += Time.deltaTime;
                }
            }
        }
        private void HandleAudioLogic()
        {
            cuePlaybackSource.mute = acknowledgedMuteOverride;
            if (cueLoopTimerTarget == -1)
            {
                cuePlaybackSource.Stop();
                cueLoopTimer = 0.0f;
            }
            else if (cueLoopTimerTarget == -2)
            {
                if(!cueOnceLock)
                {
                    if (!cuePlaybackSource.isPlaying) { if (cuePlaybackSource.resource != null) { cuePlaybackSource.Play(); } }
                    cueOnceLock = true;
                    cueLoopTimer = 0.0f;
                }
            }
            else if (cueLoopTimerTarget == 0)
            {
                if(!cuePlaybackSource.isPlaying) { if (cuePlaybackSource.resource != null) { cuePlaybackSource.Play(); } }               
                cueLoopTimer = 0.0f;
                cueOnceLock = false;
            }
            else
            {
                if(cueLoopTimer > cueLoopTimerTarget)
                {
                    if (!cuePlaybackSource.isPlaying) { if (cuePlaybackSource.resource != null) { cuePlaybackSource.Play(); } }
                    cueLoopTimer = 0.0f;
                    cueOnceLock = false;
                }
                else
                {
                    cueLoopTimer += Time.deltaTime;
                    cueOnceLock = false;
                }
            }            
        } 
        private void CheckForStateChange(string currentStateString)
        {
            if (currentStateString == lastStateString) { } //State did not change. We do nothing.
            else { OverrideUnmute(); } //State did change. We unmute our audiosource.
            lastStateString = currentStateString;
        }
    }

    [Serializable]
    public class WarningSignalProperties
    {        
        public enum BlinkSpeeds { Slow, Fast, Continuous, Off, }   
        public enum CueSpeeds { CueSlowLoop, CueFastLoop, CueContinuously, Once, Off, }

        public string OnVariableValue;
        public BlinkSpeeds BlinkSpeed;
        public CueSpeeds CueSpeed;
        public AudioClip CueClip;
    }
}        