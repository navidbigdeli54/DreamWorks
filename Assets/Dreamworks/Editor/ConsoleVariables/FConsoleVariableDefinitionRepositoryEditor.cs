using System;
using System.Collections.Generic;
using System.Globalization;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamMachineGameStudio.DreamWorks.Editor.ConsoleVariables
{
    [CustomEditor(typeof(FConsoleVariableDefinitionRepository))]
    public sealed class UConsoleVariableDefinitionRepositoryInspector : UnityEditor.Editor
    {
        #region Fields
        private const string NewDefinitionName = "NewVariable";
        private const string DefaultBooleanValue = "false";
        private const string DefaultIntegerValue = "0";
        private const string DefaultFloatValue = "0";
        private const string DefaultStringValue = "";
        private readonly List<HelpBox> validationMessages = new();
        private readonly List<Foldout> definitionFoldouts = new();
        private VisualElement inspectorRoot;
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the UI Toolkit inspector for a console variable definition repository.
        /// </summary>
        public override VisualElement CreateInspectorGUI()
        {
            inspectorRoot = new VisualElement();
            RebuildInspector();
            return inspectorRoot;
        }
        #endregion

        #region Private Methods
        private void RebuildInspector()
        {
            serializedObject.Update();
            inspectorRoot.Clear();
            validationMessages.Clear();
            definitionFoldouts.Clear();
            inspectorRoot.style.paddingLeft = 4;
            inspectorRoot.style.paddingRight = 4;
            inspectorRoot.style.paddingTop = 6;
            inspectorRoot.Add(BuildHeader());
            SerializedProperty definitions = FindDefinitionsProperty();
            if (definitions == null)
            {
                inspectorRoot.Add(new HelpBox("Could not find the serialized definitions list on this repository.", HelpBoxMessageType.Error));
                return;
            }

            for (int index = 0; index < definitions.arraySize; index++)
            {
                inspectorRoot.Add(BuildDefinitionRow(definitions.GetArrayElementAtIndex(index), index));
            }

            Button addButton = new(AddDefinition) { text = "+  Add Variable" };
            addButton.style.height = 30;
            addButton.style.marginTop = 4;
            addButton.style.marginBottom = 8;
            addButton.style.unityFontStyleAndWeight = FontStyle.Bold;
            inspectorRoot.Add(addButton);
            RefreshValidationMessages();
        }

        private VisualElement BuildHeader()
        {
            VisualElement header = new();
            header.style.marginBottom = 10;
            Label title = new("Console Variables");
            title.style.fontSize = 16;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.Add(title);
            Label subtitle = new("Define runtime variables, defaults, and persistence.");
            subtitle.style.marginTop = 2;
            subtitle.style.marginBottom = 6;
            subtitle.style.opacity = 0.72f;
            header.Add(subtitle);
            return header;
        }

        private void StyleDefinitionFoldout(Foldout foldout)
        {
            Color cardColor = EditorGUIUtility.isProSkin ? new Color(0.19f, 0.19f, 0.19f, 1f) : new Color(0.94f, 0.94f, 0.94f, 1f);
            Color borderColor = EditorGUIUtility.isProSkin ? new Color(0.31f, 0.31f, 0.31f, 1f) : new Color(0.78f, 0.78f, 0.78f, 1f);
            foldout.style.backgroundColor = cardColor;
            foldout.style.borderTopWidth = 1;
            foldout.style.borderBottomWidth = 1;
            foldout.style.borderLeftWidth = 1;
            foldout.style.borderRightWidth = 1;
            foldout.style.borderTopColor = borderColor;
            foldout.style.borderBottomColor = borderColor;
            foldout.style.borderLeftColor = borderColor;
            foldout.style.borderRightColor = borderColor;
            foldout.style.borderTopLeftRadius = 5;
            foldout.style.borderTopRightRadius = 5;
            foldout.style.borderBottomLeftRadius = 5;
            foldout.style.borderBottomRightRadius = 5;
            foldout.style.marginBottom = 6;
            foldout.style.paddingLeft = 8;
            foldout.style.paddingRight = 8;
            foldout.style.paddingTop = 5;
            foldout.style.paddingBottom = 5;
        }

        private void AddDefinitionField(Foldout foldout, VisualElement field)
        {
            field.style.marginBottom = 3;
            foldout.Add(field);
        }

        private VisualElement BuildDefinitionRow(SerializedProperty definition, int index)
        {
            SerializedProperty nameProperty = FindRelativeProperty(definition, "Name", "name");
            SerializedProperty descriptionProperty = FindRelativeProperty(definition, "Description", "description");
            SerializedProperty typeProperty = FindRelativeProperty(definition, "VariableType", "variableType");
            SerializedProperty persistentProperty = FindRelativeProperty(definition, "IsPersistent", "isPersistent");
            SerializedProperty defaultProperty = FindRelativeProperty(definition, "DefaultValue", "defaultValue");
            string variableName = nameProperty != null && !string.IsNullOrWhiteSpace(nameProperty.stringValue) ? nameProperty.stringValue : $"Variable {index + 1}";
            Foldout foldout = new() { text = variableName, value = false };
            StyleDefinitionFoldout(foldout);
            definitionFoldouts.Add(foldout);

            if (nameProperty != null && nameProperty.propertyType == SerializedPropertyType.String)
            {
                TextField nameField = new("Name") { value = nameProperty.stringValue };
                nameField.RegisterValueChangedCallback(evt =>
                {
                    UpdateStringProperty(nameProperty, evt.newValue);
                    foldout.text = string.IsNullOrWhiteSpace(evt.newValue) ? $"Variable {index + 1}" : evt.newValue;
                });
                AddDefinitionField(foldout, nameField);
            }

            if (descriptionProperty != null && descriptionProperty.propertyType == SerializedPropertyType.String)
            {
                TextField descriptionField = new("Description") { value = descriptionProperty.stringValue };
                descriptionField.RegisterValueChangedCallback(evt => UpdateStringProperty(descriptionProperty, evt.newValue));
                AddDefinitionField(foldout, descriptionField);
            }

            if (typeProperty != null && typeProperty.propertyType == SerializedPropertyType.Enum)
            {
                AddDefinitionField(foldout, BuildTypeField(typeProperty, defaultProperty));
            }

            if (persistentProperty != null && persistentProperty.propertyType == SerializedPropertyType.Boolean)
            {
                Toggle persistentField = new("Persistent") { value = persistentProperty.boolValue };
                persistentField.RegisterValueChangedCallback(evt => UpdateBooleanProperty(persistentProperty, evt.newValue));
                AddDefinitionField(foldout, persistentField);
            }

            if (defaultProperty != null && defaultProperty.propertyType == SerializedPropertyType.String && typeProperty != null)
            {
                AddDefinitionField(foldout, BuildDefaultValueField(defaultProperty, typeProperty));
            }
            else
            {
                foldout.Add(new HelpBox("The definition is missing a serialized string DefaultValue or VariableType field.", HelpBoxMessageType.Error));
            }

            HelpBox validationMessage = new(string.Empty, HelpBoxMessageType.Error);
            validationMessage.style.display = DisplayStyle.None;
            validationMessage.style.marginTop = 4;
            validationMessage.style.marginBottom = 4;
            foldout.Add(validationMessage);
            validationMessages.Add(validationMessage);
            Button deleteButton = new(() => DeleteDefinition(index)) { text = "Remove Variable" };
            deleteButton.style.marginTop = 6;
            deleteButton.style.marginBottom = 2;
            foldout.Add(deleteButton);
            return foldout;
        }

        private VisualElement BuildTypeField(SerializedProperty typeProperty, SerializedProperty defaultProperty)
        {
            Enum selectedType = (Enum)Enum.Parse(typeof(EConsoleVariableType), typeProperty.enumNames[typeProperty.enumValueIndex]);
            EnumField typeField = new("Type", selectedType);
            typeField.RegisterValueChangedCallback(evt =>
            {
                int enumIndex = Array.IndexOf(typeProperty.enumNames, evt.newValue.ToString());
                if (enumIndex < 0)
                {
                    return;
                }

                typeProperty.enumValueIndex = enumIndex;
                if (defaultProperty != null && !IsValidDefaultValue(typeProperty, defaultProperty.stringValue))
                {
                    SetStringProperty(defaultProperty, GetDefaultValue((EConsoleVariableType)Enum.Parse(typeof(EConsoleVariableType), evt.newValue.ToString())));
                }

                serializedObject.ApplyModifiedProperties();
                RebuildInspector();
            });
            return typeField;
        }

        private VisualElement BuildDefaultValueField(SerializedProperty defaultProperty, SerializedProperty typeProperty)
        {
            EConsoleVariableType variableType = (EConsoleVariableType)Enum.Parse(typeof(EConsoleVariableType), typeProperty.enumNames[typeProperty.enumValueIndex]);
            string rawValue = defaultProperty.stringValue;
            switch (variableType)
            {
                case EConsoleVariableType.Boolean:
                    Toggle booleanField = new("Default Value") { value = bool.TryParse(rawValue, out bool booleanValue) && booleanValue };
                    booleanField.RegisterValueChangedCallback(evt => UpdateStringProperty(defaultProperty, evt.newValue.ToString().ToLowerInvariant()));
                    return booleanField;
                case EConsoleVariableType.Integer:
                    IntegerField integerField = new("Default Value") { value = int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int integerValue) ? integerValue : 0 };
                    integerField.RegisterValueChangedCallback(evt => UpdateStringProperty(defaultProperty, evt.newValue.ToString(CultureInfo.InvariantCulture)));
                    return integerField;
                case EConsoleVariableType.Float:
                    FloatField floatField = new("Default Value") { value = float.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue) ? floatValue : 0f };
                    floatField.RegisterValueChangedCallback(evt => UpdateStringProperty(defaultProperty, evt.newValue.ToString("R", CultureInfo.InvariantCulture)));
                    return floatField;
                case EConsoleVariableType.String:
                    TextField stringField = new("Default Value") { value = rawValue };
                    stringField.RegisterValueChangedCallback(evt => UpdateStringProperty(defaultProperty, evt.newValue));
                    return stringField;
                default:
                    return new HelpBox("Unsupported console variable type.", HelpBoxMessageType.Error);
            }
        }

        private void AddDefinition()
        {
            serializedObject.Update();
            SerializedProperty definitions = FindDefinitionsProperty();
            if (definitions == null)
            {
                return;
            }

            int newIndex = definitions.arraySize;
            definitions.InsertArrayElementAtIndex(newIndex);
            SerializedProperty definition = definitions.GetArrayElementAtIndex(newIndex);
            SetStringProperty(FindRelativeProperty(definition, "Name", "name"), GetUniqueNewName(definitions));
            SetStringProperty(FindRelativeProperty(definition, "Description", "description"), string.Empty);
            SetEnumProperty(FindRelativeProperty(definition, "VariableType", "variableType"), EConsoleVariableType.Boolean);
            SerializedProperty persistentProperty = FindRelativeProperty(definition, "IsPersistent", "isPersistent");
            if (persistentProperty != null && persistentProperty.propertyType == SerializedPropertyType.Boolean)
            {
                persistentProperty.boolValue = false;
            }

            SetStringProperty(FindRelativeProperty(definition, "DefaultValue", "defaultValue"), DefaultBooleanValue);
            serializedObject.ApplyModifiedProperties();
            RebuildInspector();
        }

        private string GetUniqueNewName(SerializedProperty definitions)
        {
            string candidate = NewDefinitionName;
            int suffix = 1;
            while (ContainsName(definitions, candidate))
            {
                candidate = $"{NewDefinitionName}{suffix}";
                suffix++;
            }

            return candidate;
        }

        private bool ContainsName(SerializedProperty definitions, string candidate)
        {
            for (int index = 0; index < definitions.arraySize; index++)
            {
                SerializedProperty nameProperty = FindRelativeProperty(definitions.GetArrayElementAtIndex(index), "Name", "name");
                if (nameProperty != null && string.Equals(nameProperty.stringValue, candidate, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void DeleteDefinition(int index)
        {
            serializedObject.Update();
            SerializedProperty definitions = FindDefinitionsProperty();
            if (definitions == null || index < 0 || index >= definitions.arraySize)
            {
                return;
            }

            int previousSize = definitions.arraySize;
            definitions.DeleteArrayElementAtIndex(index);
            if (definitions.arraySize == previousSize)
            {
                definitions.DeleteArrayElementAtIndex(index);
            }

            serializedObject.ApplyModifiedProperties();
            RebuildInspector();
        }

        private void UpdateStringProperty(SerializedProperty property, string value)
        {
            property.stringValue = value;
            serializedObject.ApplyModifiedProperties();
            RefreshValidationMessages();
        }

        private void UpdateBooleanProperty(SerializedProperty property, bool value)
        {
            property.boolValue = value;
            serializedObject.ApplyModifiedProperties();
            RefreshValidationMessages();
        }

        private void RefreshValidationMessages()
        {
            serializedObject.Update();
            SerializedProperty definitions = FindDefinitionsProperty();
            if (definitions == null)
            {
                return;
            }

            for (int index = 0; index < validationMessages.Count && index < definitions.arraySize; index++)
            {
                string validationMessage = GetValidationMessage(definitions, index);
                validationMessages[index].text = validationMessage;
                validationMessages[index].style.display = string.IsNullOrEmpty(validationMessage) ? DisplayStyle.None : DisplayStyle.Flex;
                SerializedProperty nameProperty = FindRelativeProperty(definitions.GetArrayElementAtIndex(index), "Name", "name");
                if (nameProperty != null && index < definitionFoldouts.Count)
                {
                    definitionFoldouts[index].text = string.IsNullOrWhiteSpace(nameProperty.stringValue) ? $"Variable {index + 1}" : nameProperty.stringValue;
                }
            }
        }

        private string GetValidationMessage(SerializedProperty definitions, int index)
        {
            SerializedProperty definition = definitions.GetArrayElementAtIndex(index);
            SerializedProperty nameProperty = FindRelativeProperty(definition, "Name", "name");
            SerializedProperty typeProperty = FindRelativeProperty(definition, "VariableType", "variableType");
            SerializedProperty defaultProperty = FindRelativeProperty(definition, "DefaultValue", "defaultValue");
            List<string> errors = new();
            if (nameProperty == null || !IsValidIdentifier(nameProperty.stringValue))
            {
                errors.Add("Name must be a valid identifier: start with a letter or underscore, followed by letters, digits, or underscores.");
            }
            else if (HasDuplicateName(definitions, index, nameProperty.stringValue))
            {
                errors.Add("Name duplicates another definition (names are compared without case sensitivity).");
            }

            if (typeProperty != null && defaultProperty != null && !IsValidDefaultValue(typeProperty, defaultProperty.stringValue))
            {
                errors.Add("Default Value is not valid for the selected variable type.");
            }

            return string.Join(" ", errors);
        }

        private bool HasDuplicateName(SerializedProperty definitions, int currentIndex, string candidate)
        {
            for (int index = 0; index < definitions.arraySize; index++)
            {
                if (index == currentIndex)
                {
                    continue;
                }

                SerializedProperty otherName = FindRelativeProperty(definitions.GetArrayElementAtIndex(index), "Name", "name");
                if (otherName != null && string.Equals(otherName.stringValue, candidate, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsValidIdentifier(string value)
        {
            if (string.IsNullOrEmpty(value) || !(char.IsLetter(value[0]) || value[0] == '_'))
            {
                return false;
            }

            for (int index = 1; index < value.Length; index++)
            {
                if (!(char.IsLetterOrDigit(value[index]) || value[index] == '_'))
                {
                    return false;
                }
            }

            return true;
        }
        private static string GetDefaultValue(EConsoleVariableType variableType)
        {
            switch (variableType)
            {
                case EConsoleVariableType.Boolean:
                    return DefaultBooleanValue;
                case EConsoleVariableType.Integer:
                    return DefaultIntegerValue;
                case EConsoleVariableType.Float:
                    return DefaultFloatValue;
                case EConsoleVariableType.String:
                    return DefaultStringValue;
                default:
                    return string.Empty;
            }
        }



        private static bool IsValidDefaultValue(SerializedProperty typeProperty, string value)
        {
            EConsoleVariableType variableType = (EConsoleVariableType)Enum.Parse(typeof(EConsoleVariableType), typeProperty.enumNames[typeProperty.enumValueIndex]);
            switch (variableType)
            {
                case EConsoleVariableType.Boolean:
                    return bool.TryParse(value, out _);
                case EConsoleVariableType.Integer:
                    return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
                case EConsoleVariableType.Float:
                    return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue) && !float.IsNaN(floatValue) && !float.IsInfinity(floatValue);
                case EConsoleVariableType.String:
                    return true;
                default:
                    return false;
            }
        }

        private SerializedProperty FindDefinitionsProperty()
        {
            string[] propertyNames = { "definitions", "Definitions", "m_Definitions", "_definitions", "<Definitions>k__BackingField" };
            for (int index = 0; index < propertyNames.Length; index++)
            {
                SerializedProperty property = serializedObject.FindProperty(propertyNames[index]);
                if (property != null && property.isArray && property.propertyType == SerializedPropertyType.Generic)
                {
                    return property;
                }
            }

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (iterator.isArray && iterator.propertyType == SerializedPropertyType.Generic && iterator.name.IndexOf("definition", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return iterator.Copy();
                }
            }

            return null;
        }

        private static SerializedProperty FindRelativeProperty(SerializedProperty parent, params string[] propertyNames)
        {
            for (int index = 0; index < propertyNames.Length; index++)
            {
                string propertyName = propertyNames[index];
                SerializedProperty property = parent.FindPropertyRelative(propertyName);
                if (property != null)
                {
                    return property;
                }

                string backingFieldName = $"<{char.ToUpperInvariant(propertyName[0])}{propertyName.Substring(1)}>k__BackingField";
                property = parent.FindPropertyRelative(backingFieldName);
                if (property != null)
                {
                    return property;
                }
            }

            return null;
        }

        private static void SetStringProperty(SerializedProperty property, string value)
        {
            if (property != null && property.propertyType == SerializedPropertyType.String)
            {
                property.stringValue = value;
            }
        }

        private static void SetEnumProperty(SerializedProperty property, EConsoleVariableType value)
        {
            if (property == null || property.propertyType != SerializedPropertyType.Enum)
            {
                return;
            }

            int enumIndex = Array.IndexOf(property.enumNames, value.ToString());
            if (enumIndex >= 0)
            {
                property.enumValueIndex = enumIndex;
            }
        }
        #endregion
    }
}
