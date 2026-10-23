using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Foundation;

namespace DreamMachineGameStudio.DreamWorks.SubSystems.Settings
{
    public class USubSystemRegistery : UDataAsset
    {
        #region Fields
        [field: SerializeField]
        public List<FSubSystemSettings> SubSystemSettings { get; private set; } = new() { FSubSystemRegistryDefaults.GetConsoleSubSystemSetting() };
        #endregion
    }
}