using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public interface IGameMode
    {
        #region Public Methods
        Task InitGameAsync(IGameWorld gameWorld, string sceneName);

        Task StartPlayAsync();

        void Tick(float deltaTime);

        Task EndPlayAsync(); 
        #endregion
    }
}