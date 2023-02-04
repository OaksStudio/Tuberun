using System.Collections;
using System.Collections.Generic;
using OAKS.Utilities.Views;
using UnityEngine;

[RequireComponent(typeof(ViewBase))]
public class ViewBehaviour : MonoBehaviour
{
    protected ViewBase _viewBase;

    protected virtual void Awake()
    {
        _viewBase = GetComponent<ViewBase>();

        _viewBase.OnAnimateEnter.AddListener(OnEnter);
        _viewBase.OnAnimateExit.AddListener(OnExit);

        _viewBase.OnPreAnimateEnter.AddListener(OnPreAnimateEnter);
        _viewBase.OnPreAnimateExit.AddListener(OnPreAnimateExit);

        _viewBase.OnStacked.AddListener(OnStacked);
        _viewBase.OnUnstacked.AddListener(OnUnstacked);
    }

    private void OnDestroy()
    {
        _viewBase.OnAnimateEnter.RemoveListener(OnEnter);
        _viewBase.OnAnimateExit.RemoveListener(OnExit);

        _viewBase.OnPreAnimateEnter.RemoveListener(OnPreAnimateEnter);
        _viewBase.OnPreAnimateExit.RemoveListener(OnPreAnimateExit);

        _viewBase.OnStacked.RemoveListener(OnStacked);
        _viewBase.OnUnstacked.RemoveListener(OnUnstacked);
    }

    protected virtual void OnEnter()
    {

    }

    protected virtual void OnExit()
    {

    }

    protected virtual void OnPreAnimateEnter()
    {

    }

    protected virtual void OnPreAnimateExit()
    {

    }

    protected virtual void OnStacked()
    {

    }

    protected virtual void OnUnstacked()
    {

    }


}
