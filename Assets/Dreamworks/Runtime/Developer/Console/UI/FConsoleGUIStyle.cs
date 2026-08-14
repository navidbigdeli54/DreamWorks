using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    public sealed class FConsoleGUIStyle
    {
        public GUIStyle WindowStyle { get; }

        public GUIStyle HeaderStyle { get; }

        public GUIStyle OutputStyle { get; }

        public GUIStyle InputStyle { get; }

        public GUIStyle SuggestionStyle { get; }

        public GUIStyle SelectedSuggestionStyle { get; }

        public GUIStyle FooterStyle { get; }

        public Texture2D BackgroundTexture { get; private set; }

        public FConsoleGUIStyle(float scale)
        {
            BackgroundTexture = new Texture2D(1, 1);
            BackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.90f));
            BackgroundTexture.Apply();

            WindowStyle = new GUIStyle(GUI.skin.box);

            HeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = Mathf.RoundToInt(16 * scale)
            };

            OutputStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true,
                wordWrap = false,
                fontSize = Mathf.RoundToInt(14 * scale)
            };

            InputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = Mathf.RoundToInt(14 * scale)
            };

            SuggestionStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(13 * scale)
            };

            FooterStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(12 * scale)
            };

            SelectedSuggestionStyle = new GUIStyle(SuggestionStyle);

            SelectedSuggestionStyle.normal.background = Texture2D.whiteTexture;
        }
    }
}
