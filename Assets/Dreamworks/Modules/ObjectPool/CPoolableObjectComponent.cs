using UnityEngine;
using DreamMachineGameStudio.DreamWorks.GameFramework;
using DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool
{
    public class CPoolableObjectComponent : CGameFrameworkComponent, IPoolableObject
    {
        #region Fields
        private FObjectPool ownerPool;
        #endregion

        #region Properties
        [field: SerializeField]
        public string Category { get; private set; } = "Default";


        [field: SerializeField]
        public bool DestroyOnWorldShutdown { get; private set; } = true;


        [field: SerializeField]
        public int PoolExtendCount { get; private set; } = 5;

        public bool IsActive { get; private set; }
        #endregion

        #region IPoolableObject Implementation
        string IPoolableObject.Category => Category;

        bool IPoolableObject.DestoryOnWorldShutdown => DestroyOnWorldShutdown;

        int IPoolableObject.PoolExtendCount => PoolExtendCount;

        GameObject IPoolableObject.GameObject => gameObject;

        void IPoolableObject.Initialize(FObjectPool objectPool)
        {
            ownerPool = objectPool;

            Initialize();
        }

        void IPoolableObject.OnAcquire()
        {
            IsActive = true;

            OnAcquire();
        }

        void IPoolableObject.OnRelease()
        {
            IsActive = false;

            OnRelease();
        }
        #endregion

        #region Public Methods
        public void ReturnToPool()
        {
            if (!IsActive)
            {
                return;
            }

            ownerPool.Release(this);
        }
        #endregion

        #region Protected Methods
        protected void Initialize()
        {

        }

        protected virtual void OnAcquire()
        {

        }

        protected virtual void OnRelease()
        {

        }
        #endregion
    }
}