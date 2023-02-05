using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OAKS.Utilities.Views
{
    [CreateAssetMenu(menuName = "Views/Animations Settings/Scale", fileName = "ANIM_SCL_Default_SO")]
    public class ViewScaleAnimationSettingsSO : ViewAnimationSettingsSO
    {
        public override void Animate(ViewReferences view, AnimationKind kind, bool disableOnExit, UnityEvent onEnd = null)
        {
            Vector3 targetScale = kind == AnimationKind.IN ? Vector3.one : Vector3.zero;
            Vector3 startScale = kind == AnimationKind.IN ? Vector3.zero : Vector3.one;

            view.ViewRectTransform.localScale = startScale;

            if (disableOnExit)
                view.ViewRectTransform.gameObject.SetActive(true);

            view.ViewRectTransform.DOScale(targetScale, Speed).SetEase(Ease).SetUpdate(true).OnComplete(() =>
            {
                view.ViewRectTransform.localScale = targetScale;
                onEnd?.Invoke();
                if (disableOnExit)
                    view.ViewRectTransform.gameObject.SetActive(kind.ConvertToBool());
            });
        }

        public override void TryAdjustParemeter(ViewReferences view)
        {
            base.TryAdjustParemeter(view);
            if (view.ViewRectTransform.localScale == Vector3.one) return;
            view.ViewRectTransform.localScale = Vector3.one;
        }
    }
}