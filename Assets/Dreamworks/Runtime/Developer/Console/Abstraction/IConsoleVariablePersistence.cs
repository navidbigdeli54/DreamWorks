using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Defines methods for managing the persistence of console variable values.
    /// </summary>
    /// <remarks>This interface provides functionality to load, retrieve, and save persistent console variable
    /// values. Implementations of this interface are responsible for ensuring that variable states are correctly
    /// persisted and retrieved as needed. Call <see cref="Load"/> before using other methods to ensure the persistence
    /// system is properly initialized.</remarks>
    public interface IConsoleVariablePersistence
    {
        /// <summary>
        /// Loads the necessary resources or data required for the operation.
        /// </summary>
        /// <remarks>This method initializes and prepares the system for subsequent operations.  Ensure
        /// that this method is called before invoking any dependent functionality.</remarks>
        void Load();

        /// <summary>
        /// Attempts to retrieve the value associated with the specified variable name.
        /// </summary>
        /// <param name="variableName">The name of the variable to look up. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified variable name, if the variable
        /// name exists; otherwise, contains <see langword="null"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the value associated with the specified variable name was found; otherwise, <see
        /// langword="false"/>.</returns>
        bool TryGetValue(string variableName, out string value);

        /// <summary>
        /// Saves the specified collection of console variable persistence records.
        /// </summary>
        /// <remarks>This method persists the provided records, ensuring their values are stored for
        /// future retrieval.  The behavior of the save operation depends on the implementation and may involve writing
        /// to a file,  database, or other storage medium.</remarks>
        /// <param name="records">A read-only list of <see cref="FConsoleVariablePersistenceRecord"/> objects to be saved.  The list must not
        /// be null or empty.</param>
        void Save(IReadOnlyList<FConsoleVariablePersistenceRecord> records);
    }
}
