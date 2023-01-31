using UnityEngine;

namespace Jozi.Utilities.TagsCacher
{
    public class RegisterTagCache : MonoBehaviour
    {
        private void Awake()
        {
            RegisterCache();
        }

        private void OnDestroy()
        {
            UnRegisterCache();
        }

        public void RegisterCache()
        {
            TagObjectsCacher.Cache(gameObject);
        }

        public void UnRegisterCache()
        {
            TagObjectsCacher.UnCache(gameObject);
        }
    }
}