using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.Core.World.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;

namespace DreamMachineGameStudio.DreamWorks.Editor.SubSystems
{
    internal sealed class FSubSystemRegistrySynchronizer
    {
        #region Fields
        private readonly SerializedObject serializedObject;
        #endregion

        #region Constructors
        public FSubSystemRegistrySynchronizer(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;

            Synchronize();
        }
        #endregion

        #region Public Methods
        public void Synchronize()
        {
            SerializedProperty SubSystemSettings = serializedObject.FindAutoProperty(nameof(USubSystemRegistery.SubSystemSettings));

            SynchronizeList(SubSystemSettings, FSubSystemDiscovery.GameInstanceSubsystems);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Private Methods
        private void SynchronizeList(SerializedProperty serializedProperty, IReadOnlyList<Type> discovered)
        {
            HashSet<string> existingTypes = GetExistedTypes(serializedProperty);

            AddMissingSubSystems(serializedProperty, discovered, existingTypes);

            RemoveInvalidSubSystems(serializedProperty, discovered);
        }

        private static HashSet<string> GetExistedTypes(SerializedProperty serializedProperty)
        {
            HashSet<string> result = new HashSet<string>();

            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                SerializedProperty element = serializedProperty.GetArrayElementAtIndex(i);

                SerializedProperty subSystemProp = element.FindAutoProperty(nameof(FSubSystemSettings.SubSystem));

                if (subSystemProp == null)
                {
                    continue;
                }

                SerializedProperty typeProperty = subSystemProp.FindPropertyRelative("assemblyQualifiedTypeName");
                if (typeProperty == null)
                {
                    continue;
                }

                string typeName = typeProperty.stringValue;

                if (!string.IsNullOrEmpty(typeName))
                {
                    result.Add(typeName);
                }
            }

            return result;
        }

        private void AddMissingSubSystems(SerializedProperty listProperty, IReadOnlyList<Type> discoveredSubClasses, HashSet<string> existingTypes)
        {
            foreach (Type type in discoveredSubClasses)
            {
                string fullName = type.AssemblyQualifiedName;

                if (existingTypes.Contains(fullName))
                {
                    continue;
                }

                int index = listProperty.arraySize;
                listProperty.InsertArrayElementAtIndex(index);

                SerializedProperty newElement = listProperty.GetArrayElementAtIndex(index);

                InitializeDefault(newElement, type);
            }
        }

        private void InitializeDefault(SerializedProperty element, Type type)
        {
            SerializedProperty subSystemProp = element.FindAutoProperty("SubSystem");
            if (subSystemProp != null)
            {
                SerializedProperty typeProperty = subSystemProp.FindPropertyRelative("assemblyQualifiedTypeName");
                if (typeProperty != null)
                {
                    typeProperty.stringValue = type.AssemblyQualifiedName;
                }
            }

            SerializedProperty enabledProp = element.FindAutoProperty("IsEnable");
            if (enabledProp != null)
            {
                enabledProp.boolValue = false;
            }
        }

        private static void RemoveInvalidSubSystems(SerializedProperty serializedProperty, IReadOnlyList<Type> discoveredSubSystems)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                SerializedProperty element = serializedProperty.GetArrayElementAtIndex(i);

                SerializedProperty subSystemProp = element.FindAutoProperty("SubSystem");

                if (subSystemProp == null)
                {
                    continue;
                }

                SerializedProperty typeProperty = subSystemProp.FindPropertyRelative("assemblyQualifiedTypeName");
                if (typeProperty == null)
                {
                    continue;
                }

                string typeName = typeProperty.stringValue;

                if (discoveredSubSystems.Any(t => t.AssemblyQualifiedName == typeName))
                {
                    continue;
                }

                serializedProperty.DeleteArrayElementAtIndex(i);
            }
        }
        #endregion
    }
}