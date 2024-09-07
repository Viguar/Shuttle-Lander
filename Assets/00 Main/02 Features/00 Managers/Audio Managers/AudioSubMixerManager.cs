using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSubMixerManager : MonoBehaviour
{
    public AudioMixer Submixer;
    public string SubmixerParameter;
    public void SetSubmixerVolume(float Volume)
    {
        Submixer.SetFloat(SubmixerParameter, Volume);
    }
}
