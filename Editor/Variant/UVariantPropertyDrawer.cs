/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEditor;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Editor.Variant
{
    [CustomPropertyDrawer(typeof(FInt))]
    [CustomPropertyDrawer(typeof(FFloat))]
    [CustomPropertyDrawer(typeof(FString))]
    [CustomPropertyDrawer(typeof(FBool))]
    [CustomPropertyDrawer(typeof(FVector2))]
    [CustomPropertyDrawer(typeof(FVector3))]
    public class UVariantPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_value"), new GUIContent(property.displayName));
        }
    }
}