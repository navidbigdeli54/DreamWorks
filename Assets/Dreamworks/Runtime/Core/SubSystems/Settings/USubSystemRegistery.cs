using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Assets;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class USubSystemRegistery : UDataAsset
    {
        #region Fields
        [field: SerializeField]
        public List<FSubSystemSettings> SubSystemSettings { get; private set; } = new() { FSubSystemRegistryDefaults.GetConsoleSubSystemSetting() };
        #endregion
    }
}