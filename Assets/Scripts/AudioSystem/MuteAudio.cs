using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MuteAudio : MonoBehaviour
{
    public bool IsMutingMusic = true;
    public UnityEvent OnMute, OnUnmute;

    private void Start()
    {
        CheckMutedStatus();
    }

    private void OnEnable()
    {
        CheckMutedStatus();
    }

    private void CheckMutedStatus()
    {
        if (IsMutingMusic)
        {
            if (AudioManager.MusicMuted) OnMute?.Invoke();
            else OnUnmute?.Invoke();
        }
        else
        {
            if (AudioManager.SfxMuted) OnMute?.Invoke();
            else OnUnmute?.Invoke();
        }
    }

    public void Mute()
    {
        if (AudioManager.MusicMuted && IsMutingMusic || !IsMutingMusic && AudioManager.SfxMuted)
        {
            OnUnmute?.Invoke();
        }
        else
        {
            OnMute?.Invoke();
        }
        if (IsMutingMusic) AudioManager.Instance.MuteMusic();
        else AudioManager.Instance.MuteSFX();
    }
}