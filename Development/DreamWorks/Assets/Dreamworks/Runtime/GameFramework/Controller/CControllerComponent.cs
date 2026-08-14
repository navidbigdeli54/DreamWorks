using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;
using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn.Abstraction;
using DreamMachineGameStudio.DreamWorks.GameFramework.Controller.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Controller
{

    public class CControllerComponent : CGameFrameworkComponent, IControllerPossession
    {
        #region Properties
        public CPawnComponent PossessedPawn { get; private set; }
        #endregion

        #region Public Methods
        public void Possess(CPawnComponent newPawn)
        {
            if (PossessedPawn == newPawn)
            {
                return;
            }

            UnPossess();

            if (newPawn.Controller != null)
            {
                newPawn.Controller.UnPossess();
            }

            PossessedPawn = newPawn;

            ((IPawnPossession)newPawn).PossessedBy(this);

            OnPossess(newPawn);

            ((IPawnPossession)newPawn).OnPossessed();
        }

        public void UnPossess()
        {
            if (PossessedPawn == null)
            {
                return;
            }

            CPawnComponent oldPawn = PossessedPawn;

            PossessedPawn = null;

            ((IPawnPossession)oldPawn).PossessedBy(null);

            ((IPawnPossession)oldPawn).OnUnPossessed();

            OnUnPossess(oldPawn);
        }
        #endregion

        #region Protected Methods
        protected virtual void OnPossess(CPawnComponent pawn)
        {
        }

        protected virtual void OnUnPossess(CPawnComponent pawn)
        {
        }
        #endregion
    }
}