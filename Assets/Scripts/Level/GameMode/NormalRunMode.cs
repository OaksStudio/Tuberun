using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRunMode : GameMode
{
    public override void Setup(List<SOTuber> tubers)
    {
        base.Setup(tubers);

        for (int i = 0; i < TuberRows.Count; i++)
        {
            TuberRows[i].OnClearRow += EndGame;
        }
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
