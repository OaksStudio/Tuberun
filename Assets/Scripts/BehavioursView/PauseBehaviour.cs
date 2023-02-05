using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehaviour : ViewBehaviour
{
    private Coroutine _pauseCO;

    protected override void OnEnter()
    {
        base.OnEnter();
        PauseManager.Instance.Pause();

        if (_pauseCO != null)
        {
            StopCoroutine(_pauseCO);
            _pauseCO = null;
        }
        _pauseCO = StartCoroutine(PauseCO());
    }

    protected override void OnExit()
    {
        base.OnExit();
        PauseManager.Instance.UnPause();
        if (_pauseCO != null)
        {
            StopCoroutine(_pauseCO);
            _pauseCO = null;
        }
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
