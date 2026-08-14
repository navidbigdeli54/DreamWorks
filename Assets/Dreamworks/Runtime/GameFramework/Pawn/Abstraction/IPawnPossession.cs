using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Pawn.Abstraction
{
    public interface IPawnPossession
    {
        CControllerComponent Controller { get; }

        void PossessedBy(CControllerComponent newController);

        void OnPossessed();

        void OnUnPossessed();
    }
}
