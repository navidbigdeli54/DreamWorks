namespace DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction
{
    /// <summary>
    /// Specifies the verbosity levels for logging messages.
    /// </summary>
    /// <remarks>The <see cref="ELogVerbosity"/> enumeration defines the severity or detail level of log
    /// messages. It can be used to filter or categorize log output based on the desired verbosity.</remarks>
    public enum ELogVerbosity : byte
    {
        /// <summary>
        /// Specifies that no logging should be performed.
        /// </summary>
        NoLogging = 0,

        /// <summary>
        /// Represents a fatal log level, indicating a critical failure that requires immediate attention.
        /// </summary>
        Fatal = 1,

        /// <summary>
        /// Represents an error condition or state.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Represents a warning log level, typically used to indicate a potential issue  that does not prevent the
        /// application from functioning but may require attention.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Represents the log level used to indicate informational messages that highlight the progress of the application.
        /// </summary>
        Display = 4,

        /// <summary>
        /// Represents the log level used to indicate informational messages that highlight the progress of the application.
        /// </summary>
        Log = 5,

        /// <summary>
        /// Specifies a verbose logging level, typically used for detailed diagnostic information.
        /// </summary>
        /// <remarks>This logging level is intended for scenarios where extensive information is required
        /// for debugging or tracing application behavior. It may produce a large volume of log messages and should be
        /// used with caution in production environments.</remarks>
        Verbose = 6,

        /// <summary>
        /// Represents a logging level that outputs all log messages, including detailed diagnostic information.
        /// </summary>
        /// <remarks>This logging level is typically used for debugging purposes and may produce a large
        /// volume of log data. Use with caution in production environments due to potential performance and storage
        /// implications.</remarks>
        VeryVerbose = 7,
    }
}
