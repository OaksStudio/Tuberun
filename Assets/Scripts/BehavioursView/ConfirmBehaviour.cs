using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OAKS.Utilities.Views;

public class ConfirmBehaviour : ViewBehaviour
{
    private Action OnYes;

    public void SetupConfirm(Action method)
    {
        OnYes = method;
        _viewMenuController.PushView(_viewBase);
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }

    public void Yes()
    {
        OnYes?.Invoke();
    }

}
