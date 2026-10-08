using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.ObjectPool.Abstraction
{
    public interface IPoolableObject
    {
        string Category { get; }

        bool DestoryOnWorldShutdown { get; }

        int PoolExtendCount { get; }

        GameObject GameObject { get; }

        void Initialize(FObjectPool objectPool);

        void ReturnToPool();

        void OnAcquire();

        void OnRelease();
    }
}
