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

namespace MonsterWhaser.Utilities.Views
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
        public UnityEvent OnPreAnimateEnter, OnAnimateEnter, OnPreAnimateExit, OnAnimateExit;

        [Header("Animation")]
        [SerializeField] protected bool DisableOnExit;
        [SerializeField] protected ViewAnimationSettingsSO EnterSettings;
        [SerializeField] protected ViewAnimationSettingsSO ExitSettings;

        [Header("References")]
        [SerializeField] protected ViewReferences _viewReferences;
        [SerializeField] protected EventSystem _eventSystem;

        private List<Button> _buttons = new List<Button>();

        protected void Awake()
        {
            InitStart();
            _buttons.ForEach(b => b.interactable = false);
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
            _buttons.ForEach(b => b.interactable = true);
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
            _buttons.ForEach(b => b.interactable = false);
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

            _buttons = GetComponentsInChildren<Button>().ToList();
        }

        protected bool CanExecuteAnimation()
        {
            return ExitSettings && EnterSettings;
        }

        protected void TryGetName()
        {
            ViewId = gameObject.name;
        }
    }
}
