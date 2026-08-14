using UnityEditor;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Editor
{
    public static class FDreamWorksEditorGUI
    {
        #region Public Methods
        public static void DrawSeparator()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
        }
        #endregion
    }
}