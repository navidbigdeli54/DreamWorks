using UnityEditor;

namespace DreamMachineGameStudio.DreamWorks.Editor
{
    public static class FSerializedPropertyExtensions
    {
        public static SerializedProperty FindAutoProperty(this SerializedProperty serializedProperty, string propertyName)
        {
            return serializedProperty.FindPropertyRelative($"<{propertyName}>k__BackingField");
        }
    }

    public static class FSerializedObjectExtensions
    {
        public static SerializedProperty FindAutoProperty(this SerializedObject serializedObject, string propertyName)
        {
            return serializedObject.FindProperty($"<{propertyName}>k__BackingField");
        }
    }
}
