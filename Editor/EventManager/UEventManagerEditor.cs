/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0051

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.EventManager;

namespace DreamMachineGameStudio.Dreamworks.Editor.EventManager
{
    public class UEventManagerEditor : EditorWindow
    {
        #region Fields
        private Vector3 _scrollPosition;
        #endregion

        #region Private Methods
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.LabelField("Event");
            EditorGUILayout.LabelField("Listeners");
            EditorGUILayout.EndHorizontal();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (KeyValuePair<FStringId, FEventManager.DEventSubscriber> item in FEventManager.Events)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(item.Key.Value, GUILayout.Width(position.width / 2));

                EditorGUILayout.BeginVertical();
                foreach (Delegate subscriber in item.Value.GetInvocationList())
                {
                    EditorGUILayout.LabelField($"*{subscriber.Target.GetType().FullName}.{subscriber.Method.Name}", GUILayout.Width(position.width / 2));
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Static Methods
        [MenuItem("DreamMachineGameStudio/Event Manager")]
        private static void OpenWindow()
        {
            GetWindow<UEventManagerEditor>("Event Management", focus: true);
        }
        #endregion
    }
}