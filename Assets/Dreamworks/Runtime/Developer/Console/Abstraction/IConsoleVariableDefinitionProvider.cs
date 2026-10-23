using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Provides authored console-variable definitions to the runtime console repository.
    /// </summary>
    public interface IConsoleVariableDefinitionProvider
    {
        /// <summary>
        /// Gets the definitions that should be registered for the current application.
        /// </summary>
        IReadOnlyList<FConsoleVariableDefinition> Definitions { get; }
    }
}
