using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jozi.Pools
{
    public class PoolExample : MonoBehaviour
    {
        [Header("Setup")]
        public int PoolSize = 5;
        public GameObject Prefab;

        [Header("Actions keys")]
        public KeyCode GetAction = KeyCode.Space;
        public KeyCode ReleaseAction = KeyCode.R;

        [Header("Ranges")]
        public Vector2 XRange = new Vector2(1, 10);
        public Vector2 YRange = new Vector2(1, 10);

        private Queue<GameObject> lastObjects = new Queue<GameObject>();

        private void Start()
        {
            PoolCommand.Warm(Prefab, PoolSize);
        }

        private void Update()
        {
            if (Input.GetKeyDown(GetAction))
            {
                GetNewObject();
            }

            if (Input.GetKeyDown(ReleaseAction))
            {
                ReleaseObject();
            }
        }

        private void GetNewObject()
        {
            var newObject = PoolCommand.GetObject(Prefab, transform.position + Vector3.up * Random.Range(YRange.x, YRange.y) + Vector3.right * Random.Range(XRange.x, XRange.y), Quaternion.identity);
            if (!newObject)
                return;
            lastObjects.Enqueue(newObject);
        }

        private void ReleaseObject()
        {
            if (lastObjects.Count <= 0)
                return;
            PoolCommand.ReleaseObject(lastObjects.Dequeue());
        }
    }
}