using System;
using System.Collections;
using System.Collections.Generic;
using OAKS.Utilities.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBotBehaviour : ViewBehaviour
{
    [Header("Confirm Setup")]
    public HoldButton HoldConfirm;

    [Header("Next View")]
    [SerializeField] protected string NextViewName = "View-SelectGameMode";
    [SerializeField] private ViewBase NextView;

    [Header("Difficulty Setup")]
    public RectTransform DifficultyHolder;
    public TextMeshProUGUI DifficultyText;
    public PressButton PressAddDifficulty;
    public PressButton PressRemoveDifficulty;

    protected override void Awake()
    {
        base.Awake();

        HoldConfirm.OnComplete += OnConfirm;
        PressAddDifficulty.OnButtonDown += OnAdd;
        PressRemoveDifficulty.OnButtonDown += OnRemove;
    }

    private void Start()
    {
        if (NextView == null) NextView = _viewMenuController.GetViewById(NextViewName);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        ControlMap map = (CharacterSelect.SelectedCompetitors.Find(s => s is SOPlayerPuller) as SOPlayerPuller).ControlMap;

        ControlMap.Map confirm = map.KeysMapped.Find(k => k.InputAction == InputActions.CONFIRM);
        ControlMap.Map left = map.KeysMapped.Find(k => k.InputAction == InputActions.LEFT);
        ControlMap.Map right = map.KeysMapped.Find(k => k.InputAction == InputActions.RIGHT);

        HoldConfirm.Setup(confirm);
        PressRemoveDifficulty.Setup(left);
        PressAddDifficulty.Setup(right);

        HoldConfirm.Activate(true);

        if (CharacterSelect.SelectedCompetitors.Exists(s => s is SOBotPuller))
        {
            PressAddDifficulty.Activate(false);
            PressRemoveDifficulty.Activate(false);
            DifficultyHolder.gameObject.SetActive(false);
        }
        else
        {
            PressAddDifficulty.Activate(true);
            PressRemoveDifficulty.Activate(true);
            DifficultyHolder.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(DifficultyHolder);
        }
    }

    protected override void OnExit()
    {
        HoldConfirm.Activate(false);
        PressAddDifficulty.Activate(false);
        PressRemoveDifficulty.Activate(false);
    }

    protected override void OnStacked()
    {
        base.OnStacked();
        HoldConfirm.Activate(false);
        PressAddDifficulty.Activate(false);
        PressRemoveDifficulty.Activate(false);
    }

    protected override void OnUnstacked()
    {
        base.OnUnstacked();
        HoldConfirm.Activate(true);
        PressAddDifficulty.Activate(true);
        PressRemoveDifficulty.Activate(true);
    }

    private void OnConfirm()
    {
        if (NextView)
            _viewMenuController.PushView(NextView);
        else
            _viewMenuController.PushView(NextViewName);
    }

    private void OnAdd()
    {
        DifficultyText.text = CharacterSelect.Instance.SwitchBot(true).ToString();
    }

    private void OnRemove()
    {

        DifficultyText.text = CharacterSelect.Instance.SwitchBot(false).ToString();
    }

}
