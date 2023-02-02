using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

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
        [SerializeField] protected ViewReferences ViewReferences;
        [SerializeField] protected EventSystem EventSystem;

        protected void Start()
        {
            InitStart();
        }

        protected virtual void InitStart()
        {
            TryGetReferences();
        }

        public virtual void OnEnter()
        {
            if (!CanExecuteAnimation()) return;
            OnPreAnimateEnter?.Invoke();
            ExitSettings.TryAdjustParemeter(ViewReferences);
            EnterSettings.Animate(ViewReferences, AnimationKind.IN, DisableOnExit, OnAnimateEnter);
            SetSelected();
        }

        public virtual void SetSelected()
        {
            if (FocusItem == null) return;
            EventSystem.SetSelectedGameObject(FocusItem);
        }

        public virtual void OnExit()
        {
            if (!CanExecuteAnimation()) return;
            OnPreAnimateExit?.Invoke();
            EnterSettings.TryAdjustParemeter(ViewReferences);
            ExitSettings.Animate(ViewReferences, AnimationKind.OUT, DisableOnExit, OnAnimateExit);
        }

        protected void Reset()
        {
            TryGetReferences();
            TryGetName();
        }

        [Button]
        protected void TryGetReferences()
        {
            if (!ViewReferences.ViewRectTransform)
                TryGetComponent(out ViewReferences.ViewRectTransform);

            if (!ViewReferences.ViewGroup)
                TryGetComponent(out ViewReferences.ViewGroup);

            if (!EventSystem)
                EventSystem = EventSystem.current;
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
