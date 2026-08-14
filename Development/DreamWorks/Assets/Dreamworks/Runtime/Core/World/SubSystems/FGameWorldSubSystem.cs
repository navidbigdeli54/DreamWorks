using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems;

namespace DreamMachineGameStudio.DreamWorks.Core.World.SubSystems
{
    public abstract class FGameWorldSubSystem : FSubSystem
    {
        #region Properties
        public IGameWorld GameWorld { get; }
        #endregion

        #region Constructors
        public FGameWorldSubSystem(IGameWorld gameWorld)
        {
            GameWorld = gameWorld;
        }
        #endregion
    }
}
