using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamMachineGameStudio.DreamWorks.Editor.LogProvider
{
    public sealed class ULogCategoryEditorWindow : EditorWindow
    {
        #region Fields
        private const string MenuPath = "DreamWorks/Log Provider/Categories";
        private const int RefreshIntervalMilliseconds = 500;
        private readonly List<CategoryRow> rows = new();
        private readonly List<string> verbosityNames = new(Enum.GetNames(typeof(ELogVerbosity)));
        private TextField searchField;
        private Label statusLabel;
        private VisualElement rowsRoot;
        private IVisualElementScheduledItem refreshSchedule;
        private bool previousPlayModeState;
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens the log category settings window.
        /// </summary>
        [MenuItem(MenuPath)]
        public static void Open()
        {
            ULogCategoryEditorWindow window = GetWindow<ULogCategoryEditorWindow>();
            window.titleContent = new GUIContent("Log Categories");
            window.minSize = new Vector2(520, 240);
            window.Show();
        }

        /// <summary>
        /// Builds the UIToolkit interface for viewing and editing registered log categories.
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
            rowsRoot.style.flexGrow = 1;
            rootVisualElement.Add(rowsRoot);

            RebuildRows();
            refreshSchedule?.Pause();
            refreshSchedule = rootVisualElement.schedule.Execute(RefreshCategories).Every(RefreshIntervalMilliseconds);
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

            rows.Clear();
            rowsRoot.Clear();
            if (!EditorApplication.isPlaying)
            {
                statusLabel.text = "Enter Play Mode to view and edit registered log categories.";
                return;
            }

            AddTableHeader();
            string searchText = searchField.value;
            string[] categoryNames = FLogCategoryRegistry.GetRegisteredCategoryNames();
            for (int index = 0; index < categoryNames.Length; index++)
            {
                string categoryName = categoryNames[index];
                if (!MatchesSearch(categoryName, searchText))
                {
                    continue;
                }

                if (!FLogCategoryRegistry.TryGetCategorySettings(categoryName, out bool isEnabled, out ELogVerbosity verbosityThreshold))
                {
                    continue;
                }

                CategoryRow row = BuildRow(categoryName, isEnabled, verbosityThreshold);
                rows.Add(row);
                rowsRoot.Add(row.Root);
            }

            statusLabel.text = rows.Count == 0
                ? categoryNames.Length == 0 ? "No log categories have been registered." : "No categories match the search."
                : $"{rows.Count} category row(s)";
        }

        private void AddTableHeader()
        {
            VisualElement header = CreateTableRow();
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.style.paddingBottom = 6;
            header.style.paddingTop = 6;
            header.Add(CreateColumnLabel("Category", 0.56f));
            header.Add(CreateColumnLabel("Enabled", 0.18f));
            header.Add(CreateColumnLabel("Verbosity Threshold", 0.26f));
            rowsRoot.Add(header);
        }

        private CategoryRow BuildRow(string categoryName, bool isEnabled, ELogVerbosity verbosityThreshold)
        {
            CategoryRow row = new(categoryName);
            row.Root = CreateTableRow();
            row.Root.style.borderBottomWidth = 1;
            row.Root.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f, 1f);
            row.Root.style.paddingBottom = 4;
            row.Root.style.paddingTop = 4;

            Label nameLabel = CreateColumnLabel(categoryName, 0.56f);
            nameLabel.tooltip = categoryName;
            row.Root.Add(nameLabel);

            row.EnabledField = new Toggle { value = isEnabled };
            row.EnabledField.style.flexBasis = new Length(18, LengthUnit.Percent);
            row.EnabledField.style.flexGrow = 0;
            row.EnabledField.RegisterValueChangedCallback(evt => SetEnabled(row, evt.newValue));
            row.Root.Add(row.EnabledField);

            int verbosityIndex = verbosityNames.IndexOf(verbosityThreshold.ToString());
            row.VerbosityField = new DropdownField(string.Empty, verbosityNames, Math.Max(verbosityIndex, 0));
            row.VerbosityField.style.flexBasis = new Length(26, LengthUnit.Percent);
            row.VerbosityField.style.flexGrow = 0;
            row.VerbosityField.RegisterValueChangedCallback(evt => SetVerbosity(row, evt.newValue));
            row.Root.Add(row.VerbosityField);
            return row;
        }

        private void SetEnabled(CategoryRow row, bool isEnabled)
        {
            try
            {
                FLogCategoryRegistry.SetCategoryEnabled(row.CategoryName, isEnabled);
            }
            catch (Exception exception)
            {
                statusLabel.text = exception.Message;
                RefreshRow(row);
            }
        }

        private void SetVerbosity(CategoryRow row, string verbosityName)
        {
            if (!Enum.TryParse(verbosityName, out ELogVerbosity verbosityThreshold))
            {
                RefreshRow(row);
                return;
            }

            try
            {
                FLogCategoryRegistry.SetCategoryVerbosity(row.CategoryName, verbosityThreshold);
            }
            catch (Exception exception)
            {
                statusLabel.text = exception.Message;
                RefreshRow(row);
            }
        }

        private void RefreshCategories()
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

            if (HasCategorySetChanged())
            {
                RebuildRows();
                return;
            }

            for (int index = 0; index < rows.Count; index++)
            {
                RefreshRow(rows[index]);
            }
        }

        private bool HasCategorySetChanged()
        {
            string searchText = searchField.value;
            string[] categoryNames = FLogCategoryRegistry.GetRegisteredCategoryNames();
            int visibleIndex = 0;
            for (int index = 0; index < categoryNames.Length; index++)
            {
                string categoryName = categoryNames[index];
                if (!MatchesSearch(categoryName, searchText))
                {
                    continue;
                }

                if (visibleIndex >= rows.Count || !string.Equals(rows[visibleIndex].CategoryName, categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                visibleIndex++;
            }

            return visibleIndex != rows.Count;
        }

        private static void RefreshRow(CategoryRow row)
        {
            if (FLogCategoryRegistry.TryGetCategorySettings(row.CategoryName, out bool isEnabled, out ELogVerbosity verbosityThreshold))
            {
                row.EnabledField.SetValueWithoutNotify(isEnabled);
                row.VerbosityField.SetValueWithoutNotify(verbosityThreshold.ToString());
            }
        }

        private static VisualElement CreateTableRow()
        {
            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            return row;
        }

        private static Label CreateColumnLabel(string text, float widthPercent)
        {
            Label label = new(text);
            label.style.flexBasis = new Length(widthPercent * 100, LengthUnit.Percent);
            label.style.flexGrow = 0;
            label.style.flexShrink = 1;
            return label;
        }

        private static bool MatchesSearch(string categoryName, string searchText)
        {
            return string.IsNullOrWhiteSpace(searchText) || categoryName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        #endregion

        #region Private Types
        private sealed class CategoryRow
        {
            #region Properties
            public string CategoryName { get; }
            public VisualElement Root { get; set; }
            public Toggle EnabledField { get; set; }
            public DropdownField VerbosityField { get; set; }
            #endregion

            #region Constructors
            public CategoryRow(string categoryName)
            {
                CategoryName = categoryName;
            }
            #endregion
        }
        #endregion
    }
}
