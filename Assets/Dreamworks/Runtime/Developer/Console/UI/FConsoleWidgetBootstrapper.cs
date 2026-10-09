using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    [DefaultExecutionOrder(-10000)]
    public sealed class FConsoleWidgetBootstrapper : IDeveloperConsoleInitializer
    {
        #region Fields
        private UIDocument uiDocument;

        private GameObject documentObject;

        private PanelSettings runtimePanelSettings;

        private bool fourFingerGestureConsumed;
        #endregion

        #region Properties
        internal FConsoleWidget ConsoleWidget { get; }
        #endregion

        #region Constructors
        internal FConsoleWidgetBootstrapper(IConsoleCommandQuery commandQuery, IConsoleCommandOutputBuffer commandBuffer, IConsoleCommandHistory commandHistory)
        {
            ConsoleWidget = new FConsoleWidget(commandQuery, commandBuffer, commandHistory);
        }
        #endregion

        #region IDeveloperConsoleInitializer Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            runtimePanelSettings = Resources.Load<PanelSettings>("DeveloperConsolePanelSettings");
            if (runtimePanelSettings == null)
            {
                throw new System.InvalidOperationException("DeveloperConsolePanelSettings is missing from a Resources folder.");
            }

            runtimePanelSettings.themeStyleSheet = Resources.Load<ThemeStyleSheet>("DeveloperConsoleTheme");

            documentObject = new GameObject("DeveloperConsoleUIDocument");
            documentObject.SetActive(false);
            Object.DontDestroyOnLoad(documentObject);

            uiDocument = documentObject.AddComponent<UIDocument>();
            uiDocument.panelSettings = runtimePanelSettings;
            uiDocument.sortingOrder = 1000;
            documentObject.SetActive(true);

            ConsoleWidget.Initialize(uiDocument.rootVisualElement);
        }


        void IDeveloperConsoleInitializer.ShutDown()
        {
            ConsoleWidget.Shutdown();

            if (documentObject != null)
            {
                Object.Destroy(documentObject);
            }

            runtimePanelSettings = null;
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
                ConsoleWidget.ToggleFullVisibility();
            }
        }

        private void CycleVisibility()
        {
            ConsoleWidget.CycleVisibility();
        }
        #endregion
    }
}
