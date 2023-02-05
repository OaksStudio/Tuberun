using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerPuller", menuName = "Pullers/PlayerPuller", order = 0)]
public class SOPlayerPuller : SOPuller
{
    public ControlMap ControlMap => _controlMap;
    [SerializeField] private ControlMap _controlMap;
}

