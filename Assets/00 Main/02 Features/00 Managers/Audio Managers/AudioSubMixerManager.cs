using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSubMixerManager : MonoBehaviour
{
    public AudioMixer Submixer;
    public Slider UISlider;
    public string SubmixerParameter;

    private void Start()
    {
        UISlider.SetValueWithoutNotify(Submixer.GetFloat(SubmixerParameter, out float value) ? value : 0f);
    }

    public void SetSubmixerVolume(float Volume)
    {
        Submixer.SetFloat(SubmixerParameter, Volume);
    }


}
