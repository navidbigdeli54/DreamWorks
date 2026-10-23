using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems
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
