using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Patterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Setup")]
    [SerializeField] private AudioCommand Music;
    [SerializeField] private AudioCommand Ambience;

    [SerializeField] private AudioMixerGroup MusicMixer;
    [SerializeField] private AudioMixerGroup SoundEffectMixer;

    [Header("Initialize")]
    [SerializeField] private SOAudio InitialMusic;
    [SerializeField] private SOAudio InitialAmbience;

    private void Start()
    {
        PlayMusic(InitialMusic);
        PlayAmbience(InitialAmbience);
    }

    public void PlayMusic(SOAudio audio)
    {
        Music.Play(audio);
    }

    public void PlayAmbience(SOAudio audio)
    {
        Ambience.Play(audio);
    }

    [Button]
    public void PlayMusic()
    {
        if (InitialMusic) PlayMusic(InitialMusic);
    }

    [Button]
    public void PlayAmbience()
    {
        if (InitialAmbience) PlayAmbience(InitialAmbience);
    }
}
