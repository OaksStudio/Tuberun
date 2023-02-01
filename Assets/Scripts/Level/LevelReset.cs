using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jozi.Level
{
    public class LevelReset : MonoBehaviour
    {
        public void ReStart()
        {
            LevelLoaderManager.Load(SceneManager.GetActiveScene().buildIndex);
        }
    }
}