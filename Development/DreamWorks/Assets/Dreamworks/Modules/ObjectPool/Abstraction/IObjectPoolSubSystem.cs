namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool.Abstraction
{
    public interface IObjectPoolSubSystem
    {
        IPoolableObject Acquire(IPoolableObject prefab);

        void Release(IPoolableObject instance);
    }
}
