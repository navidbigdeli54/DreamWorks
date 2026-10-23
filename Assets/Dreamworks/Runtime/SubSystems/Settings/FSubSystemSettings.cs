using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.SubSystems.Settings
{
    [Serializable]
    public class FSubSystemSettings
    {
        #region Properties
        [field: SerializeField]
        public TSubclassOf<ISubSystem> SubSystem { get; private set; }

        [field: SerializeField]
        public bool IsEnable { get; private set; }
        #endregion

        #region Constructors
        public FSubSystemSettings(TSubclassOf<ISubSystem> subSystem, bool isEnable = false)
        {
            SubSystem = subSystem;

            IsEnable = isEnable;
        }
        #endregion
    }
}