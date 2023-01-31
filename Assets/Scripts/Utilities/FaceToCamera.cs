using UnityEngine;

namespace Jozi.Utilities
{
    public class FaceToCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}