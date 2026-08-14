using DreamMachineGameStudio.DreamWorks.Core.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems
{
    public sealed class FGameInstanceSubSystemCollection : FSubSystemCollection<FGameInstanceSubSystem>
    {
        #region Fields
        private readonly IGameInstance gameInstance;
        #endregion

        #region Constructors
        public FGameInstanceSubSystemCollection(IGameInstance gameInstance, ILogProvider logProvider)
            : base(new FGameInstanceSubSystemFactory(gameInstance), logProvider)
        {
            this.gameInstance = gameInstance;
        }
        #endregion
    }
}
