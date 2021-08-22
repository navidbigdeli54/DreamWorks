/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using UnityEditor;
using DreamMachineGameStudio.Dreamworks.Core;

using UEditor = UnityEditor.Editor;

namespace DreamMachineGameStudio.Dreamworks.Editor
{
    [CustomEditor(typeof(SDreamworksConfiguration))]
    public class UDreamworksConfiguration : UEditor
    {
        #region Field
        private SerializedProperty _startupClassProperty;
        private SerializedProperty _dontLoadFrameworkProperty;

        private bool foldout = true;
        #endregion

        #region Static Methods
        [MenuItem(itemName: "DreamMachineGameStudio/DreamWork/Configuration", priority = 0)]
        private static void OpenConfigurationFile()
        {
            Selection.activeObject = Resources.Load(nameof(SDreamworksConfiguration));
        }
        #endregion

        #region Method
        private void OnEnable()
        {
            _dontLoadFrameworkProperty = serializedObject.FindProperty("_dontLoadFramework");

            _startupClassProperty = serializedObject.FindProperty("_startupClass");
        }

        public override void OnInspectorGUI()
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, target);

            if (foldout)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.PropertyField(_dontLoadFrameworkProperty);

                EditorGUILayout.PropertyField(_startupClassProperty);

                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.EndVertical();
            }
        }
        #endregion
    }
}