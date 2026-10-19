namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
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