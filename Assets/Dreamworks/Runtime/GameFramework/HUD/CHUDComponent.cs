using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;
using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;
using DreamMachineGameStudio.DreamWorks.GameFramework.HUD.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.HUD
{
    public class CHUDComponent : CGameFrameworkComponent, IHUDPossession
    {
        #region Properties
        public CPlayerControllerComponent OwningPlayerController { get; private set; }

        public CPawnComponent OwningCharacter => OwningPlayerController.PossessedPawn;
        #endregion

        #region IHUDPossession Implementation
        void IHUDPossession.SetOwningPlayerController(CPlayerControllerComponent owningPlayerController)
        {
            OwningPlayerController = owningPlayerController;
        }
        #endregion

        #region MonoBehaviour Methods
        private void OnGUI()
        {
            DrawHUD();
        }
        #endregion

        #region Protected Methods
        protected virtual void DrawHUD()
        {

        }
        #endregion
    }
}