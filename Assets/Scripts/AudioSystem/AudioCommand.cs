using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioCommand : MonoBehaviour
{
    [SerializeField] private SOAudio _audio;
    private AudioSource _audioSource;

    public void Setup(SOAudio audio)
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        if (audio == null) return;
        _audio = audio;
        _audio.Setup(ref _audioSource);
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
}
