using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using DreamMachineGameStudio.DreamWorks.Core;

namespace DreamMachineGameStudio.DreamWorks.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TSubclassOf<>))]
    public class TSubclassOfPropertyDrawer : PropertyDrawer
    {
        #region Fields
        private readonly AdvancedDropdownState dropdownState = new();
        #endregion

        #region PropertyDrawer
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProperty = property.FindPropertyRelative("assemblyQualifiedTypeName");

            Type baseType = GetBaseType();

            if (baseType == null)
            {
                EditorGUI.LabelField(position, label.text, "Unable to resolve generic type");
                return;
            }

            Type currentType = string.IsNullOrEmpty(typeProperty.stringValue)
                ? null
                : Type.GetType(typeProperty.stringValue);

            Rect totalRect = position;
            Rect fieldRect = EditorGUI.PrefixLabel(totalRect, label);

            string buttonText = currentType != null
                ? ObjectNames.NicifyVariableName(currentType.Name)
                : $"Select {baseType.Name}";

            if (EditorGUI.DropdownButton(fieldRect, new GUIContent(buttonText), FocusType.Keyboard, EditorStyles.popup))
            {
                FTypeDropdown dropdown = new FTypeDropdown(
                    dropdownState,
                    FTypeCache.GetDerivedTypes(baseType),
                    selected =>
                    {
                        typeProperty.stringValue = selected.AssemblyQualifiedName;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    type => ObjectNames.NicifyVariableName(type.Name));

                dropdown.Show(totalRect);
            }
        }
        #endregion

        #region Private Methods
        private Type GetBaseType()
        {
            return GetBaseType(fieldInfo.FieldType);
        }

        private static Type GetBaseType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    Type generic = type.GetGenericTypeDefinition();

                    if (generic == typeof(TSubclassOf<>))
                    {
                        return type.GetGenericArguments()[0];
                    }

                    type = type.GetGenericArguments()[0];

                    continue;
                }

                if (type.IsArray)
                {
                    type = type.GetElementType();

                    continue;
                }

                break;
            }

            return null;
        }
        #endregion
    }
}