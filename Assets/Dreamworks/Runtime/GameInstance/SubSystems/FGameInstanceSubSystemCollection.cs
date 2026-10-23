using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems
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
