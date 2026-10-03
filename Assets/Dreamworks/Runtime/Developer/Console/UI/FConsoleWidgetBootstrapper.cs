using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    [DefaultExecutionOrder(-10000)]
    public sealed class FConsoleWidgetBootstrapper : IDeveloperConsoleInitializer
    {
        #region Fields
        private static readonly System.Type InputSystemKeyboardType = System.Type.GetType("UnityEngine.InputSystem.Keyboard, Unity.InputSystem");
        private static readonly PropertyInfo InputSystemKeyboardCurrentProperty = InputSystemKeyboardType?.GetProperty("current", BindingFlags.Public | BindingFlags.Static);
        private static readonly PropertyInfo InputSystemBackquoteKeyProperty = InputSystemKeyboardType?.GetProperty("backquoteKey", BindingFlags.Public | BindingFlags.Instance);
        private static readonly System.Type InputSystemTouchscreenType = System.Type.GetType("UnityEngine.InputSystem.Touchscreen, Unity.InputSystem");
        private static readonly PropertyInfo InputSystemTouchscreenCurrentProperty = InputSystemTouchscreenType?.GetProperty("current", BindingFlags.Public | BindingFlags.Static);
        private static readonly PropertyInfo InputSystemTouchesProperty = InputSystemTouchscreenType?.GetProperty("touches", BindingFlags.Public | BindingFlags.Instance);

        private static PropertyInfo isPressedProperty;
        private static PropertyInfo touchPressProperty;
        private static PropertyInfo wasPressedThisFrameProperty;

        private EConsoleVisibility visibility = EConsoleVisibility.Hidden;

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
            runtimePanelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            runtimePanelSettings.name = "RuntimeDeveloperConsolePanelSettings";
            runtimePanelSettings.themeStyleSheet = Resources.Load<ThemeStyleSheet>("DeveloperConsoleTheme");
            runtimePanelSettings.scaleMode = PanelScaleMode.ConstantPhysicalSize;
            runtimePanelSettings.referenceDpi = 96f;
            runtimePanelSettings.fallbackDpi = 96f;

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

            if (runtimePanelSettings != null)
            {
                Object.Destroy(runtimePanelSettings);
            }
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
            object keyboard = InputSystemKeyboardCurrentProperty?.GetValue(null);

            if (keyboard == null)
            {
                return false;
            }

            object key = InputSystemBackquoteKeyProperty?.GetValue(keyboard);

            if (key == null)
            {
                return false;
            }

            wasPressedThisFrameProperty ??= key.GetType().GetProperty("wasPressedThisFrame", BindingFlags.Public | BindingFlags.Instance);
            return wasPressedThisFrameProperty?.GetValue(key) is bool wasPressed && wasPressed;
        }

        private void HandleInputSystemTouch()
        {
            object touchscreen = InputSystemTouchscreenCurrentProperty?.GetValue(null);

            if (touchscreen == null)
            {
                fourFingerGestureConsumed = false;
                return;
            }

            object touchesObject = InputSystemTouchesProperty?.GetValue(touchscreen);

            if (touchesObject is not IEnumerable touches)
            {
                fourFingerGestureConsumed = false;
                return;
            }

            int activeTouchCount = 0;

            foreach (object touch in touches)
            {
                touchPressProperty ??= touch.GetType().GetProperty("press", BindingFlags.Public | BindingFlags.Instance);
                object press = touchPressProperty?.GetValue(touch);

                if (press == null)
                {
                    continue;
                }

                isPressedProperty ??= press.GetType().GetProperty("isPressed", BindingFlags.Public | BindingFlags.Instance);

                if (isPressedProperty?.GetValue(press) is bool isPressed && isPressed)
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
                CycleVisibility();
            }
        }

        private void CycleVisibility()
        {
            switch (visibility)
            {
                case EConsoleVisibility.Hidden:
                    visibility = EConsoleVisibility.Mini;
                    break;
                case EConsoleVisibility.Mini:
                    visibility = EConsoleVisibility.Full;
                    break;
                default:
                    visibility = EConsoleVisibility.Hidden;
                    break;
            }

            ConsoleWidget.SetVisibility(visibility);
        }

        
        #endregion
    }
}
