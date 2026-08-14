using DreamMachineGameStudio.DreamWorks.GameFramework.Pawn;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.Controller.Abstraction
{
    public interface IControllerPossession
    {
        CPawnComponent PossessedPawn { get; }

        void Possess(CPawnComponent newPawn);

        void UnPossess();
    }
}
