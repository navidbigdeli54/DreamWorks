/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEditor;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Utility;

namespace DreamMachineGameStudio.Dreamworks.Editor
{
    [CustomPropertyDrawer(typeof(FSubclass))]
    public class USubclassPropertyDrawer : PropertyDrawer
    {
        #region Fields
        private string[] _typeFullNames;
        private string[] _displaynames;
        private int _selectedIndex;
        #endregion

        #region Editor Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProperty = property.FindPropertyRelative("_fullName");

            if (_displaynames == null)
            {
                CacheTypeNames();

                if (_displaynames != null && _displaynames.Length > 0)
                {
                    _selectedIndex = Array.IndexOf(_displaynames, nameProperty.stringValue);

                    if (_selectedIndex == -1) _selectedIndex = 0;
                }
            }

            Vector2 lableSize = GUI.skin.label.CalcSize(label);

            EditorGUI.LabelField(new Rect(position.x, position.y, lableSize.x, position.height), label);
            _selectedIndex = EditorGUI.Popup(new Rect(position.x + lableSize.x + 5, position.y, position.width - lableSize.x - 5, position.height), _selectedIndex, _displaynames);

            if (_displaynames != null && _displaynames.Length > 0)
            {
                nameProperty.stringValue = _typeFullNames[_selectedIndex];
            }
        }
        #endregion

        #region Private Methods
        private void CacheTypeNames()
        {
            if (fieldInfo.IsDefined(FSubclassFilterAttribute.CLASS_TYPE))
            {
                Attribute attribute = fieldInfo.GetCustomAttribute(FSubclassFilterAttribute.CLASS_TYPE);
                PropertyInfo typePropertyInfo = attribute.GetType().GetProperty(nameof(FSubclassFilterAttribute.Type));
                Type typeFilter = (Type)typePropertyInfo.GetValue(attribute);
                IEnumerable<Type> subtypes = FReflectionUtility.GetSubTypesOf(typeFilter);
                _typeFullNames = subtypes.Select(x => x.FullName).ToArray();
                _displaynames = subtypes.Select(x => FReflectionUtility.HasDefinedAttribute(x, FNameAttribute.CLASS_TYPE) ? FReflectionUtility.GetAttributeProperty<string>(x, FNameAttribute.CLASS_TYPE, nameof(FNameAttribute.Name)) : x.FullName).ToArray();
            }
        }

        #endregion
    }
}