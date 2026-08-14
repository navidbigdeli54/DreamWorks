using DreamMachineGameStudio.DreamWorks.Core.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems
{
    public sealed class FGameInstanceSubSystemFactory : FSubSystemFactory<IGameInstance, FGameInstanceSubSystem>
    {
        public FGameInstanceSubSystemFactory(IGameInstance gameInstance)
            : base(gameInstance)
        {

        }
    }
}
