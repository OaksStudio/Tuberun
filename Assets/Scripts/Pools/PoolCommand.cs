using System.Collections.Generic;
using UnityEngine;
using Jozi.Utilities.Patterns;

namespace Jozi.Pools
{
    public class PoolCommand : Singleton<PoolCommand>
    {
        private Dictionary<int, List<GameObject>> pooledObjects = new Dictionary<int, List<GameObject>>();
        private Dictionary<int, bool> objectsAvailability = new Dictionary<int, bool>();

        public static void Warm(GameObject prefab, int poolSize)
        {
            Instance?.WarmPool(prefab, poolSize);
        }

        public static GameObject GetObject(GameObject prefab)
        {
            return Instance?.GetObjectFromPool(prefab);
        }

        public static GameObject GetObject(GameObject prefab, Transform parent, bool positionWorldSpace = false)
        {
            return Instance?.GetObjectFromPool(prefab, parent, positionWorldSpace);
        }
        public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instance?.GetObjectFromPool(prefab, position, rotation);
        }

        public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool positionWorldSpace = false, bool rotationWorldSpace = false)
        {
            return Instance?.GetObjectFromPool(prefab, position, rotation, parent, positionWorldSpace, rotationWorldSpace);
        }

        public static GameObject GetObject(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            return Instance?.GetObjectFromPool(prefab, position, rotation);
        }

        public static GameObject GetObject(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent, bool positionWorldSpace = false, bool rotationWorldSpace = false)
        {
            return Instance?.GetObjectFromPool(prefab, position, rotation, parent, positionWorldSpace, rotationWorldSpace);
        }

        public static void ReleaseObject(GameObject poolObject)
        {
            Instance?.ReleaseObjectOnPool(poolObject);
        }

        private void WarmPool(GameObject prefab, int poolSize)
        {
            int poolInitialLength = 0;
            if (!pooledObjects.ContainsKey(prefab.GetInstanceID()))
            {
                pooledObjects.Add(prefab.GetInstanceID(), new List<GameObject>());
            }
            else
            {
                poolInitialLength = pooledObjects[prefab.GetInstanceID()].Count;
            }

            for (int i = 0; i < poolSize; i++)
            {
                var newObject = Instantiate(prefab);
                pooledObjects[prefab.GetInstanceID()].Add(newObject);
                pooledObjects[prefab.GetInstanceID()][i + poolInitialLength].SetActive(false);
                objectsAvailability.Add(newObject.GetInstanceID(), true);
            }
        }

        private GameObject GetObjectFromPool(GameObject prefab)
        {
            int length = pooledObjects[prefab.GetInstanceID()].Count;

            for (int i = 0; i < length; i++)
            {
                int id = pooledObjects[prefab.GetInstanceID()][i].GetInstanceID();
                if (objectsAvailability[id])
                {
                    objectsAvailability[id] = false;
                    pooledObjects[prefab.GetInstanceID()][i].SetActive(true);
                    return pooledObjects[prefab.GetInstanceID()][i];
                }
            }
            return null;
        }

        private GameObject GetObjectFromPool(GameObject prefab, Transform parent, bool positionWorldSpace = false)
        {
            var gettedObject = GetObjectFromPool(prefab);
            if (!gettedObject)
                return null;
            gettedObject.transform.SetParent(parent);
            if (!positionWorldSpace)
            {
                gettedObject.transform.localPosition = Vector3.zero;
            }
            return gettedObject;
        }

        private GameObject GetObjectFromPool(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            return GetObjectFromPool(prefab, (Vector3)position, rotation);
        }

        private GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var gettedObject = GetObjectFromPool(prefab);
            if (!gettedObject)
                return null;

            gettedObject.transform.position = position;
            gettedObject.transform.rotation = rotation;

            return gettedObject;
        }

        private GameObject GetObjectFromPool(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent, bool positionWorldSpace = false, bool rotationWorldSpace = false)
        {
            return GetObjectFromPool(prefab, (Vector3)position, rotation, parent, positionWorldSpace, rotationWorldSpace);
        }

        private GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool positionWorldSpace = false, bool rotationWorldSpace = false)
        {
            var gettedObject = GetObjectFromPool(prefab);
            if (!gettedObject)
                return null;

            gettedObject.transform.SetParent(parent);

            if (!positionWorldSpace)
            {
                gettedObject.transform.localPosition = position;
            }
            else
            {
                gettedObject.transform.position = position;
            }

            if (!rotationWorldSpace)
            {
                gettedObject.transform.localRotation = rotation;
            }
            else
            {
                gettedObject.transform.rotation = rotation;
            }

            return gettedObject;
        }


        private void ReleaseObjectOnPool(GameObject poolObject)
        {
            if (!poolObject)
                return;
            if (!objectsAvailability.ContainsKey(poolObject.GetInstanceID()))
                return;

            objectsAvailability[poolObject.GetInstanceID()] = true;
            poolObject.SetActive(false);
            poolObject.transform.SetParent(null);
        }
    }
}