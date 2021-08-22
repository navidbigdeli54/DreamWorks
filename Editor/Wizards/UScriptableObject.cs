/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Utility;

namespace DreamMachineGameStudio.Dreamworks.Editor
{
    public class UScriptableObject : EditorWindow
    {
        #region Field
        private int _selectedIndex;
        private string[] _scriptableObjectNames;
        private Type[] _scriptableObjectTypes;

        private const string POPUP_LABLE_NAME = "Scriptable Objects";
        private const string CREATE_BUTTON_NAME = "Create";
        #endregion

        #region Static Methods
        [MenuItem(itemName: "Assets/Create/ScriptableObject", priority = 100)]
        private static void OpenWizardWindow()
        {
            UScriptableObject window = GetWindow<UScriptableObject>(false, "Scriptable Object Wizard");
            window.maxSize = window.minSize = new Vector2(400, 40);
            window.Show();
        }
        #endregion

        #region EditorWindow Methods
        private void OnEnable()
        {
            _scriptableObjectTypes = FReflectionUtility.GetSubTypesOf<SScriptableObject>().Where(x => FReflectionUtility.HasDefinedAttribute(x, FScriptableObjectWizardAttribute.CLASS_TYPE)).ToArray();

            _scriptableObjectNames = _scriptableObjectTypes.Select(x => FReflectionUtility.GetAttributeProperty<string>(x, FScriptableObjectWizardAttribute.CLASS_TYPE, nameof(FScriptableObjectWizardAttribute.Name))).ToArray();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            _selectedIndex = EditorGUILayout.Popup(POPUP_LABLE_NAME, _selectedIndex, _scriptableObjectNames);

            if (GUILayout.Button(CREATE_BUTTON_NAME))
            {

                string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                ScriptableObject asset = CreateInstance(_scriptableObjectTypes.ElementAt(_selectedIndex));

                ProjectWindowUtil.CreateAsset(asset, $"{path}/{_scriptableObjectTypes.ElementAt(_selectedIndex).Name}.asset");

                Close();
            }

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}