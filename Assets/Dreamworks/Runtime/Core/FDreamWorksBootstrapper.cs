using UnityEngine;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class FDreamWorksBootstrapper : MonoBehaviour
    {
        #region Properties
        public IGame Game { get; private set; }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            Internal_Initialize();
        }

        public void Tick()
        {
            Internal_Tick();
        }

        public void ShutDown()
        {
            Internal_ShutDown();
        }
        #endregion

        #region Private Methods
        private void Internal_Initialize()
        {
            DontDestroyOnLoad(gameObject);

            OverrideSceneManagerAPI();

            LoadDreamWorkSettings();

            LoadSubSystemRegistry();

            CreateAndInitializeGame();
        }

        private void OverrideSceneManagerAPI()
        {
            SceneManagerAPI.overrideAPI = new DreamWorkSceneManagerAPI();
        }

        private void LoadDreamWorkSettings()
        {
            FDreamWorkSettingsProvider.Load();
        }

        private void LoadSubSystemRegistry()
        {
            FSubSystemRegisteryProvider.Load();
        }

        private void CreateAndInitializeGame()
        {
            ILogProvider gameLogProvider = new FScopedLogProvider(new FLogCategory(nameof(FGame), ELogVerbosity.Display, Color.blue));

            Game = new FGame(FDreamWorkSettingsProvider.Settings, gameLogProvider);

            ((IDreamWorksObject)Game).Initialize();
        }

        private void Internal_Tick()
        {
            if (Game == null)
            {
                return;
            }

            FFrameContext context = new(Time.deltaTime, Time.frameCount);

            FConsoleBootstrapper.Instance.Tick(context);

            ((IDreamWorksObject)Game).Tick(context);
        }

        private void Internal_ShutDown()
        {
            if (Game == null)
            {
                return;
            }

            ((IDreamWorksObject)Game).ShutDown();
        }
        #endregion
    }
}