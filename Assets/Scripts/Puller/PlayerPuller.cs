using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuller : Puller
{
    private SOPlayerPuller _pullerInfo;
    private ControlMap _controlMap;

    public override void Setup(int id, SOPuller puller)
    {
        base.Setup(id, puller);
        _pullerInfo = puller as SOPlayerPuller;
        _controlMap = _pullerInfo.ControlMap;
        _pullerInfo.ControlMap.OnKeyDown += ProcessPull;
    }

    protected override void Initialize()
    {

    }

    private void ProcessPull(Direction direction)
    {
        OnPull?.Invoke(direction, _pullerInfo.PullForce);
    }

    protected override void Process()
    {
        _pullerInfo.ControlMap.CheckKeyDown();
    }

    private void OnDestroy()
    {
        _pullerInfo.ControlMap.OnKeyDown -= ProcessPull;
    }

}
