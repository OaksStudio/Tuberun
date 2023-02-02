using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace MonsterWhaser.Utilities.Views
{
    public enum Direction { NONE, UP, RIGHT, DOWN, LEFT }
    public enum AnimationKind { IN, OUT }

    [System.Serializable]
    public struct ViewReferences
    {
        public RectTransform ViewRectTransform;
        public CanvasGroup ViewGroup;
    }
    public class ViewAnimationSettingsSO : ScriptableObject
    {
        public Ease Ease = Ease.Linear;
        public float Speed = .1f;

        public virtual void Animate(ViewReferences view, AnimationKind kind, bool disableOnExit, UnityEvent onEnd = null)
        {

        }

        public virtual void TryAdjustParemeter(ViewReferences view)
        {
             if (view.ViewRectTransform.anchoredPosition == Vector2.zero) return;
            view.ViewRectTransform.anchoredPosition = Vector2.zero;
        }
    }

}