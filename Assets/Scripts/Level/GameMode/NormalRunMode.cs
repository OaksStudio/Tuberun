using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRunMode : GameMode
{

    private float _initialTime;

    public override void Setup(List<SOTuber> tubers)
    {
        base.Setup(tubers);

        for (int i = 0; i < TuberRows.Count; i++)
        {
            TuberRows[i].OnClearRow += EndGame;
        }
        _initialTime = Time.time;
        GameManager.Instance.SetTime(Time.time - _initialTime, 0);
        GameManager.Instance.SetTime(Time.time - _initialTime, 1);
    }

    private void Update()
    {
        GameManager.Instance.SetTime(Time.time - _initialTime, 0);
        GameManager.Instance.SetTime(Time.time - _initialTime, 1);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < TuberRows.Count; i++)
        {
            TuberRows[i].OnClearRow -= EndGame;
        }
    }
}
