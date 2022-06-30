using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider audioSlider;
    private float sound;

    private void Update()
    {
        sound = audioSlider.value ;
    }

    public void AudioControl()
    {
        if (sound == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound);
    }
}
