using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems
{
    public sealed class FGameInstanceSubSystemFactory : FSubSystemFactory<IGameInstance, FGameInstanceSubSystem>
    {
        public FGameInstanceSubSystemFactory(IGameInstance gameInstance)
            : base(gameInstance)
        {

        }
    }
}
