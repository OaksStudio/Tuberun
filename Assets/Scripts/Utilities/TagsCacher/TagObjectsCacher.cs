using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jozi.Utilities.TagsCacher
{
    public static class TagObjectsCacher
    {
        private static Dictionary<string, List<GameObject>> objectsCached = new Dictionary<string, List<GameObject>>();

        public static void Cache(GameObject gameObject)
        {
            if (objectsCached.ContainsKey(gameObject.tag))
            {
                if (!objectsCached[gameObject.tag].Contains(gameObject))
                {
                    objectsCached[gameObject.tag].Add(gameObject);
                }
            }
            else
            {
                objectsCached.Add(gameObject.tag, new List<GameObject> { gameObject });
            }
        }

        public static void UnCache(GameObject gameObject)
        {
            if (objectsCached.ContainsKey(gameObject.tag))
            {
                if (objectsCached[gameObject.tag].Contains(gameObject))
                {
                    objectsCached[gameObject.tag].Remove(gameObject);
                }
            }
        }

        public static List<GameObject> GetObjects(string tag, bool getDeactivated = false)
        {
            if (!objectsCached.ContainsKey(tag))
            {
                objectsCached.Add(tag, new List<GameObject> { });
            }
            if (getDeactivated)
                return objectsCached[tag];
            else
                return objectsCached[tag].FindAll(o => o.activeInHierarchy);
        }

    }
}