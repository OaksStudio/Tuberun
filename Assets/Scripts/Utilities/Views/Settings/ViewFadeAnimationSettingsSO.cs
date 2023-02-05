using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace OAKS.Utilities.Views
{
    [CreateAssetMenu(menuName = "Views/Animations Settings/Fade", fileName = "ANIM_FAD_Default_SO")]
    public class ViewFadeAnimationSettingsSO : ViewAnimationSettingsSO
    {
        public override void Animate(ViewReferences view, AnimationKind kind, bool disableOnExit, UnityEvent onEnd = null)
        {
            bool active = kind.ConvertToBool();
            float startAlpha = kind == AnimationKind.IN ? 0 : 1;
            float targetAlpha = kind == AnimationKind.IN ? 1 : 0;

            view.ViewGroup.alpha = startAlpha;

            if (active)
            {
                view.ViewGroup.interactable = active;
                view.ViewGroup.blocksRaycasts = active;
            }

            if (disableOnExit)
                view.ViewGroup.gameObject.SetActive(true);

            view.ViewGroup.DOFade(targetAlpha, Speed).SetEase(Ease).SetUpdate(true).OnComplete(() =>
            {
                view.ViewGroup.alpha = targetAlpha;
                view.ViewGroup.interactable = active;
                view.ViewGroup.blocksRaycasts = active;

                if (disableOnExit)
                    view.ViewGroup.gameObject.SetActive(kind.ConvertToBool());

                onEnd?.Invoke();
            });
        }

        public override void TryAdjustParemeter(ViewReferences view)
        {
            base.TryAdjustParemeter(view);
            if (view.ViewGroup.alpha == 1) return;
            view.ViewGroup.alpha = 1;
            view.ViewGroup.blocksRaycasts = true;
            view.ViewGroup.interactable = true;
        }
    }
}