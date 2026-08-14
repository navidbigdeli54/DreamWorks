using System;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;

namespace DreamMachineGameStudio.DreamWorks.Core.SubSystems
{

    public abstract class FSubSystem : ISubSystem
    {
        #region Properties
        public virtual Type RegistrationType => GetType();

        public virtual bool CanTick => false;
        #endregion

        #region ISubSystem Implementation
        async Task ISubSystem.InitializeAsync()
        {
            await InitializeAsync();
        }

        void ISubSystem.Tick(float delatTime)
        {
            Tick(delatTime);
        }

        async Task ISubSystem.ShutDownAsync()
        {
            await ShutDownAsync();
        }
        #endregion

        #region Protected Methods
        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void Tick(float deltaTime)
        {

        }

        protected virtual Task ShutDownAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
