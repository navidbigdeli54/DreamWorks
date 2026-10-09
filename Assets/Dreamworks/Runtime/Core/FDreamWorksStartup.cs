using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Developer.Console;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class FDreamWorksStartup
    {
        #region Private Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Startup()
        {
            CreateDeveloperConsoleBootstrapper();

            CreateDreamWorksBootstrapper();
        }

        private static void CreateDeveloperConsoleBootstrapper()
        {
            FConsoleBootstrapper console = new FConsoleBootstrapper();

            console.Initialize();

            Application.quitting += OnApplicationQuite;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
        }

#if UNITY_EDITOR
        private static void OnPlayModeChanged(UnityEditor.PlayModeStateChange change)
        {
            UnityEditor.EditorApplication.playModeStateChanged-= OnPlayModeChanged;

            if (change == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                FConsoleBootstrapper.Instance.ShutDown();
            }
        } 
#endif

        private static void OnApplicationQuite()
        {
            Application.quitting -= OnApplicationQuite;

            FConsoleBootstrapper.Instance.ShutDown();
        }

        private static void CreateDreamWorksBootstrapper()
        {
            new GameObject(nameof(FDreamWorksBootstrapper)).AddComponent<FDreamWorksBootstrapper>();
        }
        #endregion
    }
}