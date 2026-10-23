using System;

namespace DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction
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
