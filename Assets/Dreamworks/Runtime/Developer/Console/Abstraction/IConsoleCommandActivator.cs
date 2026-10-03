using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines the contract for activating and executing console commands.
    /// </summary>
    /// <remarks>This interface provides methods and events for handling console command execution. 
    /// Implementations of this interface are responsible for processing command input and returning the results of
    /// execution. It also includes events to notify when commands  are entered and executed.</remarks>
    internal interface IConsoleCommandActivator
    {
        #region Events
        /// <summary>
        /// Occurs when a command is entered.
        /// </summary>
        /// <remarks>This event is triggered whenever a command is entered, providing the entered command
        /// as a string. Subscribers can use this event to handle or process the command.</remarks>
        event Action<string> OnCommandEntered;

        /// <summary>
        /// Occurs when a console command has been executed, providing the result of the execution.
        /// </summary>
        /// <remarks>This event is triggered after a console command is processed. The event handler
        /// receives an  <see cref="FConsoleCommandExecutionResult"/> object that contains details about the execution, 
        /// such as whether the command succeeded and any output or error messages.</remarks>
        event Action<FConsoleCommandExecutionResult> OnCommandExecuted;
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command line and returns the result of the execution.
        /// </summary>
        /// <remarks>The method processes the provided command line string and executes it in the context
        /// of the application.  Ensure that the command is valid and formatted correctly to avoid execution
        /// errors.</remarks>
        /// <param name="commandLine">The command line string to execute. This should include the command and any arguments.</param>
        /// <returns>An <see cref="FConsoleCommandExecutionResult"/> representing the outcome of the command execution, 
        /// including any output, errors, and execution status.</returns>
        FConsoleCommandExecutionResult ExecuteCommand(string commandLine);
        #endregion
    }
}
