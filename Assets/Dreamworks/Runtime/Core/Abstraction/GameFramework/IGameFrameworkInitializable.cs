using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public interface IGameFrameworkInitializable
    {
        public EGameFrameworkLifecycleState LifecycleState { get; }

        void PreInitialize();

        void Initialize();

        void PostInitialize();

        void BeginPlay();

        void EndPlay();

        void Uninitialize();
    }
}