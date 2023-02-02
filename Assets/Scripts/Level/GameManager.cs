using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Utilities.Patterns;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static List<SOPuller> SelectedCompetitors = new List<SOPuller>();

    public List<Puller> Pullers => _pullers;

    [Header("Default Info")]
    public List<SOPuller> Competitors = new List<SOPuller>();
    public List<SOTuber> MatchTubers = new List<SOTuber>();

    [Header("Setup")]
    [SerializeField] private GameMode GameMode;
    [SerializeField] private List<Puller> _pullers = new List<Puller>();

    public Action<SOPuller> OnWon;
    public Action OnLost;

    private void Start()
    {
        GameMode = FindObjectOfType<GameMode>();

        foreach (var competitor in Competitors)
        {
            GameObject pullerHolder = new GameObject();
            pullerHolder.transform.parent = transform;
            if (competitor is SOPlayerPuller playerPuller)
            {

                pullerHolder.name = $"P_{playerPuller.name}";
                PlayerPuller puller = pullerHolder.AddComponent<PlayerPuller>();
                puller.Setup(playerPuller);
                _pullers.Add(puller);
            }
            else if (competitor is SOBotPuller botPuller)
            {
                pullerHolder.name = $"B_{botPuller.name}";
                BotPuller puller = pullerHolder.AddComponent<BotPuller>();
                puller.Setup(botPuller);
                _pullers.Add(puller);
            }
        }

        GameMode.Setup(MatchTubers);
        GameMode.OnOneWins += Won;
        GameMode.OnAllLost += Losted;
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