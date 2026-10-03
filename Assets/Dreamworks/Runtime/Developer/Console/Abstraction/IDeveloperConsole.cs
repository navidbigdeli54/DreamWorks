namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines the contract for a developer console, providing functionality to manage console methods and variables.
    /// </summary>
    /// <remarks>This interface combines the capabilities of <see cref="IConsoleMethodRepository"/> and <see
    /// cref="IConsoleVariableRepository"/>, allowing the implementation to handle both console commands and variables
    /// in a unified manner.</remarks>
    public interface IDeveloperConsole : IConsoleMethodRepository, IConsoleVariableRepository
    {

    }
}
