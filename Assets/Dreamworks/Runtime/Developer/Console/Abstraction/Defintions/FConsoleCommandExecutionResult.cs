namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions
{
    /// <summary>
    /// Represents the result of a console command execution, including its success status and an associated message.
    /// </summary>
    /// <remarks>This struct is used to encapsulate the outcome of a console command execution. It provides
    /// information about whether the command was successful and an optional message describing the result.</remarks>
    internal readonly struct FConsoleCommandExecutionResult
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool WasSuccessful { get; }

        /// <summary>
        /// Gets the message associated with the current instance.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Represents an empty result for a console command execution.
        /// </summary>
        /// <remarks>This static field provides a predefined instance of <see
        /// cref="FConsoleCommandExecutionResult"> with a default state indicating no successful execution and an empty
        /// message.</remarks>
        public static FConsoleCommandExecutionResult Empty { get; } = new(false, string.Empty);
        #endregion

        #region Constructors
        public FConsoleCommandExecutionResult(bool wasSuccessful, string message)
        {
            WasSuccessful = wasSuccessful;

            Message = message;
        }
        #endregion
    }
}