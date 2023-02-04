using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SO_AudioInfo", menuName = "AudioSystem/AudioInfo", order = 0)]
public class SOAudio : ScriptableObject
{
    [SerializeField] private List<AudioClip> Clip = new List<AudioClip>();
    [SerializeField, Range(0, 1)] private float Volume = 1f;
    [SerializeField, Range(-3, 3)] private float Pitch = 1f;
    [SerializeField] private bool Loop;

    [Header("Random Pitch")]
    [SerializeField] private bool RandomizePitch;
    [SerializeField, Range(-3, 3)] private float MinPitch = 1f;
    [SerializeField, Range(-3, 3)] private float MaxPitch = 1f;

    public AudioMixerGroup Mixer => _mixer;
    [SerializeField] private AudioMixerGroup _mixer;

    public void Setup(ref AudioSource audioSource)
    {
        audioSource.volume = Volume;
        audioSource.pitch = Pitch;
        audioSource.outputAudioMixerGroup = _mixer;
#if UNITY_EDITOR
        if (!_mixer) Debug.Log($"<color='red'>The Audio {name} does note have and mixer attached!</color>");
#endif
        audioSource.loop = Loop;
    }

    public float GetPitch()
    {
        if (RandomizePitch) return Random.Range(MinPitch, MaxPitch);
        return Pitch;
    }

    public AudioClip GetClip()
    {
        if (Clip.Count <= 0) return null;
        if (Clip.Count == 1) return Clip.First();
        return Clip[Random.Range(0, Clip.Count)];
    }
}
