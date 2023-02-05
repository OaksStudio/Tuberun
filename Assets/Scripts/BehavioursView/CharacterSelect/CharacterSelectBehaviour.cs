using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using OAKS.Utilities.Views;

public class CharacterSelectBehaviour : ViewBehaviour
{
    [Header("Next View")]
    [SerializeField] protected string NextViewName = "View-SelectGameMode";
    [SerializeField] private ViewBase NextView;
    [Header("Setup")]
    public CompetitorView Competitor_1;
    public CompetitorView Competitor_2;

    protected override void Awake()
    {
        base.Awake();

        Competitor_1.OnJoin += OnJoin;
        Competitor_1.OnUnJoin += OnUnJoin;
        Competitor_2.OnJoin += OnJoin;
        Competitor_2.OnUnJoin += OnUnJoin;

        Competitor_1.OnReady += OnReady;
        Competitor_1.OnUnready += OnUnready;
        Competitor_2.OnReady += OnReady;
        Competitor_2.OnUnready += OnUnready;

    }

    private void Start()
    {
        if (NextView == null) NextView = _viewMenuController.GetViewById(NextViewName);
    }

    private void OnJoin(SOPuller puller)
    {
        CharacterSelect.Instance.Add(puller);
    }

    private void OnUnJoin(SOPuller puller)
    {
        CharacterSelect.Instance.Remove(puller);
    }

    private void OnReady(SOPuller puller)
    {
        if (CharacterSelect.Instance.SelectedPullersPreview.Count >= 2)
        {
            CheckAllReady();
        }
        else
        {
            if (Competitor_1.Ready && !Competitor_2.Ready)
            {
                SOBotPuller botPuller = CharacterSelect.Instance.GetBot();
                Competitor_2.SetConfirmSetup(botPuller);
                CharacterSelect.Instance.Add(botPuller);
            }
            else if (!Competitor_1.Ready && Competitor_2.Ready)
            {
                SOBotPuller botPuller = CharacterSelect.Instance.GetBot();
                Competitor_1.SetConfirmSetup(CharacterSelect.Instance.GetBot());
                CharacterSelect.Instance.Add(botPuller);
            }

            CheckAllReady();
        }
    }

    private void CheckAllReady()
    {
        if (Competitor_1.Ready && Competitor_2.Ready)
        {
            if (NextView)
                _viewMenuController.PushView(NextView);
            else
                _viewMenuController.PushView(NextViewName);
        }
    }

    private void OnUnready(SOPuller puller)
    {
        if (!Competitor_1.Ready && Competitor_2.SelectedSetup is SOBotPuller)
        {
            Competitor_2.HoldLeaveFinished();
        }
        else if (!Competitor_2.Ready && Competitor_1.SelectedSetup is SOBotPuller)
        {
            Competitor_1.HoldLeaveFinished();
        }
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Competitor_1.SetJoinSetup(CharacterSelect.Instance.DefaultPlayers[0]);
        Competitor_2.SetJoinSetup(CharacterSelect.Instance.DefaultPlayers[1]);
    }

    protected override void OnExit()
    {
        Competitor_1.HoldLeaveFinished();
        Competitor_2.HoldLeaveFinished();

        if (Competitor_1.Joined) Competitor_1.HoldLeaveFinished();
        if (Competitor_2.Joined) Competitor_2.HoldLeaveFinished();

        Competitor_1.BlockJoin();
        Competitor_2.BlockJoin();
    }

    protected override void OnStacked()
    {
        Competitor_1.OnReadyBlock();
        Competitor_2.OnReadyBlock();
    }

    protected override void OnUnstacked()
    {
        Competitor_1.OnReadyUnblock();
        Competitor_2.OnReadyUnblock();

        Competitor_1.HoldLeaveFinished();
        Competitor_2.HoldLeaveFinished();
    }

    protected override void Update()
    {
        if (!_viewMenuController.IsViewOnTop(_viewBase)) return;

        if (Input.GetButtonDown("Cancel"))
        {
            if (Competitor_1.Ready || Competitor_2.Ready)
            {
                if (Competitor_1.Ready) Competitor_1.HoldLeaveFinished();
                if (Competitor_2.Ready) Competitor_2.HoldLeaveFinished();
                return;
            }

            if (Competitor_1.Joined || Competitor_2.Joined)
            {
                if (Competitor_1.Joined) Competitor_1.HoldLeaveFinished();
                if (Competitor_2.Joined) Competitor_2.HoldLeaveFinished();
                return;
            }

            if (CancelReturn) _viewMenuController.PopView();
        }
    }

    private void OnDestroy()
    {
        Competitor_1.OnJoin -= OnJoin;
        Competitor_1.OnUnJoin -= OnUnJoin;
        Competitor_2.OnJoin -= OnJoin;
        Competitor_2.OnUnJoin -= OnUnJoin;

        Competitor_1.OnReady -= OnReady;
        Competitor_1.OnUnready -= OnUnready;
        Competitor_2.OnReady -= OnReady;
        Competitor_2.OnUnready -= OnUnready;
    }
}
