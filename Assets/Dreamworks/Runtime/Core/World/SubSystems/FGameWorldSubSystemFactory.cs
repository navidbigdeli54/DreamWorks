using DreamMachineGameStudio.DreamWorks.Core.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core.World.SubSystems
{
    public sealed class FGameWorldSubSystemFactory : FSubSystemFactory<IGameWorld, FGameWorldSubSystem>
    {
        public FGameWorldSubSystemFactory(IGameWorld gameWorld)
            : base(gameWorld)
        {
        }
    }
}
