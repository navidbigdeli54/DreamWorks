using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core.World;
using DreamMachineGameStudio.DreamWorks.Core.Assets;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class UDreamWorksSettings : UDataAsset
    {
        #region Properties
        [field: SerializeField]
        public TSubclassOf<IGameInstance> GameInstanceClass { get; private set; } = typeof(FGameInstance);

        [field: SerializeField]
        public TSubclassOf<IGameWorld> GameWorldClass { get; private set; } = typeof(FGameWorld);

        [field: SerializeField]
        public FGameModeSettings GameModeSettings { get; private set; }
        #endregion
    }
}