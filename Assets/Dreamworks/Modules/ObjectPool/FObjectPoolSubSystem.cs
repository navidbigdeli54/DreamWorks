using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Log;

namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool
{
    [ADreamWorksSubSystem(
        displayName: "Object Pool",
        description: "Provides pooled object allocation and recycling for improved runtime performance and reduced garbage collection.",
        category: "Core",
        order: 30,
        Experimental = false,
        Advanced = false,
        Keywords = "object pool pooling spawn recycle instantiate performance memory gc")]
    public class FObjectPoolSubSystem : FGameInstanceSubSystem, IObjectPoolSubSystem
    {
        #region Fields
        private CObjectPoolSubSystem rootComponent;

        private readonly ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(FObjectPoolSubSystem), ELogVerbosity.Verbose));

        private readonly Dictionary<string, Transform> categoryParents = new(StringComparer.Ordinal);
        #endregion

        #region Properties
        public override Type RegistrationType => typeof(IObjectPoolSubSystem);

        public readonly Dictionary<EntityId, FObjectPool> ObjectPools = new();
        #endregion

        #region Constructors
        public FObjectPoolSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
        }
        #endregion

        #region Protected Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            GameInstance.WorldManager.OnWorldShutDown += OnWorldShutDown;

            rootComponent = new GameObject(nameof(FObjectPoolSubSystem)).AddComponent<CObjectPoolSubSystem>();
        }

        protected override async Task ShutDownAsync()
        {
            GameInstance.WorldManager.OnWorldShutDown -= OnWorldShutDown;

            ClearPools();

            DestoryRootObject();

            await base.ShutDownAsync();
        }
        #endregion

        #region IObjectPoolSubSystem Implementation
        IPoolableObject IObjectPoolSubSystem.Acquire(IPoolableObject prefab)
        {
            if (prefab == null)
            {
                logProvider.LogError($"The given prefab should not be null!");

                return null;
            }

            FObjectPool pool = GetPool(prefab);

            return pool.Acquire();
        }

        void IObjectPoolSubSystem.Release(IPoolableObject instance)
        {
            if (instance == null)
            {
                return;
            }

            instance.ReturnToPool();
        }
        #endregion

        #region Private Methods
        private void OnWorldShutDown(IGameWorld world)
        {
            List<EntityId> poolsToRemove = new List<EntityId>();

            foreach (KeyValuePair<EntityId, FObjectPool> pair in ObjectPools)
            {
                FObjectPool pool = pair.Value;

                if (pool.Prefab.DestoryOnWorldShutdown)
                {
                    pool.ClearAll();

                    poolsToRemove.Add(pair.Key);
                }
            }

            foreach (EntityId pool in poolsToRemove)
            {
                ObjectPools.Remove(pool);
            }
        }

        private FObjectPool GetPool(IPoolableObject prefab)
        {
            EntityId key = prefab.GameObject.GetEntityId();

            if (!ObjectPools.TryGetValue(key, out FObjectPool pool))
            {
                pool = new FObjectPool(GameInstance.WorldManager.GetFirstGameWorld(), GetCategoryParent(prefab.Category), prefab);

                ObjectPools.Add(key, pool);
            }

            return pool;
        }

        private Transform GetCategoryParent(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                category = "Default";
            }

            if (!categoryParents.TryGetValue(category, out Transform parent))
            {
                GameObject categoryObject = new GameObject(category);

                parent = categoryObject.transform;

                parent.SetParent(rootComponent.transform, false);

                categoryParents.Add(category, parent);
            }

            return parent;
        }

        private void ClearPools()
        {
            foreach (FObjectPool pool in ObjectPools.Values)
            {
                pool.ClearAll();
            }

            ObjectPools.Clear();
            categoryParents.Clear();
        }

        private void DestoryRootObject()
        {
            if (rootComponent != null)
            {
                UnityEngine.Object.Destroy(rootComponent.gameObject);
                rootComponent = null;
            }
        }
        #endregion
    }
}