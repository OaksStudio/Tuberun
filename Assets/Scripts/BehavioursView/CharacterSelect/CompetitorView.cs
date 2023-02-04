using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompetitorView : MonoBehaviour
{
    [Header("Join Setup")]
    public SOPlayerPuller defaultSetup;
    public RectTransform JoinHolder;
    public List<PressButton> JoinImages = new List<PressButton>();

    [Header("Joined Setup")]
    public TextMeshProUGUI CompetitorName;
    public RectTransform ConfirmHolder;
    public RectTransform ConfirmReadyHolder;
    public PressButton ConfirmReady;
    public HoldButton HoldLeaveJoin;
    public Image ConfirmBack;

    public Action<SOPuller> OnJoin, OnReady, OnUnready, OnUnJoin;

    private bool _joined, _ready;
    private List<ControlMap.Map> _maps = new List<ControlMap.Map>();

    private SOPuller selectedSetup;

    private void Awake()
    {
        JoinImages[0].OnButtonDown += JoinedProcedure;
        JoinImages[1].OnButtonDown += JoinedProcedure;

        HoldLeaveJoin.OnComplete += HoldLeaveFinished;
        ConfirmReady.OnButtonDown += ReadyProcedure;

        ConfirmBack.gameObject.SetActive(false);
    }

    #region JOIN
    [Button]
    public void SetJoinSetup(SOPlayerPuller playerPuller)
    {
        defaultSetup = playerPuller;
        HoldLeaveJoin.Activate(false);

        JoinHolder.gameObject.SetActive(true);
        _maps = defaultSetup.ControlMap.KeysMapped.FindAll(k => k.InputAction == InputActions.CONFIRM);

        JoinImages[0].Setup(_maps[0]);
        JoinImages[1].Setup(_maps[1]);

        JoinImages[0].Activate(true);
        JoinImages[1].Activate(true);

        ConfirmHolder.gameObject.SetActive(false);
        ConfirmBack.gameObject.SetActive(false);
    }

    private void JoinedProcedure()
    {
        if (_joined) return;

        SetConfirmSetup(defaultSetup);
        OnJoin?.Invoke(defaultSetup);
        _joined = true;

        JoinImages[0].Activate(false);
        JoinImages[1].Activate(false);
    }

    public void RemoveJoinSetup()
    {
        JoinHolder.gameObject.SetActive(false);
    }
    #endregion

    #region  READY
    public void SetConfirmSetup(SOPuller puller)
    {
        RemoveJoinSetup();

        CompetitorName.text = puller.name;
        CompetitorName.color = puller.PullerColor;

        selectedSetup = puller;

        if (puller is SOBotPuller botPuller)
        {
            ConfirmHolder.gameObject.SetActive(false);

            ConfirmReady.Activate(false);
            HoldLeaveJoin.Activate(false);

            OnReady?.Invoke(puller);
        }
        else if (puller is SOPlayerPuller playerPuller)
        {
            ConfirmHolder.gameObject.SetActive(true);

            ConfirmReady.Setup(_maps[0]);
            HoldLeaveJoin.Setup(_maps[1]);

            ConfirmReady.Activate(true);
            HoldLeaveJoin.Activate(true);
        }
    }


    [Button]
    public void RemoveConfirmSetup(SOPuller puller)
    {
        ConfirmHolder.gameObject.SetActive(false);
        CompetitorName.text = "";
        ConfirmReady.Activate(false);
        HoldLeaveJoin.Activate(false);
        HoldLeaveJoin.Reset();
    }

    private void ReadyProcedure()
    {
        ConfirmReady.Activate(false);
        ConfirmReadyHolder.gameObject.SetActive(false);
        ConfirmBack.gameObject.SetActive(true);

        OnReady?.Invoke(selectedSetup);
        _ready = true;
    }


    #endregion

    #region LEAVE
    [Button]
    private void HoldLeaveFinished()
    {
        if (_joined)
        {
            OnUnJoin?.Invoke(selectedSetup);
            _joined = false;
            RemoveConfirmSetup(selectedSetup);
            selectedSetup = null;
            return;
        }

        if (_ready)
        {
            if (selectedSetup is SOBotPuller botPuller)
            {
                RemoveConfirmSetup(selectedSetup);
            }
            else if (selectedSetup is SOPlayerPuller playerPuller)
            {
                ConfirmReady.Activate(true);
                ConfirmReadyHolder.gameObject.SetActive(false);
            }
            OnUnready?.Invoke(selectedSetup);
            ConfirmBack.gameObject.SetActive(false);
            selectedSetup = null;
            return;
        }
    }

    #endregion


}
