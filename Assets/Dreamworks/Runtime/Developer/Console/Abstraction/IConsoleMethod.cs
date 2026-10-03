namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines a method that can be executed with a set of string arguments in a console context.
    /// </summary>
    /// <remarks>Implementations of this interface should define the behavior of the <see cref="Execute"/>
    /// method, which processes the provided arguments and returns a result. The result type and behavior are determined
    /// by the specific implementation.</remarks>
    public interface IConsoleMethod : IConsoleCommand
    {
        /// <summary>
        /// Executes a command based on the provided arguments and returns the result.
        /// </summary>
        /// <param name="arguments">An array of strings representing the arguments for the command. Cannot be null.</param>
        /// <returns>An object representing the result of the command execution. The type and content of the result depend on the
        /// specific command.</returns>
        object Execute(string[] arguments);
    }
}
