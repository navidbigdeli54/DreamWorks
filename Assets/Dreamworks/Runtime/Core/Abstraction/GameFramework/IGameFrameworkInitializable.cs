using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public interface IGameFrameworkInitializable
    {
        public EGameFrameworkLifecycleState LifecycleState { get; }

        public Task PreInitializeAsync();

        public Task InitializeAsync();

        public Task PostInitializeAsync();

        public Task BeginPlayAsync();

        public Task EndPlayAsync();

        public Task UninitializeAsync();
    }
}