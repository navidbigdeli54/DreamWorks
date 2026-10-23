using DreamMachineGameStudio.DreamWorks.Application;

using UnityEngine;
using DreamMachineGameStudio.DreamWorks.World;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;
using DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameInstance
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
        void IDreamWorksObject.Initialize()
        {
            logProvider.Log($"Initializing.");

            InitializeSubSystems();

            InitializeWorldManager();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            TickSubSystems(frameContext);

            TickWorldManager(frameContext);
        }

        void IDreamWorksObject.ShutDown()
        {
            logProvider.Log($"Shutting Down.");

            ShutDownSubSystems();

            ShutDownWorldManager();

            ClearGameInstanceSubSystemCollection();
        }
        #endregion

        #region Private Methods
        private void CreateGameInstanceSubSystemCollection()
        {
            logProvider.Log($"Creating {nameof(FGameInstanceSubSystemCollection)}.");

            FScopedLogProvider subSystemLogProvider = new FScopedLogProvider(new FLogCategory("GameInstanceSubSystem", ELogVerbosity.Display));

            subSystems = new FGameInstanceSubSystemCollection(this, subSystemLogProvider);
        }

        private void ClearGameInstanceSubSystemCollection()
        {
            subSystems.ClearSubSystems();
        }

        private void CreateWorldManager()
        {
            logProvider.Log($"Creating {nameof(FGameWorldManager)}.");

            ILogProvider worldManagerLogProvider = new FScopedLogProvider(new FLogCategory($"{nameof(FGameWorldManager)}", ELogVerbosity.Display, Color.green));

            gameWorldManager = new FGameWorldManager(worldManagerLogProvider, this, gameWorldClass, defaultGameModeSettings);
        }

        private void InitializeWorldManager()
        {
            gameWorldManager.Initialize();
        }

        private void InitializeSubSystems()
        {
            subSystems.Initialize();
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

        private void ShutDownWorldManager()
        {
            if (gameWorldManager == null)
            {
                return;
            }

            gameWorldManager.ShutDown();
        }

        private void ShutDownSubSystems()
        {
            subSystems.ShutDown();
        }
        #endregion
    }
}