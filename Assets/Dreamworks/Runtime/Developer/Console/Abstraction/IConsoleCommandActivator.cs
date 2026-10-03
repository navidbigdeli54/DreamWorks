using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    internal interface IConsoleCommandActivator
    {
        #region Events
        event Action<string> OnCommandEntered;

        event Action<FConsoleCommandExecutionResult> OnCommandExecuted;
        #endregion

        #region Methods
        FConsoleCommandExecutionResult ExecuteCommand(string commandLine);
        #endregion
    }
}
