using System;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction
{
    public interface ILogProvider
    {
        /// <summary>
        /// Determines whether logging is enabled for the specified verbosity level.
        /// </summary>
        /// <param name="verbosity">The verbosity level to check.</param>
        /// <returns><see langword="true"/> if logging is enabled for the specified verbosity level; otherwise, <see
        /// langword="false"/>.</returns>
        bool IsEnable(ELogVerbosity verbosity);
    }

    internal interface ILogProvider_Internal : ILogProvider
    {
        /// <summary>
        /// Logs a message with the specified verbosity level and optional context object.
        /// </summary>
        /// <param name="verbosity">The verbosity level of the log message, indicating its importance or severity.</param>
        /// <param name="message">The message to log. Cannot be null or empty.</param>
        /// <param name="context">An optional Unity object to associate with the log message, providing additional context. Can be null.</param>
        void Log(ELogVerbosity verbosity, string message, UnityEngine.Object context = null);

        /// <summary>
        /// Logs a message, exception, or both with the specified verbosity level and optional context.
        /// </summary>
        /// <remarks>If both <paramref name="message"/> and <paramref name="exception"/> are provided,
        /// they will be logged together. The <paramref name="context"/> parameter can be used to associate the log
        /// entry with a specific Unity object,  which may help in debugging within the Unity Editor.</remarks>
        /// <param name="verbosity">The verbosity level of the log entry, indicating its importance or severity.</param>
        /// <param name="exception">The exception to log. Can be <see langword="null"/> if only a message is being logged.</param>
        /// <param name="message">The message to log. Can be <see langword="null"/> if only an exception is being logged.</param>
        /// <param name="context">An optional Unity object to associate with the log entry, providing additional context. Can be <see
        /// langword="null"/>.</param>
        void Log(ELogVerbosity verbosity, Exception exception, string message = null, UnityEngine.Object context = null);
    }
}
