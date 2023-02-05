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
        public float TransitionInTime = 0.1f;
        public float TransitionOutTime = 0.1f;
        public string NextLevel;
        public ViewAnimationSettingsSO OverrideEnterSettings;

        [Header("View")]
        public ViewBase View;
        public ViewMenuController ViewController;

        private static bool _wasLoading;

        protected override void Awake()
        {
            base.Awake();
            if (_wasLoading)
            {
                View.EnterSettings = OverrideEnterSettings;
                ViewController.ForceInitialView(View);
                StartCoroutine(PopUpAfterLoad());
            }
        }

        private IEnumerator PopUpAfterLoad()
        {
            yield return new WaitForSeconds(TransitionInTime);
            ViewController.PopView();
        }

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
            _wasLoading = true;
            ViewController.PushView(View);
            yield return new WaitForSeconds(TransitionInTime);

            StartCoroutine(LoadAsynchrously(sceneName));
        }

        private IEnumerator LoadAsynchrously(string sceneName)
        {
            _wasLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                //float progress = Mathf.Clamp01(operation.progress / .9f);
                yield return null;
            }
        }
    }
}