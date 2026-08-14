using UnityEditor;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Editor
{
    public static class FDreamWorksEditorStyles
    {
        #region Fields
        private static GUIStyle headerLabelInstance;
        private static GUIStyle sectionLabelInstance;
        private static GUIStyle centeredButtonInstance;
        #endregion

        #region Properties
        public static GUIStyle HeaderLabel
        {
            get
            {
                if (headerLabelInstance == null)
                {
                    headerLabelInstance = new GUIStyle(EditorStyles.boldLabel)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 16,
                        fixedHeight = 28
                    };
                }

                return headerLabelInstance;
            }
        }

        public static GUIStyle SectionLabel
        {
            get
            {
                if (sectionLabelInstance == null)
                {
                    sectionLabelInstance = new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = 11
                    };
                }

                return sectionLabelInstance;
            }
        }

        public static GUIStyle CenteredButton
        {
            get
            {
                if (centeredButtonInstance == null)
                {
                    centeredButtonInstance = new GUIStyle(EditorStyles.miniButton)
                    {
                        fixedHeight = 30
                    };
                }

                return centeredButtonInstance;
            }
        }
        #endregion
    }
}