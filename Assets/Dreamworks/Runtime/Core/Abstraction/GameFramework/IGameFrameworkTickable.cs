namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public interface IGameFrameworkTickable
    {
        public FTickSetting TickSetting { get; }

        public FTickState TickState { get; }

        public void Tick(float deltaTime);
    }
}