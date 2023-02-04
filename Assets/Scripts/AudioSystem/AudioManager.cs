using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Patterns;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    public SOAudio InitialMusic;
    public SOAudio InitialAmbience;

    public AudioCommand Music;
    public AudioCommand Ambience;

    private void Start()
    {
        if (InitialMusic) PlayMusic(InitialMusic);
        if (InitialAmbience) PlayAmbience(InitialAmbience);
    }

    public void PlayMusic(SOAudio audio)
    {
        Music.Play(audio);
    }

    public void PlayAmbience(SOAudio audio)
    {
        Ambience.Play(audio);
    }
}
