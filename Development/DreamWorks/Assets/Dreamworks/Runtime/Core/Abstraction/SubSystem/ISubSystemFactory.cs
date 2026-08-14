using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystemFactory
    {
        #region Methods
        IReadOnlyList<ISubSystem> CreateSubSystems();
        #endregion
    }
}
