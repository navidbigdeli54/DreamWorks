using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.UI
{
    [DefaultExecutionOrder(-10000)]
    public sealed class UConsoleWidgetBootstrapper : MonoBehaviour
    {
        #region Fields
        private const KeyCode ToggleKey = KeyCode.BackQuote;

        private EConsoleVisibility visibility = EConsoleVisibility.Hidden;

        private readonly FConsoleWidget consoleWidget = new FConsoleWidget();
        #endregion

        #region MonoBehaviour Methods
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            HandleKeyboardInput();

            HandleTouchInput();
        }

        private void OnGUI()
        {
            consoleWidget.OnGUI();
        }
        #endregion

        #region Public Methods
        public void Initialize(FConsoleSubSystem consoleSubSystem)
        {
            consoleWidget.Initialize(consoleSubSystem);
        }

        public void ShutDown()
        {
            consoleWidget.Shutdown();
        }
        #endregion

        #region Private Methods
        private void HandleKeyboardInput()
        {
            if (!Input.GetKeyDown(ToggleKey))
            {
                return;
            }

            CycleVisibility();
        }

        private void HandleTouchInput()
        {
            if (!FConsoleTouchInput.IsFourFingerTap())
            {
                return;
            }

            CycleVisibility();
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

            consoleWidget.SetVisibility(visibility);
        }
        #endregion
    }
}
