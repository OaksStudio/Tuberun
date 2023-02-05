using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioCommand : MonoBehaviour
{
    [SerializeField] private SOAudio _audio;
    private AudioSource _audioSource;

    private void Awake()
    {
        AudioManager.OnMute += MuteProcedure;
        AudioManager.OnUnmute += UnmuteProcedure;
    }

    public void Setup(SOAudio audio)
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        if (audio == null) return;
        _audio = audio;
        _audio.Setup(ref _audioSource);
        if (_audio.Mixer) _audioSource.mute = AudioManager.IsMuted(_audio.Mixer);
    }

    [Button]
    public void Setup()
    {
        Setup(_audio);
    }

    [Button]
    public void Play()
    {
        Play(_audio);
    }

    [Button]
    public void Play(SOAudio audio)
    {
        Setup(audio);
        if (_audio == null) return;
        _audioSource.pitch = _audio.GetPitch();
        _audioSource.clip = _audio.GetClip();
        _audioSource.Play();
    }

    [Button]
    public void Stop()
    {
        Stop(_audio);
    }

    [Button]
    public void Stop(SOAudio audio)
    {
        Setup(audio);
        if (_audio == null) return;
        _audioSource.Stop();
    }

    private void OnDestroy()
    {
        AudioManager.OnMute -= MuteProcedure;
        AudioManager.OnUnmute -= UnmuteProcedure;
    }

    private void MuteProcedure(AudioMixerGroup mixer)
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        if (_audio.Mixer != mixer) return;
        _audioSource.mute = true;
        Debug.Log($"Mute {name} = {_audio.Mixer.name}{mixer.name}");
    }

    private void UnmuteProcedure(AudioMixerGroup mixer)
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        if (_audio.Mixer != mixer) return;
        _audioSource.mute = false;
        Debug.Log($"Unmute {name} = {_audio.Mixer.name}{mixer.name}");
    }
}
