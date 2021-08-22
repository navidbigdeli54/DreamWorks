/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0051

using UnityEditor;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.ServiceLocator;

namespace DreamMachineGameStudio.Dreamworks.Editor.ServiceLocator
{
    public class UServiceLocatorEditor : EditorWindow
    {
        #region Fields
        private Vector3 _scrollPosition;
        #endregion

        #region Methods
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.LabelField("Service");
            EditorGUILayout.LabelField("Instance");
            EditorGUILayout.EndHorizontal();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var item in FServiceLocator.RegisteredServices)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(item.Key.Name);
                EditorGUILayout.LabelField(item.Value.GetType().FullName);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Static Methods
        [MenuItem("DreamMachineGameStudio/Service Locator")]
        private static void OpenWindow()
        {
            GetWindow<UServiceLocatorEditor>("Service Locator", focus: true);
        }
        #endregion
    }
}