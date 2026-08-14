using UnityEngine;
using DreamMachineGameStudio.DreamWorks.GameFramework.GameMode;

namespace DreamMachineGameStudio.DreamWorks.Core.World
{
    [DisallowMultipleComponent]
    public sealed class CGameWorldSettings : MonoBehaviour
    {
        #region Properties
        [field: SerializeField]
        public FGameModeSettings GameModeSettingsOverride { get; private set; }
        #endregion
    }
}
