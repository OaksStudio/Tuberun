using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CharacterSelectBehaviour : ViewBehaviour
{
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

    private void OnJoin(SOPuller puller)
    {
        Debug.Log($"{puller.PullerName} Joined");
        CharacterSelect.Instance.Add(puller);
    }

    private void OnUnJoin(SOPuller puller)
    {
        Debug.Log($"{puller.PullerName} Unjoined");
        CharacterSelect.Instance.Remove(puller);
    }

    private void OnReady(SOPuller puller)
    {
        Debug.Log($"{puller.PullerName} is Ready!");
        if (CharacterSelect.Instance.SelectedPullersPreview.Count >= 2)
        {
            if (Competitor_1.Ready && Competitor_2.Ready)
            {
                Debug.Log("Call next View!");
            }
        }
        else
        {
            if (Competitor_1.Ready && !Competitor_2.Ready)
            {
                Competitor_2.SetConfirmSetup(CharacterSelect.Instance.GetBot());
            }
            else if (!Competitor_1.Ready && Competitor_2.Ready)
            {
                Competitor_1.SetConfirmSetup(CharacterSelect.Instance.GetBot());
            }
        }
    }

    private void OnUnready(SOPuller puller)
    {
        if (puller) Debug.Log($"{puller.PullerName} become unready!");
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
