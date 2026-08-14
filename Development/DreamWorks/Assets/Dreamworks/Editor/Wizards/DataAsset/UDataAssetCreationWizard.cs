using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.Core.Assets;

namespace DreamMachineGameStudio.DreamWorks.Editor.Wizard.DataAsset
{
    public sealed class UDataAssetCreationWizard : EditorWindow
    {
        #region Fields
        private Type selectedType;
        private string assetName;
        private string targetFolder;
        private readonly AdvancedDropdownState dropdownState = new AdvancedDropdownState();
        #endregion

        #region Public Methods
        public static void Open(string targetFolder)
        {
            UDataAssetCreationWizard window = GetWindow<UDataAssetCreationWizard>(true, "Create Data Asset");

            window.targetFolder = targetFolder;

            window.minSize = new Vector2(600, 220);

            window.Show();
        }
        #endregion

        #region EditorWindow Methods
        private void OnGUI()
        {
            HandleKeyboard();

            EditorGUILayout.BeginVertical();

            GUILayout.Space(10);

            DrawTypeField();

            GUILayout.Space(5);

            DrawNameField();

            GUILayout.Space(10);

            DrawTargetFolder();

            FDreamWorksEditorGUI.DrawSeparator();

            DrawCreateButton();

            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Private Methods
        private void DrawTypeField()
        {
            EditorGUILayout.LabelField("Asset Type", FDreamWorksEditorStyles.SectionLabel);

            string buttonText = selectedType != null
                ? ObjectNames.NicifyVariableName(selectedType.Name.TrimStart('U'))
                : "Select Data Asset Type";

            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, new GUIContent("Type"));

            if (EditorGUI.DropdownButton(rect, new GUIContent(buttonText), FocusType.Keyboard, EditorStyles.popup))
            {
                IReadOnlyList<Type> types = FTypeCache.GetDerivedTypes(typeof(UDataAsset));

                FTypeDropdown dropdown = new(
                    dropdownState,
                    types,
                    OnTypeSelected,
                    type => ObjectNames.NicifyVariableName(type.Name.TrimStart('U')),
                    "Data Assets");

                dropdown.Show(rect);
            }
        }

        private void DrawNameField()
        {
            EditorGUILayout.LabelField("Asset Name", FDreamWorksEditorStyles.SectionLabel);

            GUI.SetNextControlName("AssetName");

            assetName = EditorGUILayout.TextField(assetName);
        }

        private void DrawTargetFolder()
        {
            EditorGUILayout.LabelField("Create In", FDreamWorksEditorStyles.SectionLabel);

            EditorGUILayout.HelpBox(targetFolder, MessageType.None);
        }

        private void DrawCreateButton()
        {
            using (new EditorGUI.DisabledScope(!CanCreate()))
            {
                if (GUILayout.Button("Create", FDreamWorksEditorStyles.CenteredButton))
                {
                    CreateAsset();
                }
            }
        }
        #endregion

        #region Private Methods
        private void OnTypeSelected(Type type)
        {
            selectedType = type;

            if (string.IsNullOrWhiteSpace(assetName))
            {
                assetName = type.Name.StartsWith("U") ? type.Name.Substring(1) : type.Name;
            }

            GUI.FocusControl("AssetName");

            Repaint();
        }

        private bool CanCreate()
        {
            if (selectedType == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(assetName))
            {
                return false;
            }

            return assetName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        private void HandleKeyboard()
        {
            if (Event.current.type != EventType.KeyDown)
            {
                return;
            }

            if (Event.current.keyCode != KeyCode.Return)
            {
                return;
            }

            if (!CanCreate())
            {
                return;
            }

            CreateAsset();

            Event.current.Use();
        }

        private void CreateAsset()
        {
            UDataAsset asset = CreateInstance(selectedType) as UDataAsset;

            if (asset == null)
            {
                return;
            }

            string path = AssetDatabase.GenerateUniqueAssetPath($"{targetFolder}/{assetName}.asset");

            AssetDatabase.CreateAsset(asset, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = asset;

            EditorGUIUtility.PingObject(asset);

            Close();
        }
        #endregion
    }
}