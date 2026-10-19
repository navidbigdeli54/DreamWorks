using System;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystem
    {
        #region Properties
        Type RegistrationType { get; }

        bool CanTick { get; }
        #endregion

        #region Methods
        void Initialize();

        void Tick(float deltaTime);

        void ShutDown();
        #endregion
    }
}
