using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerPuller", menuName = "Pullers/PlayerPuller", order = 0)]
public class SOPlayerPuller : SOPuller
{
    public ControlMap ControlMap => _controlMap;
    public int GamepadId => _gamepadId;
    [SerializeField] private int _gamepadId = -1;
    [SerializeField] private ControlMap _controlMap;

    public void ResetGamepad()
    {
        _gamepadId = -1;
    }
}

