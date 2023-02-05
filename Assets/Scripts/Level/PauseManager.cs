using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Patterns;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public float ScaledTime = 0;
    public bool CanPause = true;
    public bool IsPaused = false;

    private void Start()
    {
        IsPaused = false;
    }

    public void Pause()
    {
        if (!CanPause) return;
        Time.timeScale = ScaledTime;
        IsPaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }

    private void OnDestroy()
    {
        UnPause();
    }
}
