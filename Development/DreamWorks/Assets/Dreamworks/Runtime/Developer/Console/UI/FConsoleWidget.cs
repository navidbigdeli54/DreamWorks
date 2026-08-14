using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Search;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    public sealed class FConsoleWidget
    {
        #region Fields
        private const string InputControlName = "DreamWorksConsoleInput";

        private FConsoleSubSystem consoleSubSystem;

        private string inputBuffer = string.Empty;

        private Vector2 outputScrollPosition;

        private bool requestFocus;

        private bool autoScroll = true;

        private int selectedSuggestionIndex = -1;

        private FConsoleGUIStyle consoleStyle;

        private EConsoleVisibility currentVisibility;

        private bool suppressKeyboardInput;
        #endregion

        #region Public Methods
        public void Initialize(FConsoleSubSystem subSystem)
        {
            consoleSubSystem = subSystem;

            if (consoleSubSystem.OutputBuffer != null)
            {
                consoleSubSystem.OutputBuffer.OnEntryAdded += OnOutputAdded;
            }

            requestFocus = true;
        }

        public void SetVisibility(EConsoleVisibility visibility)
        {
            currentVisibility = visibility;

            requestFocus = true;

            suppressKeyboardInput = true;
        }

        public void OnGUI()
        {
            if (consoleSubSystem == null)
            {
                return;
            }

            if (currentVisibility == EConsoleVisibility.Hidden)
            {
                return;
            }

            if (consoleStyle == null)
            {
                CreateStyles();
            }

            Matrix4x4 previousMatrix = GUI.matrix;

            GUI.matrix = Matrix4x4.Scale(Vector3.one * GetDPIScale());

            switch (currentVisibility)
            {
                case EConsoleVisibility.Mini:
                    DrawSingleLine();
                    break;

                case EConsoleVisibility.Full:
                    DrawFullConsole();
                    break;
            }

            GUI.matrix = previousMatrix;
        }

        public void Shutdown()
        {
            if (consoleSubSystem?.OutputBuffer != null)
            {
                consoleSubSystem.OutputBuffer.OnEntryAdded -= OnOutputAdded;
            }

            consoleSubSystem = null;
        }
        #endregion

        #region Private Methods
        private void CreateStyles()
        {
            consoleStyle = new FConsoleGUIStyle(GetDPIScale());
        }

        private float GetDPIScale()
        {
            float dpi = Screen.dpi;

            if (dpi <= 0)
            {
                return 1f;
            }

            return Mathf.Max(1f, dpi / 96f);
        }

        private void DrawSingleLine()
        {
            float height = Scale(42);

            Rect area = new Rect(0, (Screen.height / GetDPIScale()) - height, Screen.width / GetDPIScale(), height);

            DrawBackground(area);

            GUILayout.BeginArea(area);

            DrawInput();

            DrawSuggestions();

            GUILayout.EndArea();
        }

        private int Scale(int value)
        {
            return Mathf.RoundToInt(value * GetDPIScale());
        }

        private void DrawBackground(Rect rect)
        {
            GUI.DrawTexture(rect, consoleStyle.BackgroundTexture, ScaleMode.StretchToFill);
        }

        private void DrawInput()
        {
            HandleInputEvents();

            GUI.SetNextControlName(InputControlName);

            string newValue = GUILayout.TextField(inputBuffer, consoleStyle.InputStyle, GUILayout.Height(Scale(28)));

            if (!string.Equals(newValue, inputBuffer))
            {
                inputBuffer = newValue;
                selectedSuggestionIndex = -1;
            }

            if (requestFocus)
            {
                GUI.FocusControl(InputControlName);
                requestFocus = false;
            }
        }

        private void HandleInputEvents()
        {
            if (suppressKeyboardInput)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    suppressKeyboardInput = false;
                }

                return;
            }

            Event currentEvent = Event.current;

            if (currentEvent.type != EventType.KeyDown)
            {
                return;
            }

            switch (currentEvent.keyCode)
            {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    ExecuteCurrentCommand();
                    currentEvent.Use();
                    break;

                case KeyCode.UpArrow:
                    if (HandleSuggestionUp())
                    {
                        currentEvent.Use();
                    }
                    else
                    {
                        NavigateHistoryUp();
                        currentEvent.Use();
                    }
                    break;

                case KeyCode.DownArrow:
                    if (HandleSuggestionDown())
                    {
                        currentEvent.Use();
                    }
                    else
                    {
                        NavigateHistoryDown();
                        currentEvent.Use();
                    }
                    break;

                case KeyCode.Tab:
                    AutoComplete();
                    currentEvent.Use();
                    break;
            }
        }

        private void ExecuteCurrentCommand()
        {
            if (string.IsNullOrWhiteSpace(inputBuffer))
            {
                return;
            }

            string command = inputBuffer.Trim();

            consoleSubSystem.ConsoleManager.Execute(command);

            inputBuffer = string.Empty;

            selectedSuggestionIndex = -1;

            requestFocus = true;

            if (currentVisibility == EConsoleVisibility.Mini)
            {
                SetVisibility(EConsoleVisibility.Hidden);
            }
        }

        private bool HandleSuggestionUp()
        {
            IReadOnlyList<FConsoleSuggestion> suggestions = consoleSubSystem.ConsoleManager.GetSuggestions(inputBuffer);

            if (suggestions == null || suggestions.Count == 0)
            {
                return false;
            }

            if (selectedSuggestionIndex < 0)
            {
                selectedSuggestionIndex = suggestions.Count - 1;
            }
            else
            {
                selectedSuggestionIndex--;

                if (selectedSuggestionIndex < 0)
                {
                    selectedSuggestionIndex = suggestions.Count - 1;
                }
            }

            inputBuffer = suggestions[selectedSuggestionIndex].Name;

            requestFocus = true;

            return true;
        }

        private bool HandleSuggestionDown()
        {
            IReadOnlyList<FConsoleSuggestion> suggestions = consoleSubSystem.ConsoleManager.GetSuggestions(inputBuffer);

            if (suggestions == null || suggestions.Count == 0)
            {
                return false;
            }

            selectedSuggestionIndex++;

            if (selectedSuggestionIndex >= suggestions.Count)
            {
                selectedSuggestionIndex = 0;
            }

            inputBuffer = suggestions[selectedSuggestionIndex].Name;

            requestFocus = true;

            return true;
        }

        private void NavigateHistoryUp()
        {
            string value = consoleSubSystem.History.GetPrevious();

            if (!string.IsNullOrEmpty(value))
            {
                inputBuffer = value;
            }
        }

        private void NavigateHistoryDown()
        {
            string value = consoleSubSystem.History.GetNext();

            if (!string.IsNullOrEmpty(value))
            {
                inputBuffer = value;
            }
        }

        private void AutoComplete()
        {
            IReadOnlyList<FConsoleSuggestion> suggestions = consoleSubSystem.ConsoleManager.GetSuggestions(inputBuffer);

            if (suggestions == null || suggestions.Count == 0)
            {
                return;
            }

            if (selectedSuggestionIndex < 0)
            {
                selectedSuggestionIndex = 0;
            }
            else
            {
                selectedSuggestionIndex++;

                if (selectedSuggestionIndex >= suggestions.Count)
                {
                    selectedSuggestionIndex = 0;
                }
            }

            inputBuffer = suggestions[selectedSuggestionIndex].Name;

            requestFocus = true;
        }

        private void DrawFullConsole()
        {
            float width = Screen.width / GetDPIScale();
            float height = (Screen.height / GetDPIScale()) * 0.5f;
            Rect area = new Rect(0, 0, width, height);

            DrawBackground(area);

            GUILayout.BeginArea(area);

            DrawOutput();

            GUILayout.Space(4);

            DrawInput();

            DrawSuggestions();

            GUILayout.EndArea();
        }

        private void DrawOutput()
        {
            outputScrollPosition = GUILayout.BeginScrollView(outputScrollPosition, GUILayout.ExpandHeight(true));

            IReadOnlyList<FConsoleOutputEntry> entries = consoleSubSystem.OutputBuffer.Entries;

            for (int i = 0; i < entries.Count; ++i)
            {
                FConsoleOutputEntry entry = entries[i];

                Color previousColor = GUI.color;

                GUI.color = GetColor(entry.MessageType);

                GUILayout.Label(entry.Message, consoleStyle.OutputStyle);

                GUI.color = previousColor;
            }

            GUILayout.EndScrollView();

            if (autoScroll && Event.current.type == EventType.Repaint)
            {
                outputScrollPosition.y = float.MaxValue;
            }
        }

        private void DrawSuggestions()
        {
            if (string.IsNullOrWhiteSpace(inputBuffer))
            {
                return;
            }

            IReadOnlyList<FConsoleSuggestion> suggestions = consoleSubSystem.ConsoleManager.GetSuggestions(inputBuffer);

            if (suggestions == null ||
                suggestions.Count == 0)
            {
                return;
            }

            GUILayout.Space(4);

            GUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < suggestions.Count; ++i)
            {
                GUIStyle style = i == selectedSuggestionIndex ? consoleStyle.SelectedSuggestionStyle : consoleStyle.SuggestionStyle;

                GUILayout.Label($"{suggestions[i].Name} - {suggestions[i].Description}", style);
            }

            GUILayout.EndVertical();
        }

        private void OnOutputAdded(FConsoleOutputEntry entry)
        {
            autoScroll = true;
        }



        private Color GetColor(EConsoleOutputType type)
        {
            return type switch
            {
                EConsoleOutputType.Warning => Color.yellow,
                EConsoleOutputType.Error => Color.red,
                EConsoleOutputType.Command => Color.gray,
                EConsoleOutputType.Log => Color.white,
                _ => Color.white,
            };
        }
        #endregion
    }
}
