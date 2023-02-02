using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace MonsterWhaser.Utilities.Views
{
    [CreateAssetMenu(menuName = "Views/Animations Settings/Slide", fileName = "ANIM_SLD_Default_SO")]
    public class ViewSlideAnimationSettingsSO : ViewAnimationSettingsSO
    {
        public Direction Direction = Direction.UP;
        public override void Animate(ViewReferences view, AnimationKind kind, bool disableOnExit, UnityEvent onEnd = null)
        {
            Vector2 startPosition = Vector2.zero;

            int multiplier = kind == AnimationKind.IN ? 1 : -1;

            startPosition.x = Direction == Direction.RIGHT ? -view.ViewRectTransform.rect.width * multiplier :
            Direction == Direction.LEFT ? view.ViewRectTransform.rect.width * multiplier : 0;

            startPosition.y = Direction == Direction.UP ? -view.ViewRectTransform.rect.height * multiplier :
            Direction == Direction.DOWN ? view.ViewRectTransform.rect.height * multiplier : 0;

            view.ViewRectTransform.anchoredPosition = kind == AnimationKind.IN ? startPosition : Vector2.zero;

            var target = kind == AnimationKind.IN ? Vector2.zero : startPosition;

            if (disableOnExit)
                view.ViewRectTransform.gameObject.SetActive(true);

            view.ViewRectTransform.DOAnchorPos(target, Speed).SetEase(Ease).SetUpdate(true).OnComplete(() =>
            {
                view.ViewRectTransform.anchoredPosition = target;
                onEnd?.Invoke();

                if (disableOnExit)
                    view.ViewRectTransform.gameObject.SetActive(kind.ConvertToBool());
            });
        }
    }
}