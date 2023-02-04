using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private AudioMixerGroup musicMixerMute;
    private AudioMixerGroup soundEffectMixerMute;

    [Header("Volumes")]
    [Range(0.00001f, 1)] public float MasterVolume = 1;
    [Range(0.00001f, 1)] public float MusicVolume = .5f;
    [Range(0.00001f, 1)] public float SoundEffectVolume = .5f;

    [Header("Initialize")]
    [SerializeField] private SOAudio InitialMusic;
    [SerializeField] private SOAudio InitialAmbience;

    public static bool MusicMuted => _musicMuted;
    public static bool SfxMuted => _sfxMuted;

    [ReadOnly, SerializeField] private static bool _musicMuted;
    [ReadOnly, SerializeField] private static bool _sfxMuted;

    public static Action<AudioMixerGroup> OnMute, OnUnmute = delegate { };

    private void Start()
    {
        PlayMusic(InitialMusic);
        PlayAmbience(InitialAmbience);

        musicMixerMute = MusicMixer.audioMixer.FindMatchingGroups("Music").First();
        soundEffectMixerMute = SoundEffectMixer.audioMixer.FindMatchingGroups("SoundEffect").First();
    }

    public void PlayMusic(SOAudio audio)
    {
        Music.Play(audio);
    }

    public void PlayAmbience(SOAudio audio)
    {
        Ambience.Play(audio);
    }

    public static bool IsMuted(AudioMixerGroup mixer)
    {
        if (mixer == Instance.MusicMixer)
        {
            return MusicMuted;
        }
        if (mixer == Instance.SoundEffectMixer)
        {
            return SfxMuted;
        }

        return false;
    }

    private void Update()
    {
        SetVolume(MasterVolume, "Master", MusicMixer);
        SetVolume(MasterVolume, "Master", SoundEffectMixer);
        SetVolume(MusicVolume, "Music", MusicMixer);
        SetVolume(SoundEffectVolume, "SoundEffect", SoundEffectMixer);
    }

    public void SetVolume(float value, string name, AudioMixerGroup mixer)
    {
        mixer.audioMixer.SetFloat(name, Mathf.Log10(value) * 20);
    }

    [TitleGroup("Event Actions")]
    [Title("Play")]
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

    [Title("Mute")]
    [Button]
    public void MuteMusic()
    {
        Mute(musicMixerMute);
    }

    [Button]
    public void MuteSFX()
    {
        Mute(soundEffectMixerMute);
    }

    public static void Mute(AudioMixerGroup mixer)
    {
        if (mixer == Instance.musicMixerMute)
        {
            _musicMuted = !_musicMuted;
            if (_musicMuted) OnMute?.Invoke(Instance.musicMixerMute);
            else OnUnmute?.Invoke(Instance.musicMixerMute);
        }
        if (mixer == Instance.soundEffectMixerMute)
        {
            _sfxMuted = !_sfxMuted;
            if (_sfxMuted) OnMute?.Invoke(Instance.soundEffectMixerMute);
            else OnUnmute?.Invoke(Instance.soundEffectMixerMute);
        }
    }
}
