using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameMode : MonoBehaviour
{
    [Header("Info")]
    public SOGameMode GameModeInfo;
    public SOHarvester harvesterInfo;

    [Header("Farm Rows")]
    public FarmType farmType;
    public List<TuberRow> TuberRows = new List<TuberRow>();

    public List<Harvester> Harvesters = new List<Harvester>();

    public List<Transform> TubersTarget = new List<Transform>();

    public Action<int> OnOneWins;
    public Action OnAllLost;
    public UnityEvent OnCompetitorLost;

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
        TuberRows[pullerIndex].StopRow();

        OnCompetitorLost?.Invoke();

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
            EndProcedure();
            OnAllLost?.Invoke();
        }
    }

    private void EndProcedure()
    {
        for (int i = 0; i < TuberRows.Count; i++)
        {
            TuberRows[i].StopRow();
        }

        foreach (var harvester in Harvesters)
        {
            harvester.StopHarvester();
        }
    }

    protected virtual void EndGame()
    {
        for (int i = 0; i < TuberRows.Count; i++)
        {
            if (TuberRows[i].Clear)
            {
                EndProcedure();
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
