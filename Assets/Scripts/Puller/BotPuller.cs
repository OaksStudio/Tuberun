using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPuller : Puller
{
    public SOBotPuller _pullerInfo;

    public override void Setup(SOPuller puller)
    {
        base.Setup(puller);
        _pullerInfo = puller as SOBotPuller;
    }

    protected override void Initialize()
    {

    }

    protected override void Process()
    {

    }
}
