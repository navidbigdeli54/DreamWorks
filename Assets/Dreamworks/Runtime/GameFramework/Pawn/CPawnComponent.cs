using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;
using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Pawn
{
    public class CPawnComponent : CGameFrameworkComponent, IPawnPossession
    {
        #region Fields
        public CControllerComponent Controller { get; private set; }
        #endregion

        #region IPawnPossession Implementation
        void IPawnPossession.PossessedBy(CControllerComponent newController)
        {
            Controller = newController;
        }

        void IPawnPossession.OnPossessed()
        {
            OnPossessed();
        }

        void IPawnPossession.OnUnPossessed()
        {
            OnUnPossessed();
        }
        #endregion

        #region Protected Methods
        protected virtual void OnPossessed()
        {

        }

        protected virtual void OnUnPossessed()
        {

        }
        #endregion
    }
}