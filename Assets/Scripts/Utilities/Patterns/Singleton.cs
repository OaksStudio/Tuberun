using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jozi.Utilities.Patterns
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = GetComponent<T>();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}