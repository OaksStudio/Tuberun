using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OAKS.Utilities.Views
{
    public class ButtonBase : MonoBehaviour
    {
        [SerializeField] protected float timeToUseAgain = .1f;
        [SerializeField] protected Button Button;

        protected virtual void Awake()
        {
            InitAwake();
        }

        protected virtual void InitAwake()
        {
            GetButton();
            Button.onClick.AddListener(OnClickButton);
        }

        protected virtual void GetButton()
        {
            if (Button) return;
            TryGetComponent(out Button);
        }

        protected virtual void OnClickButton()
        {
            Invoke(nameof(ResetUsage), timeToUseAgain);
            Button.interactable = false;
        }

        protected virtual void ResetUsage()
        {
            Button.interactable = true;
        }

        protected virtual void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClickButton);
        }
    }
}