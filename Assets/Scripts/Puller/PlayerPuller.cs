using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuller : Puller
{
    private SOPlayerPuller _pullerInfo;
    private ControlMap _controlMap;

    [SerializeField] private List<InputActions> _validPullActions = new List<InputActions>() { InputActions.UP, InputActions.DOWN, InputActions.LEFT, InputActions.RIGHT, InputActions.NONE, InputActions.ANY };

    public override void Setup(int id, SOPuller puller)
    {
        base.Setup(id, puller);
        _pullerInfo = puller as SOPlayerPuller;
        _controlMap = _pullerInfo.ControlMap;
        SetupInputs();
        _pullerInfo.ControlMap.OnKeyDown += ProcessPull;
    }

    protected void SetupInputs()
    {
        PlayerInput playerInput = gameObject.AddComponent<PlayerInput>();
        playerInput.actions = _controlMap.Asset;

        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        playerInput.SwitchCurrentActionMap("Player");

        if (_pullerInfo.GamepadId >= 0)
        {
            playerInput.SwitchCurrentControlScheme(Gamepad.all[_pullerInfo.GamepadId]);
        }
        else
        {
            playerInput.SwitchCurrentControlScheme(Keyboard.current);
        }
        
        _controlMap.Setup(playerInput);
    }

    protected override void Initialize()
    {

    }

    private void ProcessPull(InputActions inputAction)
    {
        if (!_validPullActions.Contains(inputAction)) return;
        OnPull?.Invoke(inputAction, _pullerInfo.PullForce);
    }

    protected override void Process()
    {
        _pullerInfo.ControlMap.CheckKeyDown();
    }

    private void OnDestroy()
    {
        _pullerInfo.ControlMap.OnKeyDown -= ProcessPull;
        _pullerInfo.ControlMap.ReleaseSetup();
    }

}
