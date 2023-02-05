using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OAKS.Utilities.Views;

public class ConfirmBehaviour : ViewBehaviour
{
    private Action OnYes;
    private Coroutine _pauseCO;


    public void SetupConfirm(Action method)
    {
        OnYes = method;
        _viewMenuController.PushView(_viewBase);
    }

    protected override void OnEnter()
    {
        base.OnEnter();


        if (!PauseManager.Instance) return;
        if (PauseManager.Instance.IsPaused)
        {
            if (_pauseCO != null)
            {
                StopCoroutine(_pauseCO);
                _pauseCO = null;
            }
            _pauseCO = StartCoroutine(PauseCO());
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        if (_pauseCO != null)
        {
            StopCoroutine(_pauseCO);
            _pauseCO = null;
        }
    }

    public void Yes()
    {
        OnYes?.Invoke();
    }

    IEnumerator PauseCO()
    {
        while (PauseManager.Instance.IsPaused)
        {
            yield return null;
            if (!CancelReturn) continue;
            if (!_viewMenuController.IsViewOnTop(_viewBase)) continue;
            if (Input.GetButtonDown("Cancel"))
            {
                _viewMenuController.PopView();
            }
        }
        yield return null;
        _pauseCO = null;
    }

}
