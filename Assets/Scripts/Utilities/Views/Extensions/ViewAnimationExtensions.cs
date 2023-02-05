using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace OAKS.Utilities.Views
{
    public static class ViewAnimationExtensions
    {
        public static bool ConvertToBool(this AnimationKind kind)
        {
            return kind == AnimationKind.IN ? true : false;
        }
    }

}
