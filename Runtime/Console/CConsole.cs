using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks
{
    public class CConsole : CComponent
    {
        #region Fields
        private Dictionary<string, FCommand> _commands = new Dictionary<string, FCommand>();

        private bool _isActive;

        private const int CONSOLE_HEIGHT = 40;

        private static CConsole _instance;

        private string _inputText;

        private GUISkin _skin;
        #endregion

        #region Properties
        public static CConsole Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(CConsole)).AddComponent<CConsole>();
                }

                return _instance;
            }
        }
        #endregion

        #region Public Methods
        public void Startup()
        {
            _skin = Resources.Load<GUISkin>("console_skin");
        }

        public void AddCommand(string name, FCommand command)
        {
            _commands.Add(name, command);
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
                    e.Use();
                }
                else if (_isActive && e.keyCode == KeyCode.Return)
                {
                    _isActive = false;
                    _inputText = string.Empty;
                    e.Use();
                }
            }

            if (e.keyCode != KeyCode.BackQuote && _isActive)
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
        }
        #endregion
    }
}
