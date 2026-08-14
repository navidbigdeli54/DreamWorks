using System;
using UnityEditor;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core;

namespace DreamMachineGameStudio.DreamWorks.Editor.SubSystems
{
    [CustomEditor(typeof(USubSystemRegistery))]
    public sealed class USubSystemRegisteryEditor : UnityEditor.Editor
    {
        #region Fields
        private SerializedProperty subSystemSettings;

        private FSubSystemRegistrySynchronizer synchronizer;

        private string searchText = string.Empty;

        private bool expandAll;
        #endregion

        #region Editor Methods
        private void OnEnable()
        {
            subSystemSettings = serializedObject.FindAutoProperty(nameof(USubSystemRegistery.SubSystemSettings));

            synchronizer = new FSubSystemRegistrySynchronizer(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawToolbar();

            GUILayout.Space(8);

            DrawSubsystemSection();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private Methods
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            searchText = GUILayout.TextField(searchText, GUI.skin.FindStyle("ToolbarSearchTextField"), GUILayout.MinWidth(200));

            if (GUILayout.Button("Scan", EditorStyles.toolbarButton))
            {
                synchronizer.Synchronize();
            }

            if (GUILayout.Button("Expand All", EditorStyles.toolbarButton))
            {
                expandAll = true;
            }

            if (GUILayout.Button("Collapse All", EditorStyles.toolbarButton))
            {
                expandAll = false;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSubsystemSection()
        {
            for (int i = 0; i < subSystemSettings.arraySize; i++)
            {
                SerializedProperty element = subSystemSettings.GetArrayElementAtIndex(i);

                SerializedProperty subSystemProp = element.FindAutoProperty(nameof(FSubSystemSettings.SubSystem));

                Type type = GetSubsystemType(subSystemProp);
                if (type == null)
                {
                    continue;
                }

                if (!FSubSystemEditorUtility.MatchesSearch(type, searchText))
                {
                    continue;
                }

                DrawSubsystem(element, type);
            }
        }

        private void DrawSubsystem(SerializedProperty element, Type type)
        {
            EditorGUILayout.BeginVertical("box");

            bool expanded = DrawSubSystemFoldout(type);

            if (expanded || expandAll)
            {
                EditorGUILayout.Space(4);

                EditorGUILayout.LabelField(FSubSystemEditorUtility.GetMetadata(type).Description, EditorStyles.wordWrappedLabel);

                EditorGUILayout.Space(6);

                DrawAllFields(element);
            }

            EditorGUILayout.EndVertical();
        }

        private bool DrawSubSystemFoldout(Type type)
        {
            EditorGUILayout.BeginHorizontal();

            string name = FSubSystemEditorUtility.GetDisplayName(type);

            string tags = FSubSystemEditorUtility.GetTagString(type);

            bool expanded = SessionState.GetBool(type.FullName, false);

            expanded = EditorGUILayout.Foldout(expanded, name, true);

            GUILayout.Label(tags, EditorStyles.miniLabel);

            SessionState.SetBool(type.FullName, expanded);

            EditorGUILayout.EndHorizontal();

            return expanded;
        }

        private void DrawAllFields(SerializedProperty element)
        {
            SerializedProperty iterator = element.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        private Type GetSubsystemType(SerializedProperty subSystemProp)
        {
            if (subSystemProp == null)
            {
                return null;
            }

            SerializedProperty typeProperty = subSystemProp.FindPropertyRelative("assemblyQualifiedTypeName");

            if (typeProperty == null || string.IsNullOrEmpty(typeProperty.stringValue))
            {
                return null;
            }

            return Type.GetType(typeProperty.stringValue);
        }
        #endregion
    }
}