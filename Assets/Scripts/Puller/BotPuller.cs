using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPuller : Puller
{
    public SOBotPuller _pullerInfo;
    private TuberRow _tuberRow;

    private float _delayTimeStamp;

    private float _timeStamp;

    public override void Setup(int id, SOPuller puller)
    {
        base.Setup(id, puller);
        _pullerInfo = puller as SOBotPuller;
        _tuberRow = GameManager.Instance.GameMode.TuberRows[id];

        _delayTimeStamp = Time.time + _pullerInfo.DelayToStart;
    }

    protected override void Initialize()
    {

    }

    protected override void Process()
    {
        if (_delayTimeStamp > Time.time) return;

        if (_timeStamp > Time.time) return;
        if (!_tuberRow.GetTuber()) return;

        InputActions correctDirection = _tuberRow.GetTuber().GetCorrectDirection();

        float random = Random.Range(0, _pullerInfo.MaxAccuracy);
        InputActions randomDirection = random < _pullerInfo.Accuracy ? correctDirection : InputActions.NONE;

        _timeStamp = Time.time + Random.Range(_pullerInfo.CooldownRange.x, _pullerInfo.CooldownRange.y);
        ProcessPull(randomDirection);
    }

    private void ProcessPull(InputActions direction)
    {
        OnPull?.Invoke(direction, _pullerInfo.PullForce);
    }
}
