using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jozi.Utilities.Events
{
    public class EventCaller : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent FirstCaller;

        [SerializeField]
        private UnityEvent SecondCaller;

        public void FirstCall()
        {
            FirstCaller?.Invoke();
        }

        public void SecondCall()
        {
            SecondCaller?.Invoke();
        }
    }
}