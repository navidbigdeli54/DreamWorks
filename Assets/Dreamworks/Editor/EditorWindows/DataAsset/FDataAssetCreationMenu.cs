using System.IO;
using UnityEditor;

namespace DreamMachineGameStudio.DreamWorks.Editor.DataAsset
{
    public static class FDataAssetCreationMenu
    {
        [MenuItem("Assets/Create/DreamWorks/Data Asset...", false, int.MinValue)]
        private static void CreateDataAsset()
        {
            UDataAssetCreationEditorWindow.Open(GetSelectedFolder());
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