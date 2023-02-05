using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OAKS.Utilities.Views
{
    public class ShowViewScreen : MonoBehaviour
    {
        [SerializeField] protected ViewMenuController ViewMenu;
        [SerializeField] protected string ViewName;
        [SerializeField] private ViewBase View;

        protected virtual void Awake()
        {
            TryGetComponent(out View);
        }

        public void ShowScreen()
        {
            if (View)
                ViewMenu.PushView(View);
            else
                ViewMenu.PushView(ViewName);
        }
    }
}