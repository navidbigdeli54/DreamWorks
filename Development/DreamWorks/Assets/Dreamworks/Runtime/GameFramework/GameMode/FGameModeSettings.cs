using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.GameFramework.HUD;
using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;
using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.GameMode
{
    [Serializable]
    public class FGameModeSettings
    {
        #region Properties
        [field: SerializeField]
        public TSubclassOf<FGameMode> GameModeClass { get; private set; } = typeof(FGameMode);

        [field: SerializeField]
        public CPlayerControllerComponent PlayerController { get; private set; }

        [field: SerializeField]
        public CPawnComponent Pawn { get; private set; }

        [field: SerializeField]
        public CHUDComponent HUD { get; private set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return GameModeClass.ToString();
        }
        #endregion
    }
}