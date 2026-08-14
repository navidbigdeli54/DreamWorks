namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction
{
    public interface IGameInstance
    {
        T GetSubSystem<T>();

        IGameWorldManager WorldManager { get; }
    }
}