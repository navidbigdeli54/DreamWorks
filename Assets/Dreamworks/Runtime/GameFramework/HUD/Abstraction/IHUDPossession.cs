using DreamMachineGameStudio.DreamWorks.GameFramework.Controller;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.HUD.Abstraction
{
    public interface IHUDPossession
    {
        CPlayerControllerComponent OwningPlayerController { get; }

        void SetOwningPlayerController(CPlayerControllerComponent owningPlayerController);
    }
}
