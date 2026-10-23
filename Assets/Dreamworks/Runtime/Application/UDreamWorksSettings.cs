using UnityEngine;
using DreamMachineGameStudio.DreamWorks.World;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Application
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