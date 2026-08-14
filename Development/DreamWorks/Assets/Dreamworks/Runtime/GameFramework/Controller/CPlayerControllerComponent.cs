using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;
using DreamMachineGameStudio.DreamWorks.GameFramework.HUD;
using DreamMachineGameStudio.DreamWorks.GameFramework.HUD.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Controller
{
    public class CPlayerControllerComponent : CControllerComponent
    {
        #region Properties
        public CHUDComponent HUD { get; private set; }

        public CPlayerStartComponent LastPlayerStart { get; set; }
        #endregion

        #region Public Methods
        public void SetHUD(CHUDComponent newHUD)
        {
            if (HUD != null)
            {
                //TODO:
                //Destory Current HUD Here!
            }

            HUD = newHUD;

            ((IHUDPossession)HUD).SetOwningPlayerController(this);
        }
        #endregion
    }
}