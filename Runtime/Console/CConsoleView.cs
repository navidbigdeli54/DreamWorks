using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    public class CConsoleView : CComponent
    {
        #region Fields
        private const int CONSOLE_HEIGHT = 40;

        private bool _isActive;

        private string _inputText;

        private GUISkin _skin;
        #endregion

        #region CComponent Methods
        protected override async Task PreInitializeComponenetAsync()
        {
            await base.PreInitializeComponenetAsync();

            _skin = Resources.Load<GUISkin>("console_skin");

            MakePersistent();
        }
        #endregion

        #region MonoBehaviour Methods
        private void OnGUI()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.BackQuote)
                {
                    _isActive = !_isActive;
                    _inputText = string.Empty;
                }
                else if (_isActive && e.keyCode == KeyCode.Return)
                {
                    _isActive = false;
                    FConsole.Instance.ExecuteCommand(_inputText);
                    _inputText = string.Empty;
                }
            }

            if (_isActive)
            {
                GUILayout.BeginVertical(GUILayout.Width(Screen.width), GUILayout.Height(CONSOLE_HEIGHT));
                GUILayout.Space(Screen.height - CONSOLE_HEIGHT);
                GUILayout.BeginHorizontal();
                GUI.SetNextControlName(nameof(_inputText));
                _inputText = GUILayout.TextField(_inputText, _skin.textField, GUILayout.Height(CONSOLE_HEIGHT));
                GUI.FocusControl(nameof(_inputText));
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            if (e.keyCode == KeyCode.BackQuote)
            {
                _inputText = string.Empty;
            }
        }
        #endregion
    }
}
