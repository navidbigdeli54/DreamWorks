namespace DreamMachineGameStudio.DreamWorks.ObjectPool.Abstraction
{
    public interface IObjectPoolSubSystem
    {
        IPoolableObject Acquire(IPoolableObject prefab);

        void Release(IPoolableObject instance);
    }
}
