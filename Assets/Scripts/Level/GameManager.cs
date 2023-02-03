using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Patterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static List<SOPuller> SelectedCompetitors = new List<SOPuller>();

    public List<Puller> Pullers => _pullers;
    public GameMode GameMode => _gameMode;

    public UnityEvent OnWonEvent, OnLoseEvent;

    [Header("Default Info")]
    public List<SOPuller> Competitors = new List<SOPuller>();
    public List<SOTuber> MatchTubers = new List<SOTuber>();

    [Header("Hidden")]
    [SerializeField, ReadOnly] private GameMode _gameMode;
    [SerializeField, ReadOnly] private List<Puller> _pullers = new List<Puller>();

    public Action<SOPuller> OnWon;
    public Action OnLost;

    private void Start()
    {
        _gameMode = FindObjectOfType<GameMode>();

        for (int i = 0; i < Competitors.Count; i++)
        {
            GameObject pullerHolder = new GameObject();
            pullerHolder.transform.parent = transform;
            if (Competitors[i] is SOPlayerPuller playerPuller)
            {
                pullerHolder.name = $"P_{playerPuller.name}";
                PlayerPuller puller = pullerHolder.AddComponent<PlayerPuller>();
                puller.Setup(i, playerPuller);
                _pullers.Add(puller);
            }
            else if (Competitors[i] is SOBotPuller botPuller)
            {
                pullerHolder.name = $"B_{botPuller.name}";
                BotPuller puller = pullerHolder.AddComponent<BotPuller>();
                puller.Setup(i, botPuller);
                _pullers.Add(puller);
            }
        }

        _gameMode.Setup(MatchTubers);
        _gameMode.OnOneWins += Won;
        _gameMode.OnAllLost += Losted;
    }

    private void Won(int pullerIndex)
    {
        DisablePullers();
        Debug.Log($"O fugitivo {Competitors[pullerIndex].PullerName} Ganhou!");
        OnWon?.Invoke(Competitors[pullerIndex]);
    }

    private void Losted()
    {
        DisablePullers();
        Debug.Log($"Todos os fugitivos perderam!");
        OnLost?.Invoke();
    }

    private void DisablePullers()
    {
        foreach (var puller in Pullers)
        {
            puller.Disable();
        }
    }

}

public enum Direction { UP = 0, DOWN = 1, RIGHT = 2, LEFT = 3, ANY = 4, NONE = 5 }