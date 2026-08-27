using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.World.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    public class FGameWorld : IGameWorld, IDreamWorksObject
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly FGameModeSettings defaultGameModeSettings;

        private readonly List<Scene> scenes = new();

        private readonly List<Scene> pendingScenes = new();

        private TSubclassOf<IGameMode> gameModeOverrideClass;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;

        public bool HasBegunPlay { get; private set; } = false;

        public bool IsShuttingDown { get; private set; } = false;

        public Scene PrimaryScene { get; private set; }

        public IGameInstance GameInstance { get; }

        public IGameMode GameMode { get; private set; }

        public FTickManager TickManager { get; private set; }

        public FSpawnManager SpawnManager { get; private set; }

        public FComponentManager ComponentManager { get; private set; }

        public ISubSystemCollection<FGameWorldSubSystem> SubSystems { get; private set; }
        #endregion

        #region Constructors
        public FGameWorld(ILogProvider logProvider, IGameInstance gameInstance, FGameModeSettings defaultGameModeSettings)
        {
            GameInstance = gameInstance;
            this.logProvider = logProvider;
            this.defaultGameModeSettings = defaultGameModeSettings;

            CreateTickManager();

            CreateComponentManager(TickManager);

            CreateSpawnManager(ComponentManager);

            CreateGameWorldSubSystemCollection();
        }
        #endregion

        #region IGameWorld Implementation
        TSubSystem IGameWorld.GetSubSystem<TSubSystem>()
        {
            return SubSystems.GetSubSystem<TSubSystem>();
        }

        GameObject IGameWorld.SpawnGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return SpawnManager.SpawnGameObject(prefab, position, rotation);
        }

        TComponent IGameWorld.SpawnGameObject<TComponent>(TComponent prefab, Vector3 position, Quaternion rotation)
        {
            GameObject spawnedGameObject = SpawnManager.SpawnGameObject(prefab, position, rotation);

            return spawnedGameObject.GetComponent<TComponent>();
        }

        IReadOnlyList<TComponent> IGameWorld.FindComponents<TComponent>()
        {
            return ComponentManager.FindComponentsOf<TComponent>();
        }

        TComponent IGameWorld.FindComponent<TComponent>()
        {
            return ComponentManager.FindComponent<TComponent>();
        }

        void IGameWorld.Destroy(GameObject gameObject)
        {
            SpawnManager.Destroy(gameObject);
        }
        #endregion

        #region IDreamWorksObject Implementation
        async Task IDreamWorksObject.InitializeAsync()
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("GameWorld is already initialized.");
            }

            await InternalInitializeAsync();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            if (!HasBegunPlay)
            {
                return;
            }

            TickSubSystems(frameContext);

            InternalTick(frameContext);
        }

        async Task IDreamWorksObject.ShutDownAsync()
        {
            if (!IsInitialized)
            {
                return;
            }

            await InternalShutDownAsync();
        }
        #endregion

        #region IGameWorld Implementation
        bool IGameWorld.HasAnyScene()
        {
            return scenes.Count > 0;
        }

        bool IGameWorld.ContainsScene(Scene scene)
        {
            return scenes.Contains(scene);
        }

        void IGameWorld.AddScene(Scene scene)
        {
            if (!scene.IsValid())
            {
                throw new ArgumentException("Invalid scene.", nameof(scene));
            }

            if (scenes.Contains(scene))
            {
                return;
            }

            if (!PrimaryScene.IsValid())
            {
                PrimaryScene = scene;
            }

            if (!IsInitialized)
            {
                pendingScenes.Add(scene);

                return;
            }

            scenes.Add(scene);

            logProvider.Log($"Scene {scene.name} added to world.");

            ComponentManager.RegisterScene(scene);

            OnSceneAdded(scene);
        }

        void IGameWorld.RemoveScene(Scene scene)
        {
            if (!scenes.Remove(scene))
            {
                return;
            }

            if (scene == PrimaryScene)
            {
                PrimaryScene = scenes.Count > 0 ? scenes[0] : default;
            }

            logProvider.Log($"Scene {scene.name} removed from world.");

            ComponentManager.UnregisterScene(scene);

            OnSceneRemoved(scene);
        }

        #endregion

        #region Public Methods
        public void SetGameModeOverride(TSubclassOf<IGameMode> gameModeClass)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Cannot change GameMode after world initialization.");
            }

            gameModeOverrideClass = gameModeClass;
        }
        #endregion

        #region Protected Methods
        protected virtual void OnCreated()
        {

        }

        protected virtual void OnDestroyed()
        {

        }

        protected virtual void OnSceneAdded(Scene scene)
        {

        }

        protected virtual void OnSceneRemoved(Scene scene)
        {

        }
        #endregion

        #region Private Methods
        private void CreateTickManager()
        {
            FScopedLogger scopedLogger = new FScopedLogger(new FLogCategory(nameof(FTickManager), ELogVerbosity.Display));

            TickManager = new FTickManager(scopedLogger);
        }

        private void CreateComponentManager(FTickManager tickManager)
        {
            FScopedLogger scopedLogger = new FScopedLogger(new FLogCategory(nameof(FComponentManager), ELogVerbosity.Display));

            ComponentManager = new FComponentManager(this, tickManager, logProvider);
        }

        private void CreateSpawnManager(FComponentManager componentManager)
        {
            FScopedLogger scopedLogger = new FScopedLogger(new FLogCategory(nameof(FSpawnManager), ELogVerbosity.Display));

            SpawnManager = new FSpawnManager(this, componentManager);
        }

        private void CreateGameWorldSubSystemCollection()
        {
            FScopedLogger scopedLogger = new FScopedLogger(new FLogCategory("GameWorldSubSystem", ELogVerbosity.Display));

            SubSystems = new FGameWorldSubSystemCollection(this, scopedLogger);
        }

        private void ClearGameWorldSubSystemCollection()
        {
            SubSystems.ClearSubSystems();
        }

        private async Task InternalInitializeAsync()
        {
            logProvider.Log("Initializing.");

            await SetupGameMode();

            await InitializeComponentManagerAsync();

            IsInitialized = true;

            ProcessPendingScenes();

            OnCreated();

            await BeginPlayAsync();
        }

        private async Task SetupGameMode()
        {
            GameMode = CreateGameMode();

            if (GameMode == null)
            {
                throw new InvalidOperationException($"Failed to create GameMode");
            }

            await InitializeGameModeAsync();
        }

        private IGameMode CreateGameMode()
        {
            FGameModeSettings gameModeSettings = ResolveGameModeSettings();

            if (gameModeSettings == null)
            {
                throw new InvalidOperationException("Failed to resolve a GameMode class.");
            }

            logProvider.Log($"Creating GameMode: {gameModeSettings.GameModeClass.Type.Name}.");

            object[] constructorArguments =
            {
                GameInstance,
                this,
                gameModeSettings,
                new FScopedLogger(new FLogCategory(gameModeSettings.GameModeClass.Type.Name, ELogVerbosity.Display))
            };

            return gameModeSettings.GameModeClass.Construct(constructorArguments);
        }

        private FGameModeSettings ResolveGameModeSettings()
        {
            if (gameModeOverrideClass != null)
            {
                logProvider.Log($"GameMode is overriden by {gameModeOverrideClass.Type.Name}!");

                return defaultGameModeSettings;
            }

            if (PrimaryScene.IsValid())
            {
                CGameWorldSettings worldSettings = FindWorldSettings(PrimaryScene);

                if (worldSettings != null && worldSettings.GameModeSettingsOverride != null)
                {
                    logProvider.Log($"Found {worldSettings.GameModeSettingsOverride} GameModeSettingsOverride!");

                    return worldSettings.GameModeSettingsOverride;
                }
            }

            return defaultGameModeSettings;
        }

        private CGameWorldSettings FindWorldSettings(Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();

            for (int i = 0; i < rootObjects.Length; ++i)
            {
                CGameWorldSettings worldSettings = rootObjects[i].GetComponentInChildren<CGameWorldSettings>(true);

                if (worldSettings != null)
                {
                    return worldSettings;
                }
            }

            return null;
        }

        private async Task InitializeGameModeAsync()
        {
            await GameMode.InitGameAsync(this, PrimaryScene.name);
        }

        private async Task InitializeComponentManagerAsync()
        {
            await ComponentManager.InitializeAsync();
        }

        private async Task BeginPlayAsync()
        {
            logProvider.Log("BeginPlay.");

            await BeginPlayGameModeAsync();

            await BeginPlayComponentManagerAsync();

            HasBegunPlay = true;
        }

        private async Task BeginPlayGameModeAsync()
        {
            await GameMode.StartPlayAsync();
        }

        private async Task BeginPlayComponentManagerAsync()
        {
            await ComponentManager.BeginPlayAsync();
        }

        private void TickSubSystems(FFrameContext frameContext)
        {
            SubSystems.Tick(frameContext);
        }

        private void InternalTick(FFrameContext frameContext)
        {
            GameMode.Tick(frameContext.DeltaTime);

            TickManager.Tick(frameContext);
        }

        private async Task InternalShutDownAsync()
        {
            logProvider.Log("Shutdown GameWorld ...");

            IsShuttingDown = true;

            await InternalUninitializeAsync();

            ClearGameWorldSubSystemCollection();

            IsShuttingDown = false;
        }

        private async Task InternalUninitializeAsync()
        {
            if (HasBegunPlay)
            {
                await ComponentManager.EndPlayAsync();
            }

            await ComponentManager.UninitializeAsync();

            await GameMode.EndPlayAsync();

            OnDestroyed();

            Dispose();
        }

        private void Dispose()
        {
            scenes.Clear();

            GameMode = null;

            PrimaryScene = default;

            IsInitialized = false;

            HasBegunPlay = false;
        }

        private void ProcessPendingScenes()
        {
            for (int i = 0; i < pendingScenes.Count; ++i)
            {
                ((IGameWorld)this).AddScene(pendingScenes[i]);
            }

            pendingScenes.Clear();
        }
        #endregion
    }
}