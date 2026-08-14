using System.IO;
using UnityEditor;

namespace DreamMachineGameStudio.DreamWorks.Editor.Wizard.DataAsset
{
    public static class DataAssetCreationMenu
    {
        [MenuItem("Assets/Create/DreamWorks/Data Asset...", false, 1000)]
        private static void CreateDataAsset()
        {
            UDataAssetCreationWizard.Open(GetSelectedFolder());
        }

        private static string GetSelectedFolder()
        {
            if (Selection.activeObject == null)
            {
                return "Assets";
            }

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                return "Assets";
            }

            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            return path.Replace('\\', '/');
        }
    }
}