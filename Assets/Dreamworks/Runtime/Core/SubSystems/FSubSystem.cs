using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;
using System;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.SubSystems
{

    public abstract class FSubSystem : ISubSystem
    {
        #region Properties
        public virtual Type RegistrationType => GetType();
        #endregion

        #region ISubSystem Implementation
        async Task ISubSystem.InitializeAsync()
        {
            await InitializeAsync();
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

        protected virtual Task ShutDownAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
