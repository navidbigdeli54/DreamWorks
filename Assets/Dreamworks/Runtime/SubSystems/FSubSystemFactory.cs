using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.SubSystems.Settings;
using DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.SubSystems
{
    public abstract class FSubSystemFactory<TOwner, TSubSystem> : ISubSystemFactory where TSubSystem : class, ISubSystem
    {
        #region Fields
        protected readonly TOwner owner;
        #endregion

        #region Constructors
        public FSubSystemFactory(TOwner owner)
        {
            this.owner = owner;
        }
        #endregion

        #region ISubSystemFactory Implementation
        IReadOnlyList<ISubSystem> ISubSystemFactory.CreateSubSystems()
        {
            var result = new List<ISubSystem>();

            IReadOnlyList<FSubSystemSettings> subsystemSettings = FSubSystemRegisteryProvider.GetSubSystemsOf<TSubSystem>();

            foreach (FSubSystemSettings setting in subsystemSettings)
            {
                ISubSystem subSystem = setting.SubSystem.Construct(new object[] { owner });

                result.Add(subSystem);
            }

            return result;
        }
        #endregion
    }
}
