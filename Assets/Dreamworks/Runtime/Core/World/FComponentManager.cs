using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    public class FComponentManager
    {
        #region Fields
        private readonly IGameWorld gameWorld;

        private readonly ILogProvider logProvider;

        private readonly FTickManager tickManager;

        private readonly Dictionary<Scene, List<IGameFrameworkComponent>> sceneToComponents = new();
        #endregion

        #region Constructors
        public FComponentManager(IGameWorld gameWorld, FTickManager tickManager, ILogProvider logProvider)
        {
            this.gameWorld = gameWorld;
            this.tickManager = tickManager;
            this.logProvider = logProvider;
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            InternalPreInitialize();

            InternalInitialize();

            InternalPostInitialize();
        }

        public void BeginPlay()
        {
            InternalBeginPlay();
        }

        public void EndPlay()
        {
            InternalEndPlay();
        }

        public void Uninitialize()
        {
            InternalUninitialize();
        }

        public void RegisterScene(Scene scene)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Registering a scene in a disposed world!");

                return;
            }

            if (!sceneToComponents.ContainsKey(scene))
            {
                List<IGameFrameworkComponent> sceneComponents = new List<IGameFrameworkComponent>();

                sceneToComponents.Add(scene, sceneComponents);
            }

            GameObject[] rootGameObjects = scene.GetRootGameObjects();

            for (int i = 0; i < rootGameObjects.Length; ++i)
            {
                GameObject gameObject = rootGameObjects[i];

                IGameFrameworkComponent[] childrenComponents = gameObject.GetComponentsInChildren<IGameFrameworkComponent>(true);

                for (int j = 0; j < childrenComponents.Length; ++j)
                {
                    RegisterComponent(childrenComponents[j], scene, gameWorld.HasBegunPlay);
                }
            }
        }

        public void UnregisterScene(Scene scene)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Unregistering a scene in a disposed world!");

                return;
            }

            if (!sceneToComponents.TryGetValue(scene, out List<IGameFrameworkComponent> sceneComponents))
            {
                logProvider.LogWarning($"Trying to unregister {scene.name} from FComponentManager but it does not exist!");

                return;
            }

            while (sceneComponents.Count > 0)
            {
                UnregisterComponent(sceneComponents[0], scene, gameWorld.HasBegunPlay);
            }

            sceneToComponents.Remove(scene);
        }

        public void RegisterSpawnedComponent(IGameFrameworkComponent component, Scene scene)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Registering a component in a disposed world!");

                return;
            }

            RegisterComponent(component, scene, gameWorld.HasBegunPlay);
        }

        public void UnregisterSpawnedComponent(IGameFrameworkComponent component, Scene scene)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Unregistering a component in a disposed world!");

                return;
            }

            UnregisterComponent(component, scene, gameWorld.HasBegunPlay);
        }

        public TComponent FindComponent<TComponent>() where TComponent : IGameFrameworkComponent
        {
            return sceneToComponents.Values
                .SelectMany(components => components)
                .OfType<TComponent>()
                .FirstOrDefault();
        }

        public IReadOnlyList<TComponent> FindComponentsOf<TComponent>() where TComponent : IGameFrameworkComponent
        {
            return sceneToComponents.Values
                .SelectMany(components => components)
                .OfType<TComponent>()
                .ToList();
        }
        #endregion

        #region Private Methods
        private void InternalPreInitialize()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].PreInitialize();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError($"An exception thrown when calling {components[i].GameObject.name}:{nameof(IGameFrameworkInitializable.PreInitialize)}. exception: {exception}");
                    }
                }
            }
        }

        private void InternalInitialize()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].Initialize();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception, $"An exception thrown when calling {components[i].GameObject.name}:{nameof(IGameFrameworkInitializable.Initialize)}.");
                    }
                }
            }
        }

        private void InternalPostInitialize()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].PostInitialize();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception, $"An exception thrown when calling {components[i].GameObject.name}:{nameof(IGameFrameworkInitializable.PostInitialize)}.");
                    }
                }
            }
        }

        private void InternalBeginPlay()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].BeginPlay();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception, $"An exception thrown when calling {components[i].GameObject.name}:{nameof(IGameFrameworkInitializable.BeginPlay)}.");
                    }
                }
            }
        }

        private void InternalEndPlay()
        {
            foreach (KeyValuePair<Scene, List<IGameFrameworkComponent>> pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].EndPlay();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception, $"An exception thrown when calling {nameof(IGameFrameworkInitializable.EndPlay)}.");
                    }
                }
            }
        }

        private void InternalUninitialize()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        components[i].Uninitialize();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception, $"An exception thrown when calling {components[i].GameObject.name}:{nameof(IGameFrameworkInitializable.Uninitialize)}.");
                    }
                }
            }
        }

        private void RegisterComponent(IGameFrameworkComponent component, Scene scene, bool beginPlay)
        {
            if (component == null)
            {
                return;
            }

            if (sceneToComponents.Any(x => x.Value.Contains(component)))
            {
                return;
            }

            component.SetGameWorld(gameWorld);

            if (!sceneToComponents.TryGetValue(scene, out List<IGameFrameworkComponent> components))
            {
                components = new List<IGameFrameworkComponent>();

                sceneToComponents.Add(scene, components);
            }

            components.Add(component);

            tickManager.Register(component);

            InitializeComponent(component, beginPlay);
        }

        private void InitializeComponent(IGameFrameworkComponent component, bool beginPlay)
        {
            try
            {
                component.PreInitialize();
                component.Initialize();
                component.PostInitialize();

                if (beginPlay)
                {
                    component.BeginPlay();
                }
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception);
            }
        }

        private void UnregisterComponent(IGameFrameworkComponent component, Scene scene, bool endPlay)
        {
            if (component == null)
            {
                return;
            }

            if (sceneToComponents.TryGetValue(scene, out List<IGameFrameworkComponent> components))
            {
                components.Remove(component);
            }

            tickManager.Unregister(component);

            ShutdownComponent(component, endPlay);
        }

        private void ShutdownComponent(IGameFrameworkComponent component, bool endPlay)
        {
            try
            {
                if (endPlay)
                {
                    component.EndPlay();
                }

                component.Uninitialize();
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception, $"An exception thrown when calling {component.GameObject.name}:{nameof(IGameFrameworkInitializable.EndPlay)}.");
            }
        }
        #endregion
    }
}