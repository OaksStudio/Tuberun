using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puller : MonoBehaviour
{
    public Action<Direction, float> OnPull = delegate { };

    public TuberRow TuberRow => _tuberRow;

    private TuberRow _tuberRow;

    protected void Awake()
    {
        Initialize();
    }

    protected void Update()
    {
        Process();
    }

    protected virtual void Setup(TuberRow tuberRow)
    {
        _tuberRow = tuberRow;
    }

    protected abstract void Initialize();

    protected abstract void Process();
}
