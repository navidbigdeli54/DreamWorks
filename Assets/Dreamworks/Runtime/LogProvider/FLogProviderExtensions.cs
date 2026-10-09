using System.Diagnostics;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LoggProvider
{
    /// <summary>
    /// Provides extension methods for logging messages with various verbosity levels using an <see cref="ILogProvider"/>.
    /// </summary>
    /// <remarks>These extension methods allow developers to log messages with different levels of severity,
    /// such as fatal errors, warnings, and verbose messages. Logging is conditionally compiled based on the presence of
    /// specific preprocessor directives, such as <c>UNITY_EDITOR</c>, <c>DEVELOPMENT_BUILD</c>, or <c>LOGGING</c>. This
    /// ensures that logging can be included or excluded from builds as needed.</remarks>
    public static class FLogProviderExtensions
    {
        #region Public Methods
        /// <summary>
        /// Logs a fatal error message with the specified provider.
        /// </summary>
        /// <remarks>This method is only executed in the Unity Editor, development builds, or when the
        /// "LOGGING" conditional compilation symbol is defined. It will not log messages in release builds unless
        /// explicitly enabled.</remarks>
        /// <param name="provider">The logging provider used to log the message.</param>
        /// <param name="message">The fatal error message to log.</param>
        /// <param name="context">An optional Unity object to associate with the log message. This can be used to provide additional context
        /// for the log entry, such as the GameObject or component that caused the error.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogFatal(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Fatal, message, context);
        }

        /// <summary>
        /// Logs a fatal error message, including an optional exception and context, for debugging purposes.
        /// </summary>
        /// <remarks>This method is only executed in the Unity Editor, development builds, or when the
        /// "LOGGING" conditional compilation symbol is defined. Use this method to log critical errors that indicate a
        /// failure requiring immediate attention.</remarks>
        /// <param name="provider">The <see cref="ILogProvider"/> instance used to handle the log entry.</param>
        /// <param name="exception">The exception associated with the fatal error. Can be <see langword="null"/> if no exception is available.</param>
        /// <param name="message">An optional message describing the fatal error. Can be <see langword="null"/>.</param>
        /// <param name="context">An optional Unity object to associate with the log entry for context. Can be <see langword="null"/>.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogFatal(this ILogProvider provider, System.Exception exception, string message = null, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Fatal, exception, message, context);
        }

        /// <summary>
        /// Logs an error message with the specified provider.
        /// </summary>
        /// <remarks>This method is only executed in builds where the <c>UNITY_EDITOR</c>,
        /// <c>DEVELOPMENT_BUILD</c>, or <c>LOGGING</c> conditional compilation symbols are defined.</remarks>
        /// <param name="provider">The logging provider used to log the error message.</param>
        /// <param name="message">The error message to log. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="context">An optional Unity object to associate with the log entry. This can be <see langword="null"/>.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogError(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Error, message, context);
        }

        /// <summary>
        /// Logs an error message along with an optional exception and context object.
        /// </summary>
        /// <remarks>This method is only executed in builds where the <c>UNITY_EDITOR</c>,
        /// <c>DEVELOPMENT_BUILD</c>, or <c>LOGGING</c> conditional compilation symbols are defined.</remarks>
        /// <param name="provider">The <see cref="ILogProvider"/> instance used to handle the logging operation.</param>
        /// <param name="exception">The exception to log. Can be <see langword="null"/> if no exception is associated with the error.</param>
        /// <param name="message">An optional error message to include in the log. Can be <see langword="null"/> or empty.</param>
        /// <param name="context">An optional Unity object to associate with the log entry, providing additional context. Can be <see
        /// langword="null"/>.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogError(this ILogProvider provider, System.Exception exception, string message = null, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Error, exception, message, context);
        }

        /// <summary>
        /// Logs a warning message to the specified log provider.
        /// </summary>
        /// <remarks>This method only logs messages when the application is running in the Unity Editor, a
        /// development build,  or when the "LOGGING" conditional compilation symbol is defined. In other environments,
        /// the method has no effect.</remarks>
        /// <param name="provider">The log provider that will handle the warning message.</param>
        /// <param name="message">The warning message to log. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="context">An optional Unity object associated with the log message. This can be used to provide additional context 
        /// for the warning, such as the object that triggered the log.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogWarning(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Warning, message, context);
        }

        /// <summary>
        /// Logs a message with <see cref="ELogVerbosity.Display"/> verbosity level.
        /// </summary>
        /// <remarks>This method is only executed in builds where the "UNITY_EDITOR", "DEVELOPMENT_BUILD",
        /// or "LOGGING" conditional compilation symbols are defined.</remarks>
        /// <param name="provider">The <see cref="ILogProvider"/> instance used to log the message.</param>
        /// <param name="message">The message to log. Cannot be <see langword="null"/>.</param>
        /// <param name="context">An optional <see cref="UnityEngine.Object"/> that provides context for the log entry. Defaults to <see
        /// langword="null"/>.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogDisplay(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Display, message, context);
        }

        /// <summary>
        /// Logs a message with the specified provider, using the <see cref="ELogVerbosity.Display"/> verbosity level.
        /// </summary>
        /// <remarks>This method is conditionally compiled and will only execute in builds where the
        /// "UNITY_EDITOR", "DEVELOPMENT_BUILD", or "LOGGING" symbols are defined.</remarks>
        /// <param name="provider">The logging provider that will handle the log message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">An optional Unity object to associate with the log message. This can be used to provide additional context
        /// in the Unity Editor.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void Log(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Display, message, context);
        }

        /// <summary>
        /// Logs a message with a verbosity level of <see cref="ELogVerbosity.Log"/>.
        /// </summary>
        /// <remarks>This method only logs messages when the application is running in the Unity Editor, a
        /// development build,  or when the "LOGGING" conditional compilation symbol is defined. In other environments,
        /// this method has no effect.</remarks>
        /// <param name="provider">The logging provider used to handle the log message.</param>
        /// <param name="message">The message to log. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="context">An optional Unity object to associate with the log message. This can be used to provide additional context 
        /// in the Unity Editor, such as linking the log entry to a specific GameObject.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOGGING")]
        public static void LogMessage(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Log, message, context);
        }

        /// <summary>
        /// Logs a verbose-level message to the specified log provider.
        /// </summary>
        /// <remarks>This method only logs messages when the application is built with the <see
        /// langword="UNITY_EDITOR"/>, <see langword="DEVELOPMENT_BUILD"/>, or <see langword="LOG_VERBOSE"/> compilation
        /// symbols defined.</remarks>
        /// <param name="provider">The log provider that will handle the log message.</param>
        /// <param name="message">The message to log. This should provide detailed information useful for debugging or tracing.</param>
        /// <param name="context">An optional Unity object associated with the log message. This can be used to provide additional context in
        /// the Unity Editor.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOG_VERBOSE")]
        public static void LogVerbose(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.Verbose, message, context);
        }

        /// <summary>
        /// Logs a message with the verbosity level set to <see cref="ELogVerbosity.VeryVerbose"/>.
        /// </summary>
        /// <remarks>This method only logs messages when the application is running in the Unity Editor, a
        /// development build,  or when the <c>LOG_VERY_VERBOSE</c> conditional compilation symbol is defined. It is
        /// intended for detailed  debugging and diagnostic purposes.</remarks>
        /// <param name="provider">The logging provider used to handle the log message.</param>
        /// <param name="message">The message to log. Cannot be <see langword="null"/>.</param>
        /// <param name="context">An optional Unity object to associate with the log message. This can be used to provide additional context 
        /// in the Unity Editor, such as highlighting the object in the Inspector when the log is clicked.</param>
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("LOG_VERY_VERBOSE")]
        public static void LogVeryVerbose(this ILogProvider provider, string message, UnityEngine.Object context = null)
        {
            ((ILogProvider_Internal)provider).Log(ELogVerbosity.VeryVerbose, message, context);
        }
        #endregion
    }
}
