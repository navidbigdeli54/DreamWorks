using System;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystem
    {
        #region Properties
        Type RegistrationType { get; }
        #endregion

        #region Methods
        Task InitializeAsync();

        Task ShutDownAsync();
        #endregion
    }
}
