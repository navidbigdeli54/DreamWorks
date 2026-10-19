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
        void ISubSystem.Initialize()
        {
            Initialize();
        }

        void ISubSystem.Tick(float delatTime)
        {
            Tick(delatTime);
        }

        void ISubSystem.ShutDown()
        {
            ShutDown();
        }
        #endregion

        #region Protected Methods
        protected virtual void Initialize()
        {
            
        }

        protected virtual void Tick(float deltaTime)
        {

        }

        protected virtual void ShutDown()
        {
            
        }
        #endregion
    }
}
