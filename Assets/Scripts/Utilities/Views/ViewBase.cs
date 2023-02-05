using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

namespace OAKS.Utilities.Views
{
    public class ViewBase : MonoBehaviour
    {
        [InlineButton(nameof(TryGetName), "Get")]
        public string ViewId;
        public bool ExitOnNewViewPush = false;
        public bool RemoveOnNewViewPush = false;
        public GameObject FocusItem;
        [PropertyOrder(2)]
        [FoldoutGroup("Events")]
        public UnityEvent OnPreAnimateEnter, OnAnimateEnter, OnPreAnimateExit, OnAnimateExit, OnStacked, OnUnstacked;

        [Header("Animation")]
        [SerializeField] protected bool DisableOnExit;
        [SerializeField] public ViewAnimationSettingsSO EnterSettings;
        [SerializeField] public ViewAnimationSettingsSO ExitSettings;

        [Header("Setup")]
        [SerializeField] protected bool DisableButtonsOnStack = true;
        [Header("References")]
        [SerializeField] protected ViewReferences _viewReferences;
        [SerializeField] protected EventSystem _eventSystem;

        [ReadOnly, SerializeField] private List<Button> _buttons = new List<Button>();
        private RectTransform _rectTransform;
        private Coroutine _buttonProcedureCO;

        protected void Awake()
        {
            _buttons = GetComponentsInChildren<Button>().ToList();
            ButtonsActivation(false);
            InitStart();
            _rectTransform = transform.GetComponent<RectTransform>();
        }

        protected virtual void InitStart()
        {
            TryGetReferences();
        }

        public virtual void OnEnter()
        {
            if (!CanExecuteAnimation()) return;
            OnPreAnimateEnter?.Invoke();
            ExitSettings.TryAdjustParemeter(_viewReferences);
            EnterSettings.Animate(_viewReferences, AnimationKind.IN, DisableOnExit, OnAnimateEnter);
            SetSelected();
            ButtonsActivation(true);
        }

        public virtual void OnStack()
        {
            OnStacked?.Invoke();
            if (DisableButtonsOnStack) ButtonsActivation(false);
        }

        public virtual void OnUnstack()
        {
            OnUnstacked?.Invoke();
            if (DisableButtonsOnStack) ButtonsActivation(true);
        }

        public virtual void SetSelected()
        {
            if (FocusItem == null) return;

            _eventSystem.SetSelectedGameObject(FocusItem);
        }

        public virtual void OnExit()
        {
            if (!CanExecuteAnimation()) return;
            OnPreAnimateExit?.Invoke();
            EnterSettings.TryAdjustParemeter(_viewReferences);
            ExitSettings.Animate(_viewReferences, AnimationKind.OUT, DisableOnExit, OnAnimateExit);
            ButtonsActivation(false);
        }

        private void ButtonsActivation(bool value)
        {
            if (_buttonProcedureCO != null)
            {
                StopCoroutine(_buttonProcedureCO);
                _buttonProcedureCO = null;
            }
            _buttonProcedureCO = StartCoroutine(ButtonsActivationCO(value));
        }

        private IEnumerator ButtonsActivationCO(bool value)
        {
            if (value)
            {
                Activation(value);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
            Activation(value);
            _buttonProcedureCO = null;
        }

        private void Activation(bool value)
        {
            _buttons.ForEach(b =>
            {
                b.interactable = value;
                b.enabled = value;
            });
        }

        protected void Reset()
        {
            TryGetReferences();
            TryGetName();
        }

        [Button]
        protected void TryGetReferences()
        {
            if (!_viewReferences.ViewRectTransform)
                TryGetComponent(out _viewReferences.ViewRectTransform);

            if (!_viewReferences.ViewGroup)
                TryGetComponent(out _viewReferences.ViewGroup);

            if (!_eventSystem)
                _eventSystem = EventSystem.current;
        }

        protected bool CanExecuteAnimation()
        {
            return ExitSettings && EnterSettings;
        }

        protected void TryGetName()
        {
            ViewId = gameObject.name;
        }

        internal void PushView(object view)
        {
            throw new NotImplementedException();
        }
    }
}
