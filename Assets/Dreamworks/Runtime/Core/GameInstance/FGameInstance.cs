using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.World;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Core.GameInstance
{
    public class FGameInstance : IGameInstance, IDreamWorksObject
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private IGameWorldManager gameWorldManager;

        private readonly TSubclassOf<IGameWorld> gameWorldClass;

        private readonly FGameModeSettings defaultGameModeSettings;

        private ISubSystemCollection<FGameInstanceSubSystem> subSystems;
        #endregion

        #region Constructors
        public FGameInstance(ILogProvider logProvider, TSubclassOf<IGameWorld> gameWorldClass, FGameModeSettings defaultGameModeSettings)
        {
            this.logProvider = logProvider ?? FDefaultLogger.Instance;
            this.gameWorldClass = gameWorldClass;
            this.defaultGameModeSettings = defaultGameModeSettings;

            CreateGameInstanceSubSystemCollection();

            CreateWorldManager();
        }
        #endregion

        #region IGameInstance
        IGameWorldManager IGameInstance.WorldManager => gameWorldManager;

        T IGameInstance.GetSubSystem<T>()
        {
            return subSystems.GetSubSystem<T>();
        }
        #endregion

        #region IDreamWorksObject Implementation
        async Task IDreamWorksObject.InitializeAsync()
        {
            logProvider.Log($"Initializing.");

            await InitializeSubSystemsAsync();

            await InitializeWorldManagerAsync();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            TickSubSystems(frameContext);

            TickWorldManager(frameContext);
        }

        async Task IDreamWorksObject.ShutDownAsync()
        {
            logProvider.Log($"Shutting Down.");

            await ShutDownSubSystemsAsync();

            await ShutDownWorldManagerAsync();

            ClearGameInstanceSubSystemCollection();
        }
        #endregion

        #region Private Methods
        private void CreateGameInstanceSubSystemCollection()
        {
            logProvider.Log($"Creating {nameof(FGameInstanceSubSystemCollection)}.");

            FScopedLogger subSystemLogProvider = new FScopedLogger(new FLogCategory("GameInstanceSubSystem", ELogVerbosity.Display));

            subSystems = new FGameInstanceSubSystemCollection(this, subSystemLogProvider);
        }

        private void ClearGameInstanceSubSystemCollection()
        {
            subSystems.ClearSubSystems();
        }

        private void CreateWorldManager()
        {
            logProvider.Log($"Creating {nameof(FGameWorldManager)}.");

            ILogProvider worldManagerLogProvider = new FScopedLogger(new FLogCategory($"{nameof(FGameWorldManager)}", ELogVerbosity.Display, Color.green));

            gameWorldManager = new FGameWorldManager(worldManagerLogProvider, this, gameWorldClass, defaultGameModeSettings);
        }

        private async Task InitializeWorldManagerAsync()
        {
            await gameWorldManager.InitializeAsync();
        }

        private async Task InitializeSubSystemsAsync()
        {
            await subSystems.InitializeAsync();
        }

        private void TickSubSystems(FFrameContext frameContext)
        {
            subSystems.Tick(frameContext);
        }

        private void TickWorldManager(FFrameContext frameContext)
        {
            if (gameWorldManager == null)
            {
                return;
            }

            gameWorldManager.Tick(frameContext);
        }

        private async Task ShutDownWorldManagerAsync()
        {
            if (gameWorldManager == null)
            {
                return;
            }

            await gameWorldManager.ShutDownAsync();
        }

        private async Task ShutDownSubSystemsAsync()
        {
            await subSystems.ShutDownAsync();
        }
        #endregion
    }
}