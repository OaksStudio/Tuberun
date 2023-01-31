using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jozi.Utilities.Patterns
{
    [CreateAssetMenu(fileName = "SO_ObserverEvent", menuName = "Observer/ObserverEvent", order = 1)]
    public class ObserverEvent : ScriptableObject
    {
        public string Tag = "Default";
        [TextArea]
        public string Description;

        public bool IsValid(ObserverEvent observerEvent)
        {
            if (observerEvent.Tag.Equals(Tag))
            {
                return true;
            }
            return false;
        }
    }
}