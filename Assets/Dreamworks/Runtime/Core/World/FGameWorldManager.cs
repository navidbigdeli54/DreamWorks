using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    public class FGameWorldManager : IGameWorldManager
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IGameInstance gameInstance;

        private readonly TSubclassOf<IGameWorld> gameWorldClass;

        private readonly FGameModeSettings defaultGameModeSettings;

        private readonly List<IGameWorld> existedGameWorlds = new();
        #endregion

        #region Events
        public event Action<IGameWorld> OnWorldInitialized;

        public event Action<IGameWorld> OnWorldShutDown;
        #endregion

        #region Constructors
        public FGameWorldManager(ILogProvider logProvider, IGameInstance gameInstance, TSubclassOf<IGameWorld> gameWorldClass, FGameModeSettings defaultGameModeSettings)
        {
            this.logProvider = logProvider ?? FDefaultLogger.Instance;
            this.gameInstance = gameInstance;
            this.gameWorldClass = gameWorldClass;
            this.defaultGameModeSettings = defaultGameModeSettings;
        }
        #endregion

        #region IGameWorldManager Implementation
        IGameWorld IGameWorldManager.GetFirstGameWorld()
        {
            return existedGameWorlds[0];
        }
        #endregion

        #region IDreamWorksObject Implementation
        Task IDreamWorksObject.InitializeAsync()
        {
            logProvider.Log("Initializing.");

            SubscribeToSceneManager();

            return Task.CompletedTask;
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            TickGameWorlds(frameContext);
        }

        async Task IDreamWorksObject.ShutDownAsync()
        {
            logProvider.Log("Shutting Down.");

            UnsubscribeToSceneManager();

            await ShutDownGameWorlds();
        }
        #endregion

        #region Private Methods
        private void SubscribeToSceneManager()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            if (SceneManager.GetActiveScene().isLoaded)
            {
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            logProvider.Log($"Scene {scene.name} is loaded as {(sceneMode == LoadSceneMode.Single ? "Single" : "Additive")}.");

            if (existedGameWorlds.Any(x => x.ContainsScene(scene)))
            {
                logProvider.Log($"Scene {scene.name} is already added to a world.");

                return;
            }

            bool isNewWorld = false;

            IGameWorld gameWorld = ResolveWorldForScene(scene, sceneMode);
            if (gameWorld == null)
            {
                gameWorld = CreateNewGameWorld(gameWorldClass);

                if (gameWorld == null)
                {
                    return;
                }

                existedGameWorlds.Add(gameWorld);

                isNewWorld = true;
            }

            gameWorld.AddScene(scene);

            if (isNewWorld)
            {
                OnWorldInitialized?.Invoke(gameWorld);

                await ((IDreamWorksObject)gameWorld).InitializeAsync();
            }
        }

        private IGameWorld ResolveWorldForScene(Scene scene, LoadSceneMode sceneMode)
        {
            if (sceneMode == LoadSceneMode.Additive)
            {
                return existedGameWorlds.FirstOrDefault();
            }

            return null;
        }

        private IGameWorld CreateNewGameWorld(TSubclassOf<IGameWorld> gameWorldClass)
        {
            ILogProvider worldLogProvider = new FScopedLogger(new FLogCategory(gameWorldClass.Type.Name, ELogVerbosity.Display));

            object[] constructorArguments = { worldLogProvider, gameInstance, defaultGameModeSettings };

            IGameWorld gameWorld = gameWorldClass.Construct(constructorArguments);

            if (gameWorld == null)
            {
                worldLogProvider.LogError("Could not create game world!");

                return null;
            }

            return gameWorld;
        }

        private async void OnSceneUnloaded(Scene scene)
        {
            logProvider.Log($"Scene {scene.name} is unloaded.");

            IGameWorld gameWorld = existedGameWorlds.FirstOrDefault(x => x.ContainsScene(scene));
            if (gameWorld == null)
            {
                return;
            }

            gameWorld.RemoveScene(scene);

            if (!gameWorld.HasAnyScene())
            {
                existedGameWorlds.Remove(gameWorld);

                await ((IDreamWorksObject)gameWorld).ShutDownAsync();

                OnWorldShutDown?.Invoke(gameWorld);
            }
        }

        private void UnsubscribeToSceneManager()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void TickGameWorlds(FFrameContext context)
        {
            for (int i = 0; i < existedGameWorlds.Count; ++i)
            {
                try
                {
                    ((IDreamWorksObject)existedGameWorlds[i]).Tick(context);
                }
                catch (Exception exception)
                {
                    logProvider.LogError(exception.ToString());
                }
            }
        }

        private async Task ShutDownGameWorlds()
        {
            for (int i = existedGameWorlds.Count - 1; i >= 0; --i)
            {
                await ((IDreamWorksObject)existedGameWorlds[i]).ShutDownAsync();
            }

            existedGameWorlds.Clear();
        }
        #endregion
    }
}