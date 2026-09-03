using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Log;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class FDreamWorksBootstrapper : MonoBehaviour
    {
        #region Fields
        private readonly ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(FDreamWorksBootstrapper), ELogVerbosity.Display, Color.blue));
        #endregion

        #region Properties
        public IGame Game { get; private set; }
        #endregion

        #region MonoBehaviour Methods
        private async void Awake()
        {
            logProvider.Log("Awake!");

            DontDestroyOnLoad(gameObject);

            OverrideSceneManagerAPI();

            LoadDreamWorkSettings();

            LoadSubSystemRegistry();

            await InitializeAsync();
        }

        private void Update()
        {
            Tick();
        }

        private async void OnDestroy()
        {
            await ShutDownAsync();
        }
        #endregion

        #region Private Methods
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

        private async Task InitializeAsync()
        {
            logProvider.Log("Initializing.");

            await CreateAndInitializeGameAsync();
        }

        private async Task CreateAndInitializeGameAsync()
        {
            logProvider.Log("Creating Game.");

            ILogProvider gameLogProvider = new FScopedLogger(new FLogCategory(nameof(FGame), ELogVerbosity.Display, Color.blue));

            Game = new FGame(FDreamWorkSettingsProvider.Settings, gameLogProvider);

            await ((IDreamWorksObject)Game).InitializeAsync();
        }

        private void Tick()
        {
            if (Game == null)
            {
                return;
            }

            FFrameContext context = new(Time.deltaTime, Time.frameCount);

            ((IDreamWorksObject)Game).Tick(context);
        }

        private async Task ShutDownAsync()
        {
            if (Game == null)
            {
                return;
            }

            await ((IDreamWorksObject)Game).ShutDownAsync();
        }
        #endregion
    }
}