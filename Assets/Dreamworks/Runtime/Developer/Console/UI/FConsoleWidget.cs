using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    public sealed class FConsoleWidget
    {
        #region Fields
        private const int MaxSuggestions = 8;
        private const int MaxVisibleHistory = 32;
        private const float ToastDuration = 3.5f;
        private const float MiniHeight = 48f;
        private static readonly Color PanelColor = new Color32(12, 15, 18, 238);
        private static readonly Color PromptColor = new Color32(7, 10, 12, 246);
        private static readonly Color ConsoleGreen = new Color32(133, 230, 145, 255);
        private static readonly Color MutedColor = new Color32(160, 174, 184, 255);
        private static readonly Color ErrorColor = new Color32(255, 112, 105, 255);
        private static readonly Color WarningColor = new Color32(255, 211, 106, 255);

        private readonly IConsoleCommandQuery commandQuery;
        private readonly IConsoleCommandOutputBuffer commandBuffer;
        private readonly IConsoleCommandHistory commandHistory;

        private VisualElement root;
        private VisualElement fullPanel;
        private VisualElement suggestionPanel;
        private VisualElement toastPanel;
        private Label toastLabel;
        private TextField inputField;
        private ScrollView outputList;
        private ScrollView historyList;
        private readonly List<FConsoleCommandSuggestion> suggestions = new();
        private EConsoleVisibility currentVisibility = EConsoleVisibility.Hidden;
        private int selectedSuggestionIndex = -1;
        private int historyIndex = -1;
        private string historyDraft = string.Empty;
        private float toastExpiresAt;
        private bool suppressValueChanged;
        #endregion

        #region Events
        public event Action<string> OnCommandEntered;
        #endregion

        #region Constructors
        internal FConsoleWidget(IConsoleCommandQuery commandQuery, IConsoleCommandOutputBuffer commandBuffer, IConsoleCommandHistory commandHistory)
        {
            this.commandQuery = commandQuery;
            this.commandBuffer = commandBuffer;
            this.commandHistory = commandHistory;
        }
        #endregion

        #region Public Methods
        public void Initialize(VisualElement rootElement)
        {
            root = rootElement;

            commandBuffer.OnEntryAdded += HandleOutputAdded;

            BuildInterface();
            RefreshOutput();
            RefreshHistory();
            SetVisibility(EConsoleVisibility.Hidden);
        }

        public void Shutdown()
        {
            commandBuffer.OnEntryAdded -= HandleOutputAdded;

            root?.Clear();

            root = null;
        }

        public void CycleVisibility()
        {
            EConsoleVisibility newVisibility = currentVisibility switch
            {
                EConsoleVisibility.Hidden => EConsoleVisibility.Mini,
                EConsoleVisibility.Mini => EConsoleVisibility.Full,
                _ => EConsoleVisibility.Hidden
            };
            SetVisibility(newVisibility);
        }

        public void SetVisibility(EConsoleVisibility visibility)
        {
            currentVisibility = visibility;
            fullPanel.style.display = visibility == EConsoleVisibility.Full ? DisplayStyle.Flex : DisplayStyle.None;
            root.Q<VisualElement>("console-prompt").style.display = visibility == EConsoleVisibility.Hidden ? DisplayStyle.None : DisplayStyle.Flex;
            suggestionPanel.style.display = visibility == EConsoleVisibility.Hidden ? DisplayStyle.None : DisplayStyle.Flex;

            if (visibility != EConsoleVisibility.Hidden)
            {
                inputField.Focus();
                inputField.cursorIndex = inputField.value.Length;
            }

            RefreshSuggestions();
        }

        public void Tick(float deltaTime)
        {
            if (toastPanel == null || toastPanel.style.display == DisplayStyle.None || Time.unscaledTime < toastExpiresAt)
            {
                return;
            }

            toastPanel.style.display = DisplayStyle.None;
        }
        #endregion

        #region Private Methods
        private void BuildInterface()
        {
            root.Clear();
            root.style.flexGrow = 1;
            root.style.flexDirection = FlexDirection.Column;
            root.style.justifyContent = Justify.FlexEnd;
            root.style.paddingLeft = 0;
            root.style.paddingRight = 0;
            root.style.paddingTop = 0;
            root.style.paddingBottom = 0;
            root.pickingMode = PickingMode.Ignore;

            toastPanel = new VisualElement { name = "console-toast" };
            toastPanel.style.position = Position.Absolute;
            toastPanel.style.left = 14;
            toastPanel.style.top = 14;
            toastPanel.style.maxWidth = Length.Percent(72);
            toastPanel.style.paddingLeft = 12;
            toastPanel.style.paddingRight = 12;
            toastPanel.style.paddingTop = 8;
            toastPanel.style.paddingBottom = 8;
            toastPanel.style.backgroundColor = PanelColor;
            toastPanel.style.borderLeftWidth = 3;
            toastPanel.style.borderLeftColor = ConsoleGreen;
            toastPanel.style.display = DisplayStyle.None;
            toastPanel.pickingMode = PickingMode.Ignore;
            toastLabel = CreateLabel(string.Empty, 15, ConsoleGreen);
            toastLabel.style.whiteSpace = WhiteSpace.Normal;
            toastPanel.Add(toastLabel);
            root.Add(toastPanel);

            fullPanel = new VisualElement { name = "console-full-panel" };
            fullPanel.style.position = Position.Absolute;
            fullPanel.style.left = 0;
            fullPanel.style.top = 0;
            fullPanel.style.bottom = MiniHeight + 4;
            fullPanel.style.width = Length.Percent(100);
            fullPanel.style.paddingLeft = 12;
            fullPanel.style.paddingRight = 12;
            fullPanel.style.paddingTop = 8;
            fullPanel.style.paddingBottom = 8;
            fullPanel.style.backgroundColor = PanelColor;
            fullPanel.style.display = DisplayStyle.None;
            fullPanel.pickingMode = PickingMode.Position;
            root.Add(fullPanel);

            Label title = CreateLabel("DEVELOPER CONSOLE", 12, MutedColor);
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.marginBottom = 5;
            fullPanel.Add(title);

            outputList = new ScrollView(ScrollViewMode.Vertical) { name = "console-output" };
            outputList.style.flexGrow = 1;
            outputList.style.minHeight = 80;
            outputList.style.backgroundColor = new Color(4f / 255f, 6f / 255f, 8f / 255f, 170f / 255f);
            outputList.style.paddingLeft = 8;
            outputList.style.paddingRight = 8;
            outputList.style.paddingTop = 5;
            outputList.style.paddingBottom = 5;
            fullPanel.Add(outputList);

            Label historyTitle = CreateLabel("HISTORY", 11, MutedColor);
            historyTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
            historyTitle.style.marginTop = 6;
            fullPanel.Add(historyTitle);

            historyList = new ScrollView(ScrollViewMode.Vertical) { name = "console-history" };
            historyList.style.height = Length.Percent(28);
            historyList.style.minHeight = 45;
            historyList.style.marginTop = 3;
            fullPanel.Add(historyList);

            suggestionPanel = new VisualElement { name = "console-suggestions" };
            suggestionPanel.style.position = Position.Absolute;
            suggestionPanel.style.left = 12;
            suggestionPanel.style.right = 12;
            suggestionPanel.style.bottom = MiniHeight + 4;
            suggestionPanel.style.maxHeight = Length.Percent(45);
            suggestionPanel.style.backgroundColor = PanelColor;
            suggestionPanel.style.paddingLeft = 8;
            suggestionPanel.style.paddingRight = 8;
            suggestionPanel.style.paddingTop = 5;
            suggestionPanel.style.paddingBottom = 5;
            suggestionPanel.style.display = DisplayStyle.None;
            suggestionPanel.pickingMode = PickingMode.Position;
            root.Add(suggestionPanel);

            VisualElement prompt = new VisualElement { name = "console-prompt" };
            prompt.style.position = Position.Absolute;
            prompt.style.left = 0;
            prompt.style.right = 0;
            prompt.style.bottom = 0;
            prompt.style.height = MiniHeight;
            prompt.style.flexDirection = FlexDirection.Row;
            prompt.style.alignItems = Align.Center;
            prompt.style.paddingLeft = 12;
            prompt.style.paddingRight = 12;
            prompt.style.backgroundColor = PromptColor;
            prompt.style.borderTopWidth = 1;
            prompt.style.borderTopColor = new Color(71f / 255f, 91f / 255f, 78f / 255f, 1f);
            root.Add(prompt);

            Label promptPrefix = CreateLabel(">", 16, ConsoleGreen);
            promptPrefix.style.height = 34;
            promptPrefix.style.unityTextAlign = TextAnchor.MiddleLeft;
            promptPrefix.style.unityFontStyleAndWeight = FontStyle.Bold;
            promptPrefix.style.marginTop = 0;
            promptPrefix.style.marginBottom = 0;
            promptPrefix.style.marginRight = 4;
            prompt.Add(promptPrefix);

            inputField = new TextField { name = "console-command-input", isDelayed = false };
            inputField.style.flexGrow = 1;
            inputField.style.flexDirection = FlexDirection.Row;
            inputField.style.height = 34;
            inputField.style.marginLeft = 0;
            inputField.style.paddingLeft = 0;
            inputField.style.fontSize = 16;
            inputField.style.color = ConsoleGreen;
            inputField.style.backgroundColor = new Color(0f, 0f, 0f, 0f);
            inputField.style.borderLeftWidth = 0;
            inputField.style.borderRightWidth = 0;
            inputField.style.borderTopWidth = 0;
            inputField.style.borderBottomWidth = 0;
            inputField.RegisterValueChangedCallback(HandleInputChanged);
            inputField.RegisterCallback<KeyDownEvent>(HandleInputKeyDown, TrickleDown.TrickleDown);
            prompt.Add(inputField);
            VisualElement inputLabel = inputField.Q<VisualElement>(className: "unity-base-field__label");

            if (inputLabel != null)
            {
                inputLabel.style.display = DisplayStyle.None;
            }

            VisualElement textInputContainer = inputField.Q<VisualElement>(className: "unity-base-field__input");
            if (textInputContainer != null)
            {
                textInputContainer.style.flexGrow = 1;
                textInputContainer.style.minWidth = 0;
                textInputContainer.style.marginLeft = 0;
                textInputContainer.style.paddingLeft = 0;
                textInputContainer.style.marginRight = 0;
                textInputContainer.style.paddingRight = 0;
                textInputContainer.style.alignItems = Align.Center;
                textInputContainer.style.paddingTop = 0;
                textInputContainer.style.paddingBottom = 0;
                textInputContainer.style.marginTop = 0;
                textInputContainer.style.marginBottom = 0;
                textInputContainer.style.backgroundColor = new Color(0f, 0f, 0f, 0f);
                textInputContainer.style.borderLeftWidth = 0;
                textInputContainer.style.borderRightWidth = 0;
                textInputContainer.style.borderTopWidth = 0;
                textInputContainer.style.borderBottomWidth = 0;
            }

            TextElement inputText = inputField.Q<TextElement>(className: "unity-text-input");
            if (inputText != null)
            {
                inputText.style.color = ConsoleGreen;
                inputText.style.unityTextAlign = TextAnchor.MiddleLeft;
                inputText.style.paddingLeft = 0;
                inputText.style.marginLeft = 0;
                inputText.style.paddingRight = 0;
                inputText.style.marginRight = 0;
                inputText.style.paddingTop = 0;
                inputText.style.paddingBottom = 0;
                inputText.style.marginTop = 0;
                inputText.style.marginBottom = 0;
                inputText.style.backgroundColor = new Color(0f, 0f, 0f, 0f);
            }

            root.RegisterCallback<KeyDownEvent>(HandleRootKeyDown);
        }

        private void HandleInputChanged(ChangeEvent<string> changeEvent)
        {
            if (suppressValueChanged)
            {
                return;
            }

            selectedSuggestionIndex = -1;
            historyIndex = -1;
            RefreshSuggestions();
        }

        private void HandleRootKeyDown(KeyDownEvent keyEvent)
        {
            if (keyEvent.keyCode != KeyCode.BackQuote && keyEvent.character != '`')
            {
                return;
            }

            ConsumeKeyEvent(keyEvent);
        }

        private void HandleInputKeyDown(KeyDownEvent keyEvent)
        {
            if (keyEvent.keyCode == KeyCode.BackQuote || keyEvent.character == '`')
            {
                ConsumeKeyEvent(keyEvent);
                return;
            }

            switch (keyEvent.keyCode)
            {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    ExecuteCurrentCommand();
                    ConsumeKeyEvent(keyEvent);
                    break;
                case KeyCode.Tab:
                    CycleSuggestion(1);
                    ConsumeKeyEvent(keyEvent);
                    break;
                case KeyCode.UpArrow:
                    if (suggestions.Count > 0)
                    {
                        CycleSuggestion(-1);
                    }
                    else
                    {
                        NavigateHistory(-1);
                    }

                    ConsumeKeyEvent(keyEvent);
                    break;
                case KeyCode.DownArrow:
                    if (suggestions.Count > 0)
                    {
                        CycleSuggestion(1);
                    }
                    else
                    {
                        NavigateHistory(1);
                    }

                    ConsumeKeyEvent(keyEvent);
                    break;
                case KeyCode.Escape:
                    SetVisibility(EConsoleVisibility.Hidden);
                    ConsumeKeyEvent(keyEvent);
                    break;
            }
        }

        private void ConsumeKeyEvent(KeyDownEvent keyEvent)
        {
            keyEvent.StopImmediatePropagation();

            if (inputField?.panel != null)
            {
                inputField.panel.focusController.IgnoreEvent(keyEvent);
            }
        }

        private void RefreshSuggestions()
        {
            suggestionPanel.Clear();
            suggestions.Clear();
            selectedSuggestionIndex = -1;

            if (currentVisibility == EConsoleVisibility.Hidden || inputField == null || string.IsNullOrWhiteSpace(inputField.value))
            {
                suggestionPanel.style.display = DisplayStyle.None;
                return;
            }

            string trimmedInput = inputField.value.TrimStart();

            if (trimmedInput.Contains(' '))
            {
                suggestionPanel.style.display = DisplayStyle.None;
                return;
            }

            suggestions.AddRange(commandQuery.QuerySuggestions(trimmedInput).Take(MaxSuggestions));

            if (suggestions.Count == 0)
            {
                suggestionPanel.style.display = DisplayStyle.None;
                return;
            }

            suggestionPanel.style.display = DisplayStyle.Flex;
            suggestionPanel.Add(CreateLabel("SUGGESTIONS  |  ↑↓ select  ·  TAB complete", 11, MutedColor));

            for (int i = 0; i < suggestions.Count; ++i)
            {
                AddSuggestionRow(suggestions[i], i);
            }
        }

        private void AddSuggestionRow(FConsoleCommandSuggestion suggestion, int index)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            row.style.paddingTop = 3;
            row.style.paddingBottom = 3;
            row.style.borderBottomWidth = 1;
            row.style.borderBottomColor = new Color(55f / 255f, 66f / 255f, 60f / 255f, 1f);

            Label name = CreateLabel(suggestion.Name, 14, ConsoleGreen);
            name.style.minWidth = 150;
            name.style.unityFontStyleAndWeight = FontStyle.Bold;
            row.Add(name);

            Label description = CreateLabel(suggestion.Description, 13, MutedColor);
            description.style.flexGrow = 1;
            description.style.whiteSpace = WhiteSpace.Normal;
            row.Add(description);

            row.RegisterCallback<ClickEvent>(_ => CompleteSuggestion(index));
            suggestionPanel.Add(row);
        }

        private void CycleSuggestion(int direction)
        {
            if (suggestions.Count == 0)
            {
                return;
            }

            if (selectedSuggestionIndex < 0)
            {
                selectedSuggestionIndex = direction < 0 ? suggestions.Count - 1 : 0;
            }
            else
            {
                selectedSuggestionIndex = (selectedSuggestionIndex + direction + suggestions.Count) % suggestions.Count;
            }

            CompleteSuggestion(selectedSuggestionIndex);
        }

        private void CompleteSuggestion(int index)
        {
            if (index < 0 || index >= suggestions.Count)
            {
                return;
            }

            SetInputValue(suggestions[index].Name + " ");
            suggestionPanel.style.display = DisplayStyle.None;
            inputField.Focus();
            inputField.cursorIndex = inputField.value.Length;
        }

        private void NavigateHistory(int direction)
        {
            IReadOnlyCollection<FConsoleCommandHistoryRecord> entries = commandHistory.HistoryRecords;

            if (entries.Count == 0)
            {
                return;
            }

            if (historyIndex < 0)
            {
                historyDraft = inputField.value;
                historyIndex = entries.Count;
            }

            historyIndex = Mathf.Clamp(historyIndex + direction, 0, entries.Count);
            SetInputValue(historyIndex == entries.Count ? historyDraft : entries.ElementAt(historyIndex).Command);
            RefreshHistory();
            RefreshSuggestions();
            inputField.Focus();
            inputField.cursorIndex = inputField.value.Length;
        }

        private void ExecuteCurrentCommand()
        {
            string command = inputField.value.Trim();

            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            OnCommandEntered?.Invoke(command);

            SetInputValue(string.Empty);
            historyIndex = -1;
            RefreshHistory();
            RefreshSuggestions();

            if (currentVisibility == EConsoleVisibility.Mini)
            {
                SetVisibility(EConsoleVisibility.Hidden);
            }
            else
            {
                inputField.Focus();
            }
        }

        private void SetInputValue(string command)
        {
            suppressValueChanged = true;
            inputField.SetValueWithoutNotify(command);
            suppressValueChanged = false;
        }

        private void RefreshOutput()
        {
            if (outputList == null)
            {
                return;
            }

            outputList.Clear();

            foreach (FConsoleCommandOutputEntry entry in commandBuffer.OutputEntries)
            {
                outputList.Add(CreateOutputRow(entry));
            }

            ScheduleOutputScrollToBottom();
        }

        private void HandleOutputAdded(FConsoleCommandOutputEntry entry)
        {
            if (outputList != null)
            {
                outputList.Add(CreateOutputRow(entry));
                ScheduleOutputScrollToBottom();
            }

            ShowToast(entry);
        }

        private void ScheduleOutputScrollToBottom()
        {
            outputList.schedule.Execute(() =>
            {
                if (outputList == null || outputList.panel == null)
                {
                    return;
                }

                Vector2 scrollOffset = outputList.scrollOffset;
                scrollOffset.y = outputList.verticalScroller.highValue;
                outputList.scrollOffset = scrollOffset;
            });
        }

        private VisualElement CreateOutputRow(FConsoleCommandOutputEntry entry)
        {
            Color color = entry.MessageType switch
            {
                EConsoleCommandOutputType.Error => ErrorColor,
                EConsoleCommandOutputType.Warning => WarningColor,
                EConsoleCommandOutputType.Command => MutedColor,
                _ => ConsoleGreen
            };

            Label row = CreateLabel(entry.Message, 13, color);
            row.style.whiteSpace = WhiteSpace.Normal;
            row.style.marginBottom = 3;
            return row;
        }

        private void RefreshHistory()
        {
            if (historyList == null)
            {
                return;
            }

            historyList.Clear();
            IReadOnlyCollection<FConsoleCommandHistoryRecord> entries = commandHistory.HistoryRecords;
            int startIndex = Mathf.Max(0, entries.Count - MaxVisibleHistory);

            for (int i = startIndex; i < entries.Count; ++i)
            {
                string command = entries.ElementAt(i).Command;
                int selectedHistoryIndex = i;
                bool isSelected = selectedHistoryIndex == historyIndex;
                Label row = CreateLabel(command, 13, isSelected ? Color.white : ConsoleGreen);
                row.style.paddingTop = 3;
                row.style.paddingBottom = 3;
                row.style.paddingLeft = 6;
                row.style.backgroundColor = isSelected ? new Color(42f / 255f, 61f / 255f, 47f / 255f, 1f) : Color.clear;
                row.style.borderLeftWidth = isSelected ? 3 : 0;
                row.style.borderLeftColor = ConsoleGreen;
                row.RegisterCallback<ClickEvent>(_ =>
                {
                    historyIndex = selectedHistoryIndex;
                    SetInputValue(command);
                    RefreshHistory();
                    SetVisibility(EConsoleVisibility.Full);
                    inputField.Focus();
                    inputField.cursorIndex = inputField.value.Length;
                });
                historyList.Add(row);
            }
        }

        private void ShowToast(FConsoleCommandOutputEntry entry)
        {
            toastLabel.text = entry.Message;
            toastLabel.style.color = entry.MessageType switch
            {
                EConsoleCommandOutputType.Error => ErrorColor,
                EConsoleCommandOutputType.Warning => WarningColor,
                _ => ConsoleGreen
            };
            toastPanel.style.borderLeftColor = toastLabel.style.color.value;
            toastPanel.style.display = DisplayStyle.Flex;
            toastExpiresAt = Time.unscaledTime + ToastDuration;
        }

        private Label CreateLabel(string text, int fontSize, Color color)
        {
            Label label = new Label(text);
            label.style.fontSize = fontSize;
            label.style.color = color;
            return label;
        }
        #endregion
    }
}
