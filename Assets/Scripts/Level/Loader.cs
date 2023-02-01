using Jozi.Utilities.Parameters;
using UnityEngine;

namespace Jozi.Level
{
    public class Loader : MonoBehaviour
    {
        public SOScene scene;

        public void Load()
        {
            LevelLoaderManager.Load(scene.ScenePath);
        }
    }
}