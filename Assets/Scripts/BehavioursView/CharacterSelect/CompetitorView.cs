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

    public bool Ready => _ready;
    public bool Joined => _joined;
    [SerializeField, ReadOnly] private bool _joined, _ready;
    private List<Map> _maps = new List<Map>();

    public SOPuller SelectedSetup => _selectedSetup;
    private SOPuller _selectedSetup;

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
        _maps.AddRange(defaultSetup.ControlMap.KeysMapped.FindAll(k => k.InputAction == InputActions.RETURN));

        JoinImages[0].Setup(_maps[0], playerPuller.ControlMap.Asset);
        JoinImages[1].Setup(_maps[1], playerPuller.ControlMap.Asset);

        JoinImages[0].Activate(true);
        JoinImages[1].Activate(true);

        ConfirmHolder.gameObject.SetActive(false);
        ConfirmBack.gameObject.SetActive(false);

        CompetitorName.text = "";
    }

    public void BlockJoin()
    {
        JoinImages[0].Activate(false);
        JoinImages[1].Activate(false);
    }

    public void UnblockJoin()
    {
        JoinImages[0].Activate(false);
        JoinImages[1].Activate(false);
    }

    private void JoinedProcedure()
    {
        if (_joined) return;
        _joined = true;

        SetConfirmSetup(defaultSetup);
        OnJoin?.Invoke(defaultSetup);

        JoinImages[0].Activate(false);
        JoinImages[1].Activate(false);
    }

    private void RemoveJoinSetup()
    {
        JoinHolder.gameObject.SetActive(false);
    }
    #endregion

    #region  READY
    [Button]
    public void SetConfirmSetup(SOPuller puller)
    {
        RemoveJoinSetup();

        CompetitorName.text = puller.PullerName;
        CompetitorName.color = puller.PullerColor;

        _selectedSetup = puller;

        Debug.Log(JoinImages[0].PlayerInput.currentControlScheme);

        if (puller is SOBotPuller botPuller)
        {
            ConfirmHolder.gameObject.SetActive(false);

            ConfirmReady.Activate(false);
            HoldLeaveJoin.Activate(false);

            _ready = true;
            _joined = true;

            OnReady?.Invoke(puller);
        }
        else if (puller is SOPlayerPuller playerPuller)
        {
            ConfirmHolder.gameObject.SetActive(true);

            ConfirmReady.Setup(_maps[0], playerPuller.ControlMap.Asset);
            HoldLeaveJoin.Setup(_maps[1], playerPuller.ControlMap.Asset);

            ConfirmReady.Activate(true);
            HoldLeaveJoin.Activate(true);
        }
    }


    [Button]
    private void RemoveConfirmSetup(SOPuller puller)
    {
        ConfirmHolder.gameObject.SetActive(false);
        CompetitorName.text = "";
        ConfirmReady.Activate(false);
        HoldLeaveJoin.Activate(false);
    }

    private void ReadyProcedure()
    {
        if (!_joined || _ready) return;
        _ready = true;

        ConfirmReady.Activate(false);
        ConfirmReadyHolder.gameObject.SetActive(false);
        ConfirmBack.gameObject.SetActive(true);

        OnReady?.Invoke(_selectedSetup);
    }

    public void OnReadyBlock()
    {
        HoldLeaveJoin.Activate(false);
    }

    public void OnReadyUnblock()
    {
        HoldLeaveJoin.Activate(false);
    }

    #endregion

    #region LEAVE
    [Button]
    public void HoldLeaveFinished()
    {
        if (_joined && !_ready)
        {
            _joined = false;
            OnUnJoin?.Invoke(_selectedSetup);
            RemoveConfirmSetup(_selectedSetup);
            SetJoinSetup(defaultSetup);
            _selectedSetup = null;
            return;
        }

        if (_ready)
        {
            _ready = false;
            if (_selectedSetup is SOBotPuller botPuller)
            {
                RemoveConfirmSetup(_selectedSetup);
                SetJoinSetup(defaultSetup);
                _joined = false;
                _selectedSetup = null;
            }
            else if (_selectedSetup is SOPlayerPuller playerPuller)
            {
                ConfirmReady.Activate(true);
                ConfirmReadyHolder.gameObject.SetActive(true);
                HoldLeaveJoin.Reset();
                SetConfirmSetup(_selectedSetup);
            }
            OnUnready?.Invoke(_selectedSetup);
            ConfirmBack.gameObject.SetActive(false);
            return;
        }
    }

    #endregion


}
