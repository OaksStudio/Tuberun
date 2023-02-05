using System.Collections;
using System.Collections.Generic;
using OAKS.Utilities.Views;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ViewBase))]
public class ViewBehaviour : MonoBehaviour
{
    [Header("Setup")]
    public bool CancelReturn = true;
    public UnityEvent CancelEvent;

    [SerializeField] protected ViewMenuController _viewMenuController;
    protected ViewBase _viewBase;

    protected virtual void Awake()
    {
        if (!_viewMenuController) _viewMenuController = GetComponentInParent<ViewMenuController>();
        _viewBase = GetComponent<ViewBase>();

        _viewBase.OnAnimateEnter.AddListener(OnEndEnterAnimation);
        _viewBase.OnAnimateExit.AddListener(OnEndExitAnimation);

        _viewBase.OnPreAnimateEnter.AddListener(OnEnter);
        _viewBase.OnPreAnimateExit.AddListener(OnExit);

        _viewBase.OnStacked.AddListener(OnStacked);
        _viewBase.OnUnstacked.AddListener(OnUnstacked);
    }

    private void OnDestroy()
    {
        _viewBase.OnAnimateEnter.RemoveListener(OnEndEnterAnimation);
        _viewBase.OnAnimateExit.RemoveListener(OnEndExitAnimation);

        _viewBase.OnPreAnimateEnter.RemoveListener(OnEnter);
        _viewBase.OnPreAnimateExit.RemoveListener(OnExit);

        _viewBase.OnStacked.RemoveListener(OnStacked);
        _viewBase.OnUnstacked.RemoveListener(OnUnstacked);
    }

    protected virtual void OnEndEnterAnimation()
    {

    }

    protected virtual void OnEndExitAnimation()
    {

    }

    protected virtual void OnEnter()
    {

    }

    protected virtual void OnExit()
    {

    }

    protected virtual void OnStacked()
    {

    }

    protected virtual void OnUnstacked()
    {

    }


    protected virtual void Update()
    {
        if (!CancelReturn) return;
        if (!_viewMenuController.IsViewOnTop(_viewBase)) return;
        if (Input.GetButtonDown("Cancel"))
        {
            _viewMenuController.PopView();
        }
    }


}
