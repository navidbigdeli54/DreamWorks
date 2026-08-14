using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Log
{
    public class FScopedLogger : ILogProvider
    {
        #region Properties
        public FLogCategory Category { get; }

        public ILogProvider Logger { get; }
        #endregion

        #region Constructor
        public FScopedLogger(FLogCategory category)
        {
            Category = category ?? throw new System.ArgumentNullException(nameof(category));

            Logger = FDefaultLogger.Instance;
        }
        #endregion

        #region ILogger Implementation
        public void Log(string message)
        {
            InternalLog(ELogVerbosity.Display, message, Logger.Log);
        }

        public void LogWarning(string message)
        {
            InternalLog(ELogVerbosity.Warning, message, Logger.LogWarning);
        }

        public void LogError(string message)
        {
            InternalLog(ELogVerbosity.Error, message, Logger.LogError);
        }
        #endregion

        #region Core Gate
        private void InternalLog( ELogVerbosity verbosity, string message, System.Action<string> logger)
        {
            if (!ShouldLogMessage(verbosity))
            {
                return;
            }

            logger(Format(message, verbosity));
        }

        private bool ShouldLogMessage(ELogVerbosity verbosity)
        {
            return verbosity >= Category.MinVerbosity;
        }
        #endregion

        #region Formatting
        private string Format(string message, ELogVerbosity verbosity)
        {
            return $"<b> <color=#{ColorUtility.ToHtmlStringRGBA(Category.Color)}> [{Category.Name}] </color> [{verbosity}] </b> {message}";
        }
        #endregion
    }
}