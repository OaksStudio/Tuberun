using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Level;
using UnityEngine;

public class LoseBehaviour : ViewBehaviour
{
    public Loader RetryLoader;

    private void Start()
    {
        GameManager.Instance.OnLost += Losted;
    }

    private void Losted()
    {
        RetryLoader.scene = GameManager.Instance.GameMode.GameModeInfo.GameModeScene;

        _viewMenuController.PushView(_viewBase);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLost -= Losted;
    }
}
