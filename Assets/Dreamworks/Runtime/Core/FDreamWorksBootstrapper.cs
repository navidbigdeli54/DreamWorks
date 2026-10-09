using UnityEngine;
using System.Threading.Tasks;
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

        #region MonoBehaviour Methods
        private async void Awake()
        {
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
            await CreateAndInitializeGameAsync();
        }

        private async Task CreateAndInitializeGameAsync()
        {
            ILogProvider gameLogProvider = new FScopedLogProvider(new FLogCategory(nameof(FGame), ELogVerbosity.Display, Color.blue));

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

            FConsoleBootstrapper.Instance.Tick(context);

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