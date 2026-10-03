namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines a repository for managing console methods, including registering, unregistering,  and retrieving methods
    /// by name.
    /// </summary>
    /// <remarks>This interface provides functionality to manage console methods, allowing dynamic
    /// registration  and retrieval of methods by their names. It is designed to support scenarios where console 
    /// commands or methods need to be managed at runtime.</remarks>
    public interface IConsoleMethodRepository
    {
        #region Methods
        /// <summary>
        /// Registers a console method for execution.
        /// </summary>
        /// <remarks>Once registered, the method can be invoked through the console interface.  Ensure
        /// that the method is properly implemented and thread-safe if accessed concurrently.</remarks>
        /// <param name="method">The console method to register. Must not be <see langword="null"/>.</param>
        void RegisterMethod(IConsoleMethod method);

        /// <summary>
        /// Unregisters a previously registered method by its name.
        /// </summary>
        /// <remarks>This method removes the specified method from the registry, making it unavailable for
        /// further use. Ensure that the method name provided matches the name used during registration.</remarks>
        /// <param name="method">The name of the method to unregister. Cannot be null or empty.</param>
        void UnregisterMethod(string method);

        /// <summary>
        /// Attempts to retrieve a console method by its name.
        /// </summary>
        /// <remarks>This method performs a case-sensitive search for the specified method name. If the
        /// method is not found, the <paramref name="method"/> parameter will be set to <see langword="null"/>.</remarks>
        /// <param name="name">The name of the method to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="method">When this method returns, contains the <see cref="IConsoleMethod"/> instance associated with the specified
        /// name, if found; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if a method with the specified name was found; otherwise, <see langword="false"/>.</returns>
        bool TryGetMethod(string name, out IConsoleMethod method);
        #endregion
    }
}
