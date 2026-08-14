using System;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
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
        public async Task InitializeAsync()
        {
            await InternalPreInitializeAsync();

            await InternalInitializeAsync();

            await InternalPostInitializeAsync();
        }

        public async Task BeginPlayAsync()
        {
            await InternalBeginPlayAsync();
        }

        public async Task EndPlayAsync()
        {
            await InternalEndPlayAsync();
        }

        public async Task UninitializeAsync()
        {
            await InternalUninitializeAsync();
        }

        public void RegisterScene(Scene scene)
        {
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
            if (!sceneToComponents.TryGetValue(scene, out List<IGameFrameworkComponent> sceneComponents))
            {
                logProvider.LogWarning($"Trying to unregister {scene.name} from FComponentManager but it does not exist!");

                return;
            }

            for (int i = 0; i < sceneComponents.Count; ++i)
            {
                UnregisterComponent(sceneComponents[i], scene, gameWorld.HasBegunPlay);
            }

            sceneToComponents.Remove(scene);
        }

        public void RegisterSpawnedComponent(IGameFrameworkComponent component, Scene scene)
        {
            RegisterComponent(component, scene, gameWorld.HasBegunPlay);
        }

        public void UnregisterSpawnedComponent(IGameFrameworkComponent component, Scene scene)
        {
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
        private async Task InternalPreInitializeAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].PreInitializeAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
                    }
                }
            }
        }

        private async Task InternalInitializeAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].InitializeAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
                    }
                }
            }
        }

        private async Task InternalPostInitializeAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].PostInitializeAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
                    }
                }
            }
        }

        private async Task InternalBeginPlayAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].BeginPlayAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
                    }
                }
            }
        }

        private async Task InternalEndPlayAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].EndPlayAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
                    }
                }
            }
        }

        private async Task InternalUninitializeAsync()
        {
            foreach (var pair in sceneToComponents)
            {
                List<IGameFrameworkComponent> components = pair.Value;

                for (int i = 0; i < components.Count; ++i)
                {
                    try
                    {
                        await components[i].UninitializeAsync();
                    }
                    catch (Exception exception)
                    {
                        logProvider.LogError(exception.Message);
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

            InitializeComponentAsync(component, beginPlay).GetAwaiter().GetResult();
        }

        private async Task InitializeComponentAsync(IGameFrameworkComponent component, bool beginPlay)
        {
            try
            {
                await component.PreInitializeAsync();
                await component.InitializeAsync();
                await component.PostInitializeAsync();

                if (beginPlay)
                {
                    await component.BeginPlayAsync();
                }
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception.ToString());
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

            ShutdownComponentAsync(component, endPlay).GetAwaiter().GetResult();
        }

        private async Task ShutdownComponentAsync(IGameFrameworkComponent component, bool endPlay)
        {
            try
            {
                if (endPlay)
                {
                    await component.EndPlayAsync();
                }

                await component.UninitializeAsync();
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception.ToString());
            }
        }
        #endregion
    }
}