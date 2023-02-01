using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuller : Puller
{
    public SOPlayerPuller PullerInfo;

    private ControlMap _controlMap;

    protected override void Setup(TuberRow tuberRow)
    {
        base.Setup(tuberRow);
    }

    protected override void Initialize()
    {
        PullerInfo.ControlMap.OnKeyDown += ProcessPull;
        _controlMap = PullerInfo.ControlMap;
    }

    private void ProcessPull(Direction direction)
    {
        OnPull?.Invoke(direction, PullerInfo.PullForce);
    }

    protected override void Process()
    {
        PullerInfo.ControlMap.CheckKeyDown();
    }

    private void OnDestroy()
    {
        PullerInfo.ControlMap.OnKeyDown -= ProcessPull;
    }

}
