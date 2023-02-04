using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioCommand : MonoBehaviour
{
    [SerializeField] private SOAudio _audio;
    private AudioSource _audioSource;

    private void Awake()
    {
        Setup(_audio);
    }

    public void Setup(SOAudio audio)
    {
        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
        if (!_audio) return;

        _audio = audio;
        _audio.Setup(ref _audioSource);
    }

    [Button]
    public void Play()
    {
        Play(_audio);
    }

    [Button]
    public void Play(SOAudio audio)
    {
        _audioSource.pitch = audio.GetPitch();
        _audioSource.clip = audio.GetClip();
        _audioSource.Play();
    }
}
