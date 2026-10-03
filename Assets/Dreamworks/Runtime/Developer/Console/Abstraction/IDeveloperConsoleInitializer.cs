namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines methods for initializing and shutting down the developer console.
    /// </summary>
    /// <remarks>Implement this interface to provide custom initialization and shutdown logic for a developer console.</remarks>
    internal interface IDeveloperConsoleInitializer
    {
        void Initialize();

        void ShutDown();
    }
}
