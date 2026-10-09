using System;
using System.Collections.Generic;
using System.Globalization;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.Developer.Console;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamMachineGameStudio.DreamWorks.Editor.ConsoleVariables
{
    public sealed class UConsoleVariableRuntimeEditorWindow : EditorWindow
    {
        #region Fields
        private const string MenuPath = "DreamWorks/Console/Variables/Runtime";
        private const int RefreshIntervalMilliseconds = 500;
        private readonly List<RuntimeVariableRow> runtimeRows = new();
        private TextField searchField;
        private Label statusLabel;
        private VisualElement rowsRoot;
        private IVisualElementScheduledItem refreshSchedule;
        private bool previousPlayModeState;
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens the live console variable editor window.
        /// </summary>
        [MenuItem(MenuPath)]
        public static void Open()
        {
            UConsoleVariableRuntimeEditorWindow window = GetWindow<UConsoleVariableRuntimeEditorWindow>();
            window.titleContent = new GUIContent("Console Variables");
            window.minSize = new Vector2(480, 240);
            window.Show();
        }

        /// <summary>
        /// Builds the UI Toolkit interface for searching and editing live console variables.
        /// </summary>
        public void CreateGUI()
        {
            previousPlayModeState = EditorApplication.isPlaying;
            rootVisualElement.Clear();
            rootVisualElement.style.paddingLeft = 8;
            rootVisualElement.style.paddingRight = 8;
            rootVisualElement.style.paddingTop = 8;
            searchField = new TextField("Search");
            searchField.RegisterValueChangedCallback(_ => RebuildRows());
            rootVisualElement.Add(searchField);
            statusLabel = new Label();
            statusLabel.style.marginBottom = 6;
            rootVisualElement.Add(statusLabel);
            rowsRoot = new ScrollView(ScrollViewMode.Vertical);
            rootVisualElement.Add(rowsRoot);
            RebuildRows();
            refreshSchedule?.Pause();
            refreshSchedule = rootVisualElement.schedule.Execute(RefreshRuntimeValues).Every(RefreshIntervalMilliseconds);
        }
        #endregion

        #region Private Methods
        private void OnDisable()
        {
            refreshSchedule?.Pause();
            refreshSchedule = null;
        }

        private void RebuildRows()
        {
            if (rowsRoot == null)
            {
                return;
            }

            runtimeRows.Clear();
            rowsRoot.Clear();
            if (!EditorApplication.isPlaying)
            {
                statusLabel.text = "Enter Play Mode to inspect and edit live console variables.";
                return;
            }

            if (FDeveloperConsole.Instance == null)
            {
                statusLabel.text = "The developer console is not initialized.";

                return;
            }

            string searchText = searchField.value;
            IReadOnlyList<IConsoleVariable> variables = FDeveloperConsole.Instance.GetRegisteredVariables();
            for (int index = 0; index < variables.Count; index++)
            {
                IConsoleVariable variable = variables[index];
                if (variable == null || !MatchesSearch(variable, searchText))
                {
                    continue;
                }

                RuntimeVariableRow row = BuildRuntimeRow(variable);
                runtimeRows.Add(row);
                rowsRoot.Add(row.Root);
            }

            statusLabel.text = runtimeRows.Count == 0 ? "No registered variables match the search." : $"{runtimeRows.Count} variable(s)";
        }

        private RuntimeVariableRow BuildRuntimeRow(IConsoleVariable variable)
        {
            RuntimeVariableRow row = new(variable);
            row.Root.style.borderBottomWidth = 1;
            row.Root.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f, 1f);
            row.Root.style.paddingBottom = 6;
            row.Root.style.paddingTop = 6;
            VisualElement heading = new();
            heading.style.flexDirection = FlexDirection.Row;
            Label nameLabel = new(variable.Name);
            nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            heading.Add(nameLabel);
            Label typeLabel = new($"  {variable.ValueType.Name}{(variable.IsPersistent ? "  Persistent" : string.Empty)}");
            typeLabel.style.opacity = 0.7f;
            heading.Add(typeLabel);
            row.Root.Add(heading);
            row.Root.Add(new Label(variable.Description ?? string.Empty));
            VisualElement valueField = BuildValueField(row);
            valueField.RegisterCallback<FocusInEvent>(_ => row.IsEditing = true);
            valueField.RegisterCallback<FocusOutEvent>(_ => row.IsEditing = false);
            row.Root.Add(valueField);
            row.ErrorLabel = new Label();
            row.ErrorLabel.style.color = new StyleColor(new Color(0.85f, 0.25f, 0.2f, 1f));
            row.Root.Add(row.ErrorLabel);
            return row;
        }

        private VisualElement BuildValueField(RuntimeVariableRow row)
        {
            Type valueType = row.Variable.ValueType;
            object value = row.Variable.GetValue();
            if (valueType == typeof(bool))
            {
                Toggle field = new("Value") { value = value != null && Convert.ToBoolean(value, CultureInfo.InvariantCulture) };
                field.RegisterValueChangedCallback(evt => ApplyRuntimeValue(row, evt.newValue ? "true" : "false"));
                row.RefreshValue = () => field.SetValueWithoutNotify(row.Variable.GetValue() != null && Convert.ToBoolean(row.Variable.GetValue(), CultureInfo.InvariantCulture));
                return field;
            }

            if (valueType == typeof(int))
            {
                IntegerField field = new("Value") { value = value == null ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture) };
                field.RegisterValueChangedCallback(evt => ApplyRuntimeValue(row, evt.newValue.ToString(CultureInfo.InvariantCulture)));
                row.RefreshValue = () => field.SetValueWithoutNotify(row.Variable.GetValue() == null ? 0 : Convert.ToInt32(row.Variable.GetValue(), CultureInfo.InvariantCulture));
                return field;
            }

            if (valueType == typeof(float))
            {
                FloatField field = new("Value") { value = value == null ? 0f : Convert.ToSingle(value, CultureInfo.InvariantCulture) };
                field.RegisterValueChangedCallback(evt => ApplyRuntimeValue(row, evt.newValue.ToString("R", CultureInfo.InvariantCulture)));
                row.RefreshValue = () => field.SetValueWithoutNotify(row.Variable.GetValue() == null ? 0f : Convert.ToSingle(row.Variable.GetValue(), CultureInfo.InvariantCulture));
                return field;
            }

            TextField stringField = new("Value") { value = ToInvariantString(value) };
            stringField.RegisterValueChangedCallback(evt => ApplyRuntimeValue(row, evt.newValue));
            row.RefreshValue = () => stringField.SetValueWithoutNotify(ToInvariantString(row.Variable.GetValue()));
            return stringField;
        }

        private void ApplyRuntimeValue(RuntimeVariableRow row, string value)
        {
            if (!row.Variable.TrySetValue(value))
            {
                row.ErrorLabel.text = "The value was rejected by the runtime variable.";
                row.RefreshValue?.Invoke();
                return;
            }

            row.ErrorLabel.text = string.Empty;
        }

        private void RefreshRuntimeValues()
        {
            bool isPlaying = EditorApplication.isPlaying;
            if (isPlaying != previousPlayModeState)
            {
                previousPlayModeState = isPlaying;
                RebuildRows();
                return;
            }

            if (!isPlaying)
            {
                return;
            }

            if (FDeveloperConsole.Instance == null)
            {
                if (runtimeRows.Count > 0)
                {
                    RebuildRows();
                }

                return;
            }

            if (runtimeRows.Count == 0 || HasVariableSetChanged(FDeveloperConsole.Instance))
            {
                RebuildRows();
                return;
            }

            for (int index = 0; index < runtimeRows.Count; index++)
            {
                RuntimeVariableRow row = runtimeRows[index];
                if (!row.IsEditing)
                {
                    row.RefreshValue?.Invoke();
                    row.ErrorLabel.text = string.Empty;
                }
            }
        }

        private bool HasVariableSetChanged(IDeveloperConsole developerConsole)
        {
            string searchText = searchField.value;
            IReadOnlyList<IConsoleVariable> variables = developerConsole.GetRegisteredVariables();
            int visibleIndex = 0;
            for (int index = 0; index < variables.Count; index++)
            {
                IConsoleVariable variable = variables[index];
                if (variable == null || !MatchesSearch(variable, searchText))
                {
                    continue;
                }

                if (visibleIndex >= runtimeRows.Count || !ReferenceEquals(runtimeRows[visibleIndex].Variable, variable))
                {
                    return true;
                }

                visibleIndex++;
            }

            return visibleIndex != runtimeRows.Count;
        }

        private static bool MatchesSearch(IConsoleVariable variable, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            return variable.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (variable.Description != null && variable.Description.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static string ToInvariantString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            IFormattable formattable = value as IFormattable;
            return formattable == null ? value.ToString() : formattable.ToString(null, CultureInfo.InvariantCulture);
        }
        #endregion

        #region Private Types
        private sealed class RuntimeVariableRow
        {
            #region Fields
            public IConsoleVariable Variable { get; }
            public VisualElement Root { get; } = new();
            public Label ErrorLabel { get; set; }
            public Action RefreshValue { get; set; }
            public bool IsEditing { get; set; }
            #endregion

            #region Constructors
            public RuntimeVariableRow(IConsoleVariable variable)
            {
                Variable = variable;
            }
            #endregion
        }
        #endregion
    }
}
