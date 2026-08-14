using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;

namespace DreamMachineGameStudio.DreamWorks.Core.SubSystems
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
            List<ISubSystem> result = new List<ISubSystem>();

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
