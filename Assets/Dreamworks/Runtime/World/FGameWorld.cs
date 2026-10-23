using DreamMachineGameStudio.DreamWorks.Application;

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameFramework;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.World
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

        public bool IsDisposed { get; private set; } = false;

        public Scene PrimaryScene { get; private set; }

        public IGameInstance GameInstance { get; }

        public IGameMode GameMode { get; private set; }

        public FTickManager TickManager { get; private set; }

        public FSpawnManager SpawnManager { get; private set; }

        public FComponentManager ComponentManager { get; private set; }
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
        }
        #endregion

        #region IGameWorld Implementation
        GameObject IGameWorld.SpawnGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return SpawnManager.SpawnGameObject(prefab, position, rotation);
        }

        TComponent IGameWorld.SpawnGameObject<TComponent>(TComponent prefab, Transform transform)
        {
            GameObject spawnedGameObject = SpawnManager.SpawnGameObject(prefab, transform);

            return spawnedGameObject.GetComponent<TComponent>();
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
        void IDreamWorksObject.Initialize()
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("GameWorld is already initialized.");
            }

            InternalInitialize();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            if (!HasBegunPlay)
            {
                return;
            }

            InternalTick(frameContext);
        }

        void IDreamWorksObject.ShutDown()
        {
            if (!IsInitialized)
            {
                return;
            }

            InternalShutDown();
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
            if (IsDisposed)
            {
                logProvider.LogError("Trying to add a scene to a disposed world!");

                return;
            }

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
            if (IsDisposed)
            {
                logProvider.LogError("Trying to add a scene to a disposed world!");

                return;
            }

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
            FScopedLogProvider scopedLogger = new FScopedLogProvider(new FLogCategory(nameof(FTickManager), ELogVerbosity.Display));

            TickManager = new FTickManager(this, scopedLogger);
        }

        private void CreateComponentManager(FTickManager tickManager)
        {
            FScopedLogProvider scopedLogger = new FScopedLogProvider(new FLogCategory(nameof(FComponentManager), ELogVerbosity.Display));

            ComponentManager = new FComponentManager(this, tickManager, scopedLogger);
        }

        private void CreateSpawnManager(FComponentManager componentManager)
        {
            FScopedLogProvider scopedLogger = new FScopedLogProvider(new FLogCategory(nameof(FSpawnManager), ELogVerbosity.Display));

            SpawnManager = new FSpawnManager(this, componentManager, scopedLogger);
        }

        private void InternalInitialize()
        {
            logProvider.Log("Initializing.");

            SetupGameMode();

            InitializeComponentManager();

            IsInitialized = true;

            ProcessPendingScenes();

            OnCreated();

            BeginPlay();
        }

        private void SetupGameMode()
        {
            GameMode = CreateGameMode();

            if (GameMode == null)
            {
                throw new InvalidOperationException($"Failed to create GameMode");
            }

            InitializeGameMode();
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
                new FScopedLogProvider(new FLogCategory(gameModeSettings.GameModeClass.Type.Name, ELogVerbosity.Display))
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

        private void InitializeGameMode()
        {
            GameMode.InitGame(this, PrimaryScene.name);
        }

        private void InitializeComponentManager()
        {
            ComponentManager.Initialize();
        }

        private void BeginPlay()
        {
            logProvider.Log("BeginPlay.");

            BeginPlayGameMode();

            BeginPlayComponentManager();

            HasBegunPlay = true;
        }

        private void BeginPlayGameMode()
        {
            GameMode.StartPlay();
        }

        private void BeginPlayComponentManager()
        {
            ComponentManager.BeginPlay();
        }

        private void InternalTick(FFrameContext frameContext)
        {
            GameMode.Tick(frameContext.DeltaTime);

            TickManager.Tick(frameContext);
        }

        private void InternalShutDown()
        {
            logProvider.Log("Shutdown GameWorld ...");

            IsShuttingDown = true;

            InternalUninitialize();

            IsShuttingDown = false;
        }

        private void InternalUninitialize()
        {
            if (HasBegunPlay)
            {
                ComponentManager.EndPlay();
            }

            ComponentManager.Uninitialize();

            GameMode.EndPlay();

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

            IsDisposed = true;
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