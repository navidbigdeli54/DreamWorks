using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    [DefaultExecutionOrder(-10000)]
    public sealed class FConsoleWidgetBootstrapper : IDeveloperConsoleInitializer
    {
        #region Fields
        private const string PanelSettingsResourcePath = "DeveloperConsolePanelSettings";

        private const string ThemeResourcePath = "DeveloperConsoleTheme";

        private readonly ILogProvider logProvider;

        private UIDocument uiDocument;

        private GameObject documentObject;

        private PanelSettings runtimePanelSettings;

        private bool fourFingerGestureConsumed;
        #endregion

        #region Properties
        internal FConsoleWidget ConsoleWidget { get; }
        #endregion

        #region Constructors
        internal FConsoleWidgetBootstrapper(ILogProvider logProvider, IConsoleCommandQuery commandQuery, IConsoleCommandOutputBuffer commandBuffer, IConsoleCommandHistory commandHistory)
        {
            this.logProvider = logProvider;

            ConsoleWidget = new FConsoleWidget(commandQuery, commandBuffer, commandHistory);
        }
        #endregion

        #region IDeveloperConsoleInitializer Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            runtimePanelSettings = Resources.Load<PanelSettings>(PanelSettingsResourcePath);
            if (runtimePanelSettings == null)
            {
                logProvider.LogFatal($"Developer console panel settings were not found at Resources/{PanelSettingsResourcePath}; the console UI cannot be created.");

                throw new System.InvalidOperationException("DeveloperConsolePanelSettings is missing from a Resources folder.");
            }

            ThemeStyleSheet themeStyleSheet = Resources.Load<ThemeStyleSheet>(ThemeResourcePath);
            if (themeStyleSheet == null)
            {
                logProvider.LogWarning($"Developer console theme was not found at Resources/{ThemeResourcePath}; the console will render with the panel's default theme.", runtimePanelSettings);
            }

            runtimePanelSettings.themeStyleSheet = themeStyleSheet;

            documentObject = new GameObject("DeveloperConsoleUIDocument");
            documentObject.SetActive(false);
            Object.DontDestroyOnLoad(documentObject);

            uiDocument = documentObject.AddComponent<UIDocument>();
            uiDocument.panelSettings = runtimePanelSettings;
            uiDocument.sortingOrder = 1000;
            documentObject.SetActive(true);

            ConsoleWidget.Initialize(uiDocument.rootVisualElement);

            logProvider.LogMessage($"Developer console UI created (sorting order {uiDocument.sortingOrder}).", documentObject);
        }


        void IDeveloperConsoleInitializer.ShutDown()
        {
            ConsoleWidget.Shutdown();

            if (documentObject != null)
            {
                Object.Destroy(documentObject);
            }

            runtimePanelSettings = null;

            logProvider.LogVerbose("Developer console UI destroyed.");
        }
        #endregion

        #region Public Methods
        public void Tick(float deltaTime)
        {
            HandleToggleInput();
            ConsoleWidget.Tick(deltaTime);
        }
        #endregion

        #region Private Methods
        private void HandleToggleInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (WasInputSystemBackquotePressed())
            {
                CycleVisibility();
            }

            HandleInputSystemTouch();
#elif ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                CycleVisibility();
            }

            HandleLegacyTouch();
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private bool WasInputSystemBackquotePressed()
        {
            return Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame;
        }

        private void HandleInputSystemTouch()
        {
            Touchscreen touchscreen = Touchscreen.current;
            if (touchscreen == null)
            {
                HandleFourFingerState(0);
                return;
            }

            int activeTouchCount = 0;
            var touchControls = touchscreen.touches;
            for (int i = 0; i < touchControls.Count; i++)
            {
                if (touchControls[i].press.isPressed)
                {
                    activeTouchCount++;
                }
            }

            HandleFourFingerState(activeTouchCount);
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        private void HandleLegacyTouch()
        {
            HandleFourFingerState(Input.touchCount);
        }
#endif

        private void HandleFourFingerState(int activeTouchCount)
        {
            if (activeTouchCount < 4)
            {
                fourFingerGestureConsumed = false;
                return;
            }

            if (!fourFingerGestureConsumed)
            {
                fourFingerGestureConsumed = true;

                logProvider.LogVerbose($"Four-finger gesture detected ({activeTouchCount} touches); toggling full console visibility.");

                ConsoleWidget.ToggleFullVisibility();
            }
        }

        private void CycleVisibility()
        {
            logProvider.LogVerbose("Console toggle key pressed; cycling console visibility.");

            ConsoleWidget.CycleVisibility();
        }
        #endregion
    }
}
