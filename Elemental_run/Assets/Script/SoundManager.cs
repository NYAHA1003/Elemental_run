using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer masterMixer;
    public Slider audioSlider;
    private float sound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        sound = audioSlider.value;
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        AudioMixerGroup[] groupArray = masterMixer.FindMatchingGroups("Master");
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = groupArray[0];
        audioSource.Play();

        Destroy(go, clip.length + 0.1f);
    }

    public void AudioControl()
    {
        if (sound == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound);
    }
}
