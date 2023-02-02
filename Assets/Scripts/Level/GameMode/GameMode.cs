using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    [Header("Info")]
    public SOHarvester harvesterInfo;

    [Header("Farm Rows")]
    public FarmType farmType;
    public List<TuberRow> TuberRows = new List<TuberRow>();

    public List<Harvester> Harvesters = new List<Harvester>();

    public Action<int> OnOneWins;
    public Action OnAllLost;

    public virtual void Setup(List<SOTuber> tubers)
    {
        for (int i = 0; i < TuberRows.Count; i++)
        {
            TuberRows[i].Setup(i, tubers, farmType);
        }

        foreach (var harvester in Harvesters)
        {
            harvester.Setup(harvesterInfo);
            harvester.OnHarvest += StopPuller;
        }
    }

    protected virtual void StopPuller(int pullerIndex)
    {
        if (GameManager.Instance.Pullers[pullerIndex].Stopped) return;

        GameManager.Instance.Pullers[pullerIndex].Stop();

        bool allStopped = true;
        for (int i = 0; i < GameManager.Instance.Pullers.Count; i++)
        {
            if (!GameManager.Instance.Pullers[i].Stopped)
            {
                allStopped = false;
            }
        }

        if (allStopped)
        {
            OnAllLost?.Invoke();
        }
    }

    protected virtual void EndGame()
    {
        for (int i = 0; i < TuberRows.Count; i++)
        {
            if (TuberRows[i].Clear)
            {
                OnOneWins?.Invoke(i);
                return;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var harvester in Harvesters)
        {
            harvester.OnHarvest -= StopPuller;
        }
    }
}
