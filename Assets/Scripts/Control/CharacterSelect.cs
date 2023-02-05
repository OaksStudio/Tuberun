using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jozi.Utilities.Patterns;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterSelect : Singleton<CharacterSelect>
{
    [ReadOnly] public static List<SOPuller> SelectedCompetitors => _selectedPullers;
    private static List<SOPuller> _selectedPullers = new List<SOPuller>();

#if UNITY_EDITOR
    [ReadOnly] public List<SOPuller> SelectedPullersPreview;
#endif

    public List<SOPlayerPuller> DefaultPlayers => _defaultPlayers;
    [SerializeField] private List<SOPlayerPuller> _defaultPlayers = new List<SOPlayerPuller>();

    public enum Difficulty { EASY = 0, NORMAL = 1, MEDIUM = 2, HARD = 3, INSANE = 4 }

    [Header("Bot Setups"), Range(0, 5)]
    public int MaxBotsDifficulties = 5;
    [SerializeField] private Difficulty DefaultDifficulty = Difficulty.NORMAL;
    [SerializeField] private List<BotsSetup> BotPullers = new List<BotsSetup>();

    public const int MaxPullers = 2;
    public Action<SOPuller> OnAdd, OnRemove, OnSwitch;

    protected override void Awake()
    {
        base.Awake();
        _selectedPullers = new List<SOPuller>();
    }

    public bool CanAdd()
    {
        if (_selectedPullers.Count >= MaxPullers) return false;
        return true;
    }

    [TitleGroup("Buttons")]
    [Button]
    public void Add(SOPuller addPuller)
    {
        if (!CanAdd() || _selectedPullers.Contains(addPuller)) return;
        _selectedPullers.Add(addPuller);
        OnRemove?.Invoke(addPuller);

#if UNITY_EDITOR
        SelectedPullersPreview = _selectedPullers;
#endif
    }

    [Button]
    public void Remove(SOPuller removePuller)
    {
        if (!_selectedPullers.Contains(removePuller)) return;
        _selectedPullers.Remove(removePuller);
        OnRemove?.Invoke(removePuller);
#if UNITY_EDITOR
        SelectedPullersPreview = _selectedPullers;
#endif
    }

    [Button]
    public void ConfirmAll()
    {
        if (CanAdd())
        {
            Add(GetBot());
        }
    }

    public SOBotPuller GetBot()
    {
        return GetBotPuller(DefaultDifficulty);
    }

    private SOBotPuller GetBotPuller(Difficulty difficulty)
    {
        return BotPullers.Find(b => b.Difficulty == difficulty).botPuller;
    }

    [Button]
    public Difficulty SwitchBot(bool right)
    {
        if (!_selectedPullers.Exists(s => s is SOBotPuller)) return DefaultDifficulty;

        int botIndex = _selectedPullers.FindIndex(s => s is SOBotPuller);
        BotsSetup botsSetup = BotPullers.Find(b => b.botPuller == (_selectedPullers[botIndex] as SOBotPuller));
        int pass = right ? 1 : -1;

        Difficulty selectedDifficulty = botsSetup.Difficulty;

        if ((int)selectedDifficulty + pass > MaxBotsDifficulties - 1)
        {
            selectedDifficulty = (Difficulty)0;
        }
        else if ((int)selectedDifficulty + pass < 0)
        {
            selectedDifficulty = (Difficulty)MaxBotsDifficulties - 1;
        }
        else
        {
            selectedDifficulty += pass;
        }

        _selectedPullers[botIndex] = GetBotPuller(selectedDifficulty);
        OnSwitch?.Invoke(_selectedPullers[botIndex]);

#if UNITY_EDITOR
        SelectedPullersPreview = _selectedPullers;
#endif

        return selectedDifficulty;

    }

    [Button]
    public void RemoveBots()
    {
        while (_selectedPullers.Exists(s => s is SOBotPuller))
        {
            Remove(_selectedPullers.Find(b => b is SOBotPuller));
        }

#if UNITY_EDITOR
        SelectedPullersPreview = _selectedPullers;
#endif
    }

    [Button]
    public void RemoveAll()
    {
        while (_selectedPullers.Count > 0)
        {
            Remove(_selectedPullers.First());
        }

#if UNITY_EDITOR
        SelectedPullersPreview = _selectedPullers;
#endif
    }

    [System.Serializable]
    public class BotsSetup
    {
        public Difficulty Difficulty = Difficulty.NORMAL;
        public SOBotPuller botPuller;
    }
}
