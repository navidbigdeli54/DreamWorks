using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool
{
    public sealed class FObjectPool
    {
        #region Fields
        private readonly Transform parent;

        private readonly IGameWorld gameWorld;

        private readonly IPoolableObject prefab;

        private readonly HashSet<IPoolableObject> activeObjects = new();

        private readonly Stack<IPoolableObject> availableObjects = new();
        #endregion

        #region Properties
        public int ActiveCount => activeObjects.Count;

        public int AvailableCount => availableObjects.Count;

        public int TotalCount => ActiveCount + AvailableCount;

        public IPoolableObject Prefab => prefab;
        #endregion

        #region Constructors
        public FObjectPool(IGameWorld gameWorld, Transform parent, IPoolableObject prefab)
        {
            this.parent = parent;
            this.prefab = prefab;
            this.gameWorld = gameWorld;
        }
        #endregion

        #region Public Methods
        public CPoolableObjectComponent Acquire()
        {
            if (availableObjects.Count == 0)
            {
                ExtendPool();
            }

            IPoolableObject instance = availableObjects.Pop();

            activeObjects.Add(instance);

            instance.OnAcquire();

            return (CPoolableObjectComponent)instance;
        }

        public void Release(IPoolableObject instance)
        {
            if (instance == null)
            {
                return;
            }

            if (activeObjects.Remove(instance) == false)
            {
                return;
            }

            instance.OnRelease();

            availableObjects.Push(instance);
        }

        public void ClearAll()
        {
            foreach (CPoolableObjectComponent instance in availableObjects)
            {
                if (instance != null)
                {
                    gameWorld.Destroy(instance.gameObject);
                }
            }

            foreach (CPoolableObjectComponent instance in activeObjects)
            {
                if (instance != null)
                {
                    gameWorld.Destroy(instance.gameObject);
                }
            }

            activeObjects.Clear();
            availableObjects.Clear();
        }
        #endregion

        #region Private Methods
        private void ExtendPool()
        {
            for (int i = 0; i < prefab.PoolExtendCount; i++)
            {
                SpawnNewInstance();
            }
        }

        private void SpawnNewInstance()
        {
            GameObject gameObject = gameWorld.SpawnGameObject(prefab.GameObject, Vector3.zero, Quaternion.identity);

            gameObject.SetActive(false);
            gameObject.name = prefab.GameObject.name;
            gameObject.transform.SetParent(parent, false);

            IPoolableObject poolableObject = gameObject.GetComponent<CPoolableObjectComponent>();
            poolableObject.Initialize(this);

            availableObjects.Push(poolableObject);
        }
        #endregion
    }
}
