namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines a repository for managing console variables, including their registration, retrieval, and removal.
    /// </summary>
    /// <remarks>Console variables are named entities that can store values and are typically used for
    /// configuration or debugging purposes. This interface provides methods to register new variables, unregister
    /// existing ones, and retrieve variables or their values.</remarks>
    public interface IConsoleVariableRepository
    {
        #region Methods
        /// <summary>
        /// Registers a console variable to make it available for use within the application.
        /// </summary>
        /// <remarks>Once registered, the variable can be accessed and manipulated through the console
        /// interface. Ensure that the variable name is unique to avoid conflicts with existing variables.</remarks>
        /// <param name="variable">The console variable to register. Must not be <see langword="null"/>.</param>
        void RegisterVariable(IConsoleVariable variable);

        /// <summary>
        /// Registers a variable with the specified name, default value, and description.
        /// </summary>
        /// <remarks>This method is used to define a variable that can be referenced later by its name. 
        /// The <paramref name="name"/> must be unique within the context of the registration system.</remarks>
        /// <typeparam name="TVariableType">The type of the variable to register.</typeparam>
        /// <param name="name">The unique name of the variable. Cannot be null or empty.</param>
        /// <param name="defaultValue">The default value assigned to the variable.</param>
        /// <param name="description">A brief description of the variable's purpose. Cannot be null or empty.</param>
        void RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description);

        /// <summary>
        /// Unregisters a variable by its name, removing it from the system.
        /// </summary>
        /// <remarks>This method removes the specified variable from the system. If the variable does not
        /// exist, no action is taken.</remarks>
        /// <param name="variableName">The name of the variable to unregister. Cannot be null or empty.</param>
        void UnregisterVariable(string variableName);

        /// <summary>
        /// Attempts to retrieve a console variable by its name.
        /// </summary>
        /// <param name="name">The name of the console variable to retrieve. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="variable">When this method returns, contains the console variable associated with the specified name,  if the name
        /// exists; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if a console variable with the specified name exists; otherwise, <see
        /// langword="false"/>.</returns>
        bool TryGetVariable(string name, out IConsoleVariable variable);

        /// <summary>
        /// Attempts to retrieve the value of a variable with the specified name and type.
        /// </summary>
        /// <remarks>This method does not throw an exception if the variable is not found or if the value
        /// cannot be cast to the specified type. Instead, it returns <see langword="false"/> and sets <paramref name="value"/> to the default value of <typeparamref name="TVariableType"/>.</remarks>
        /// <typeparam name="TVariableType">The expected type of the variable's value.</typeparam>
        /// <param name="name">The name of the variable to retrieve. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="value">When this method returns, contains the value of the variable if found and successfully cast to 
        /// <typeparamref name="TVariableType"/>; otherwise, the default value of <typeparamref name="TVariableType"/>.
        /// This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the variable with the specified name exists and its value can be cast to 
        /// <typeparamref name="TVariableType"/>; otherwise, <see langword="false"/>.</returns>
        bool TryGetVariableValue<TVariableType>(string name, out TVariableType value);
        #endregion
    }
}
