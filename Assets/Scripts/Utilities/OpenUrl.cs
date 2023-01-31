using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jozi.Utilities
{
    public class OpenUrl : MonoBehaviour
    {
        public void Open(string value)
        {
            Application.OpenURL(value);
        }
    }
}