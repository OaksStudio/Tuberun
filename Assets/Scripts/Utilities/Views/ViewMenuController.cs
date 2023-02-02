using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace MonsterWhaser.Utilities.Views
{
    [DisallowMultipleComponent]
    public class ViewMenuController : MonoBehaviour
    {
        [SerializeField] private ViewBase InitialView;
        [SerializeField] private Canvas RootCanvas;
        [InlineButton(nameof(GetAllViews), "Get")]
        [SerializeField] private List<ViewBase> Views = new List<ViewBase>();
        [ShowInInspector]
        private Stack<ViewBase> _viewsStack = new Stack<ViewBase>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            GetAllViews();
            if (InitialView == null) return;
            PushView(InitialView);
        }

        public void PushView(ViewBase view)
        {
            if (IsViewOnTop(view)) return;
            view.OnEnter();

            if (_viewsStack.Count > 0)
            {
                var currentPage = _viewsStack.Peek();

                if (currentPage.RemoveOnNewViewPush)
                    PopView();
                else if (currentPage.ExitOnNewViewPush)
                    currentPage.OnExit();
            }

            _viewsStack.Push(view);
        }

        public void PushView(string viewId)
        {
            if (!IsViewOnStack(viewId)) return;
            PushView(GetViewById(viewId));
        }

        public void PopView()
        {
            if (_viewsStack.Count == 0) return;

            var popPage = _viewsStack.Pop();
            popPage.OnExit();
          
            if (_viewsStack.Count == 0) return;
       
            var currentPage = _viewsStack.Peek();

            if (currentPage.ExitOnNewViewPush)
                currentPage.OnEnter();
            else
                currentPage.SetSelected();

        }

        private void OnReturn()
        {
            if (!RootCanvas.enabled || !RootCanvas.gameObject.activeInHierarchy
            || _viewsStack.Count == 0) return;

            PopView();
        }

        public void PopAllViews()
        {
            for (int i = 1; i < _viewsStack.Count; i++)
            {
                PopAllViews();
            }
        }

        public bool IsViewOnStack(ViewBase view)
        {
            return _viewsStack.Contains(view);
        }

        public bool IsViewOnTop(ViewBase view)
        {
            return _viewsStack.Count > 0 && _viewsStack.Peek().Equals(view);
        }

        public bool IsViewOnStack(string viewId)
        {
            return Views.Count > 0 && Views.Exists(v => v.ViewId == viewId);
        }

        public bool IsViewOnTop(string viewId)
        {
            return IsViewOnStack(viewId) && _viewsStack.Peek().Equals(GetViewById(viewId));
        }

        private ViewBase GetViewById(string viewId)
        {
            return Views.Find(v => v.ViewId == viewId);
        }

        private void Reset()
        {
            TryGetComponent(out RootCanvas);
            GetAllViews();
        }

        private void GetAllViews()
        {
            Views = GetComponentsInChildren<ViewBase>().ToList();
        }
    }

}
