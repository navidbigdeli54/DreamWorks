using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Pawn
{
    public class CPlayerStartComponent : CGameFrameworkComponent
    {
        #region Properties
        [field: SerializeField]
        public bool IsActive { get; set; }
        #endregion
    }
}