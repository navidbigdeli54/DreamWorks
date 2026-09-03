using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

using UObject = UnityEngine.Object;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    public class FSpawnManager
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IGameWorld gameWorld;

        private readonly FComponentManager componentManager;
        #endregion

        #region Constructors
        public FSpawnManager(IGameWorld gameWorld, FComponentManager componentManager, ILogProvider logProvider)
        {
            this.logProvider = logProvider;

            this.gameWorld = gameWorld;

            this.componentManager = componentManager;
        }
        #endregion

        #region Public Methods
        public GameObject SpawnGameObject(UObject prefab, Vector3 position, Quaternion rotation)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Spawnning a GameObject in a disposed world!");

                return null;
            }

            GameObject resolvedGameObject = prefab is IGameFrameworkComponent component ? component.GameObject : prefab as GameObject;

            return SpawnGameObject(resolvedGameObject, position, rotation);
        }

        public GameObject SpawnGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Spawnning a GameObject in a disposed world!");

                return null;
            }

            GameObject spawnedGameObject = UObject.Instantiate(prefab, position, rotation);

            RegisterGameObject(spawnedGameObject);

            return spawnedGameObject;
        }

        public T SpawnComponent<T>(GameObject owner) where T : MonoBehaviour, IGameFrameworkComponent
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Spawnning a component in a disposed world!");

                return null;
            }

            T component = owner.AddComponent<T>();

            componentManager.RegisterSpawnedComponent(component, gameWorld.PrimaryScene);

            return component;
        }

        public void Destroy(GameObject gameObject)
        {
            if (gameWorld.IsDisposed)
            {
                logProvider.LogError("Destorying a GameObject in a disposed world!");

                return;
            }

            IGameFrameworkComponent[] components = gameObject.GetComponentsInChildren<IGameFrameworkComponent>(true);

            for (int i = 0; i < components.Length; ++i)
            {
                componentManager.UnregisterSpawnedComponent(components[i], gameWorld.PrimaryScene);
            }

            Object.Destroy(gameObject);
        }
        #endregion

        #region Private Methods
        private void RegisterGameObject(GameObject gameObject)
        {
            IGameFrameworkComponent[] components = gameObject.GetComponentsInChildren<IGameFrameworkComponent>(true);

            for (int i = 0; i < components.Length; ++i)
            {
                componentManager.RegisterSpawnedComponent(components[i], gameWorld.PrimaryScene);
            }
        }
        #endregion
    }
}