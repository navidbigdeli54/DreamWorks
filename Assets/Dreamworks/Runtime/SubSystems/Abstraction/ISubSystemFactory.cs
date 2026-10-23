using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction
{
    public interface ISubSystemFactory
    {
        #region Methods
        IReadOnlyList<ISubSystem> CreateSubSystems();
        #endregion
    }
}
