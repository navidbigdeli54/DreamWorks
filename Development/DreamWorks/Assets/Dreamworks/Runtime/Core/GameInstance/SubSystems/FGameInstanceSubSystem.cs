using DreamMachineGameStudio.DreamWorks.Core.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems
{
    public abstract class FGameInstanceSubSystem : FSubSystem
    {
        #region Properties
        public IGameInstance GameInstance { get; }
        #endregion

        #region Constructors
        public FGameInstanceSubSystem(IGameInstance gameInstance)
        {
            GameInstance = gameInstance;
        }
        #endregion
    }
}
