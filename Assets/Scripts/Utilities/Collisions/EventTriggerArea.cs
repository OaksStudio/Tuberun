using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jozi.Utilities.Collisions
{
    public class EventTriggerArea : MonoBehaviour
    {
        public LayerMask LayerToDetect;

        [Header("On Trigger Enter")]
        public UnityEvent OnEnter;
        public UnityEvent OnStay;
        public UnityEvent OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (LayerContains(other.transform.gameObject.layer, LayerToDetect))
            {
                OnEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (LayerContains(other.transform.gameObject.layer, LayerToDetect))
            {
                OnStay?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (LayerContains(other.transform.gameObject.layer, LayerToDetect))
            {
                OnExit?.Invoke();
            }
        }

        private bool LayerContains(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}