using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puller : MonoBehaviour
{

    public int ID => _ID;
    private int _ID;

    public Action<Direction, float> OnPull = delegate { };

    public Action OnStop, OnDisable = delegate { };

    public bool Stopped => _stopped;
    private bool _stopped;

    public bool Disabled => _stopped;
    private bool _disabled;

    protected void Awake()
    {
        Initialize();
    }

    protected void Update()
    {
        if (_disabled || _stopped) return;
        Process();
    }

    public virtual void Setup(int id, SOPuller puller)
    {
        _ID = id;
        _disabled = _stopped = false;
    }

    protected abstract void Initialize();

    protected abstract void Process();

    public void Stop()
    {
        _stopped = true;
        OnStop?.Invoke();
    }

    public void Disable()
    {
        _disabled = true;
        OnDisable?.Invoke();
    }
}
