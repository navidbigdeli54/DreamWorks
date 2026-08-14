namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger
{
    public interface ILogProvider
    {
        void Log(string message);

        void LogWarning(string message);

        void LogError(string message);
    }
}