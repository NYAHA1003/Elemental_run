using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer masterMixer;

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
}
