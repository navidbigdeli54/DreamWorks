using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Core.SubSystems
{
    public abstract class FSubSystemCollection<T> : ISubSystemCollection<T> where T : class, ISubSystem
    {
        #region Fields
        protected readonly ILogProvider logProvider;

        protected readonly ISubSystemFactory factory;

        protected readonly Dictionary<Type, ISubSystem> registeredSubSystems = new Dictionary<Type, ISubSystem>();
        #endregion

        #region Constructors
        public FSubSystemCollection(ISubSystemFactory factory, ILogProvider logProvider)
        {
            this.factory = factory;

            this.logProvider = logProvider;

            CreateAndRegisterAllSubSystems();
        }
        #endregion

        #region ISubSystemCollection Implementation
        async Task ISubSystemCollection<T>.InitializeAsync()
        {
            foreach (KeyValuePair<Type, ISubSystem> pair in registeredSubSystems)
            {
                ISubSystem subSystem = pair.Value;
                if (subSystem != null)
                {
                    await subSystem.InitializeAsync();
                }
            }
        }

        async Task ISubSystemCollection<T>.ShutDownAsync()
        {
            foreach (KeyValuePair<Type, ISubSystem> pair in registeredSubSystems)
            {
                ISubSystem subSystem = pair.Value;
                if (subSystem != null)
                {
                    await subSystem.ShutDownAsync();
                }
            }
        }

        TSystem ISubSystemCollection<T>.GetSubSystem<TSystem>()
        {
            return GetSubSystem<TSystem>();
        }

        void ISubSystemCollection<T>.ClearSubSystems()
        {
            ClearSubSystems();
        }
        #endregion

        #region Private Methods
        private void CreateAndRegisterAllSubSystems()
        {
            logProvider.Log("Registering SubSystems.");

            IReadOnlyList<ISubSystem> subSystems = factory.CreateSubSystems();
            for (int i = 0; i < subSystems.Count; ++i)
            {
                ISubSystem subSystem = subSystems[i];
                if (subSystem != null)
                {
                    RegisterSubSystem(subSystem);
                }
            }
        }

        private void RegisterSubSystem(ISubSystem subSystem)
        {
            registeredSubSystems.Add(subSystem.RegistrationType, subSystem);

            logProvider.Log($"{subSystem.RegistrationType.Name} SubSystem is registered as {subSystem.GetType().Name}.");
        }

        private TSystem GetSubSystem<TSystem>()
        {
            if (registeredSubSystems.TryGetValue(typeof(TSystem), out ISubSystem system))
            {
                return (TSystem)system;
            }

            return default;
        }

        private void ClearSubSystems()
        {
            foreach (KeyValuePair<Type, ISubSystem> pair in registeredSubSystems)
            {
                ISubSystem subSystem = pair.Value;

                subSystem.ShutDownAsync().GetAwaiter().GetResult();
            }

            registeredSubSystems.Clear();
        }
        #endregion
    }
}
