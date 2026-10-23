using DreamMachineGameStudio.DreamWorks.World;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction
{
    public interface IGameInstance
    {
        T GetSubSystem<T>();

        IGameWorldManager WorldManager { get; }
    }
}