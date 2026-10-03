using UnityEngine;
using UnityEditor;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Editor.ConsoleVariables
{
    public static class FConsoleVariableDefinitionRepositoryMenu
    {
        #region Fields
        private const string ResourceFolderPath = "Assets/Resources";

        private const string DreamWorksFolderName = "DreamWorks";

        private const string MenuPath = "DreamWorks/Console/Variables/Repository";

        private const string RepositoryAssetPath = "Assets/Resources/DreamWorks/DA_ConsoleVariableDefinitions.asset";
        #endregion

        #region Private Methods
        [MenuItem(MenuPath)]
        private static void SelectOrCreateRepository()
        {
            EnsureRepositoryFolder();

            Object existingAsset = AssetDatabase.LoadMainAssetAtPath(RepositoryAssetPath);
            if (existingAsset == null)
            {
                existingAsset = ScriptableObject.CreateInstance<FConsoleVariableDefinitionRepository>();

                AssetDatabase.CreateAsset(existingAsset, RepositoryAssetPath);

                AssetDatabase.SaveAssets();
            }

            if (existingAsset != null)
            {
                FConsoleVariableDefinitionRepository repository = existingAsset as FConsoleVariableDefinitionRepository;
                if (repository == null)
                {
                    EditorUtility.DisplayDialog("Console Variable Repository", $"An asset already exists at {RepositoryAssetPath}, but it is not a console variable definition repository.", "OK");

                    return;
                }
            }

            SelectAsset(existingAsset);
        }

        private static void EnsureRepositoryFolder()
        {
            if (!AssetDatabase.IsValidFolder(ResourceFolderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string dreamWorksFolderPath = $"{ResourceFolderPath}/{DreamWorksFolderName}";
            if (!AssetDatabase.IsValidFolder(dreamWorksFolderPath))
            {
                AssetDatabase.CreateFolder(ResourceFolderPath, DreamWorksFolderName);
            }
        }

        private static void SelectAsset(Object repository)
        {
            Selection.activeObject = repository;

            EditorGUIUtility.PingObject(repository);
        }
        #endregion
    }
}
