using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Jozi.Utilities.Patterns;
using OAKS.Utilities.Views;

namespace Jozi.Level
{
    public class LevelLoaderManager : Singleton<LevelLoaderManager>
    {
        [Header("Setup")]
        public Animator Animator;
        public string TriggerName = "start";
        public float TransitionTime = 2;
        public string NextLevel;
        public Slider LoadSlider;

        [Header("View")]
        public ViewBase View;
        public ViewMenuController ViewController;

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevelCO(NextLevel));
        }

        public static void Load(int sceneIndex)
        {
            Instance?.LoadLevel(sceneIndex);
        }

        public static void Load(string sceneName)
        {
            Instance?.LoadLevel(sceneName);
        }

        public void LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadLevelCO(SceneManager.GetSceneByBuildIndex(sceneIndex).name));
        }

        public void LoadLevel(string sceneName)
        {
            StartCoroutine(LoadLevelCO(sceneName));
        }

        private IEnumerator LoadLevelCO(string sceneName)
        {
            ViewController.PushView(View);
            yield return new WaitForSeconds(TransitionTime);

            StartCoroutine(LoadAsynchrously(sceneName));
        }

        private IEnumerator LoadAsynchrously(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                //LoadSlider.value = progress;
                yield return null;
            }

            ViewController.PopView();
        }
    }
}