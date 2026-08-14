using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;

using UDebug = UnityEngine.Debug;

namespace DreamMachineGameStudio.DreamWorks.Log
{
    public class FDefaultLogger : ILogProvider
    {
        #region Fields
        public static FDefaultLogger Instance = new FDefaultLogger();
        #endregion

        #region Constructors
        private FDefaultLogger()
        {

        }
        #endregion

        #region Public Methods
        public void Log(string message)
        {
            UDebug.Log(message);
        }

        public void LogWarning(string message)
        {
            UDebug.LogWarning(message);
        }

        public void LogError(string message)
        {
            UDebug.LogError(message);
        }
        #endregion
    }
}