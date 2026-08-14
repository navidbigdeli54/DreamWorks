using DreamMachineGameStudio.DreamWorks.Core.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;

namespace DreamMachineGameStudio.DreamWorks.Core.World.SubSystems
{
    public class FGameWorldSubSystemCollection : FSubSystemCollection<FGameWorldSubSystem>
    {
        #region Fields
        protected readonly IGameWorld gameWorld;
        #endregion

        #region Constructors
        public FGameWorldSubSystemCollection(IGameWorld gameWorld, ILogProvider logProvider)
            : base(new FGameWorldSubSystemFactory(gameWorld), logProvider)
        {
            this.gameWorld = gameWorld;
        }
        #endregion
    }
}
