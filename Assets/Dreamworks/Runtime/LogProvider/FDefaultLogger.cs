using System;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ILogProvider"/> interface for logging messages with a
    /// predefined log category and verbosity level.
    /// </summary>
    /// <remarks>This class is a singleton, and its instance can be accessed via the <see cref="Instance"/>
    /// field. It uses an internal <see cref="FScopedLogProvider"/> to handle logging operations with the default category
    /// "Default" and verbosity level <see cref="ELogVerbosity.Display"/>.</remarks>
    public sealed class FDefaultLogger : ILogProvider_Internal
    {
        #region Fields
        public static readonly ILogProvider Instance = new FDefaultLogger();

        private readonly ILogProvider scopedLogger = new FScopedLogProvider(new FLogCategory("Default", ELogVerbosity.Display));
        #endregion

        #region ILogProvider Implementation
        bool ILogProvider.IsEnable(ELogVerbosity verbosity)
        {
            return scopedLogger.IsEnable(verbosity);
        }

        void ILogProvider_Internal.Log(ELogVerbosity verbosity, string message, UnityEngine.Object context)
        {
            ((ILogProvider_Internal)scopedLogger).Log(verbosity, message, context);
        }

        void ILogProvider_Internal.Log(ELogVerbosity verbosity, Exception exception, string message, UnityEngine.Object context)
        {
            ((ILogProvider_Internal)scopedLogger).Log(verbosity, exception, message, context);
        }
        #endregion
    }
}
