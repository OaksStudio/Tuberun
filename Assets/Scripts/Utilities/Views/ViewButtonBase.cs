using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace MonsterWhaser.Utilities.Views
{
    public class ViewButtonBase : ButtonBase
    {
        public enum ButtonMode { Default, Back }
        public ButtonMode ButtonType;

        [Header("References")]
        [InlineButton(nameof(TryGetView), "Try Get")]
        [HideIf("@ButtonType==ButtonMode.Back || View!=null")]
        [SerializeField] private string ViewToCall;

        [InlineButton(nameof(TryGetView), "Try Get")]
        [HideIf(nameof(ButtonType), ButtonMode.Back)]
        [SerializeField] private ViewBase View;

        [InlineButton(nameof(GetMenuController), "Get")]
        [SerializeField] private ViewMenuController controller;

        protected override void Awake()
        {
            base.Awake();
            if (!controller)
                controller = GetComponentInParent<ViewMenuController>();
        }

        private void Start()
        {
            Reset();
        }

        protected override void OnClickButton()
        {
            base.OnClickButton();
            if (!controller) return;

            if (ButtonType == ButtonMode.Default)
            {
                if (View)
                    controller.PushView(View);
                else
                    controller.PushView(ViewToCall);
            }
            else
            {
                controller.PopView();
            }
        }

        private void GetMenuController()
        {
            if (!controller) controller = GetComponentInParent<ViewMenuController>();
        }

        private void Reset()
        {
            GetMenuController();
            GetButton();
            TryGetView();
        }

        private void TryGetView()
        {
            GetMenuController();

            ViewBase viewBase = null;
            if (!string.IsNullOrEmpty(ViewToCall))
            {
                viewBase = controller.GetViewById(ViewToCall);
            }

            if (!viewBase) viewBase = GetComponentInParent<ViewBase>();

            if (viewBase && !View)
            {
                View = viewBase;
            }

            if (View)
            {
                ViewToCall = View.ViewId;
            }

            if (!string.IsNullOrEmpty(ViewToCall))
                gameObject.name = $"Button - {ViewToCall.Replace("View - ", "")}";
        }
    }

}
