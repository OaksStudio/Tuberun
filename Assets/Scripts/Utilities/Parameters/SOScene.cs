using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jozi.Utilities.Parameters
{
    [CreateAssetMenu(fileName = "SO_Scene", menuName = "Parameters/Scene", order = 0)]
    public class SOScene : ScriptableObject
    {
        [SerializeField]
        public string ScenePath;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SOScene), true)]
    public class SOSceneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var picker = target as SOScene;
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.ScenePath);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("ScenePath");
                scenePathProperty.stringValue = newPath;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}