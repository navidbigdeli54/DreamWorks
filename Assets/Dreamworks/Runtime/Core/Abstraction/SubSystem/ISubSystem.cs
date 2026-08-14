using System;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystem
    {
        #region Properties
        Type RegistrationType { get; }

        bool CanTick { get; }
        #endregion

        #region Methods
        Task InitializeAsync();

        void Tick(float deltaTime);

        Task ShutDownAsync();
        #endregion
    }
}
