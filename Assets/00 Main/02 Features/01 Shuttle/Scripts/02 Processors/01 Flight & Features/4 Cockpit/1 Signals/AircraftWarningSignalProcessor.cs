using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Inspector.PropertyFields;

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

        private AudioSource cuePlaybackSource;
        private AudioClip cuePlaybackClip;        

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
            //cuePlaybackSource = GameObject.FindGameObjectWithTag("cockpitAlarmSystemAudio").GetComponent<AudioSource>();
            cuePlaybackSource = GetComponent<AudioSource>();
            cueLoopFrequencySlow = 1 / clfs;
            cueLoopFrequencyFast = 1 / clff;
        }

        public void HandleWarningSignals()
        {
            _configBaseProcessor.DefineDebugDictionaryString();
            _configBaseProcessor.DebugStringDict.TryGetValue(VariableName, out string CurrentVariableString);

            if (CurrentVariableString != lastStateString) //If the state has changed, we need to adjust the timers of the lights / audio! In this case we also unmute the audio should it have been manually muted.
            {
                OverrideUnmute();
                foreach (WarningSignalProperties SignalProperty in SignalProperties)
                {
                    switch (SignalProperty.BlinkSpeed) //Handle the current frequency of light flashing.
                    {
                        case Aircraft.WarningSignalProperties.BlinkSpeeds.Continuous:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = 0; }
                            break;
                        case Aircraft.WarningSignalProperties.BlinkSpeeds.Slow:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = blinkingFrequencySlow;  }
                            break;
                        case Aircraft.WarningSignalProperties.BlinkSpeeds.Fast:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = blinkingFrequencyFast;  }
                            break;
                        case Aircraft.WarningSignalProperties.BlinkSpeeds.Off:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { blinkTimerTarget = -1;  }
                            break;
                    }
                    switch (SignalProperty.CueSpeed) //Handle the current playback of audio.
                    {
                        case Aircraft.WarningSignalProperties.CueSpeeds.CueContinuously:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = 0; cuePlaybackSource.clip = SignalProperty.CueClip; }
                            break;
                        case Aircraft.WarningSignalProperties.CueSpeeds.CueSlowLoop:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = cueLoopFrequencySlow; cuePlaybackSource.clip = SignalProperty.CueClip;}
                            break;
                        case Aircraft.WarningSignalProperties.CueSpeeds.CueFastLoop:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = cueLoopFrequencyFast; cuePlaybackSource.clip = SignalProperty.CueClip; }
                            break;
                        case Aircraft.WarningSignalProperties.CueSpeeds.Once:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = -1; cuePlaybackSource.clip = SignalProperty.CueClip; cuePlaybackSource.Play(); }
                            break;
                        case Aircraft.WarningSignalProperties.CueSpeeds.Off:
                            if (CurrentVariableString == SignalProperty.OnVariableValue) { cueLoopTimerTarget = -1;  }
                            break;
                    }
                }
            }             
            lastStateString = CurrentVariableString; //Set the value equal, so we can check again in the next frame whether or not the state has changed!           
            HandleLightLogic(); //Run the logic for light
            HandleAudioLogic(); //Run the logic for audio
        }

        public void OverrideMute()
        {
            if(hasAudioSignal && respondsToMuteButton)
            {
                cuePlaybackSource.mute = true;
            }
        }
        public void OverrideUnmute()
        {
            if (hasAudioSignal && respondsToMuteButton)
            {
                cuePlaybackSource.mute = false;
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
            if (cueLoopTimerTarget != -1f)
            {
                if (cueLoopTimerTarget == 0)
                {
                    if (cuePlaybackSource.clip != null && !cuePlaybackSource.isPlaying) { cuePlaybackSource.Play(); }
                    cueLoopTimer = 0.0f;
                }
                else
                {
                    if (cueLoopTimer > cueLoopTimerTarget)
                    {
                        if (cuePlaybackSource.clip != null && !cuePlaybackSource.isPlaying) { cuePlaybackSource.Play(); }
                        cueLoopTimer = 0.0f;
                    }
                    else
                    {
                        cueLoopTimer += Time.deltaTime;
                    }
                }
            }
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