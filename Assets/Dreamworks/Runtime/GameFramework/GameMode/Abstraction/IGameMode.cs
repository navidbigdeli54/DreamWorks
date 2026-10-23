using DreamMachineGameStudio.DreamWorks.World;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework.GameMode.Abstraction
{
    public interface IGameMode
    {
        #region Public Methods
        void InitGame(IGameWorld gameWorld, string sceneName);

        void StartPlay();

        void Tick(float deltaTime);

        void EndPlay(); 
        #endregion
    }
}