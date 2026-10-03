using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Abstracts loading and snapshot-saving of persistent console-variable values.
    /// </summary>
    /// <remarks>Persistence receives only variable names and values. It does not know why or by whom a value changed.</remarks>
    public interface IConsoleVariablePersistence
    {
        /// <summary>
        /// Loads previously saved values into the persistence provider.
        /// </summary>
        void Load();

        /// <summary>
        /// Looks up a saved serialized value by variable name.
        /// </summary>
        bool TryGetValue(string variableName, out string value);

        /// <summary>
        /// Replaces the saved state with the current complete set of persistent variable values.
        /// </summary>
        void Save(IReadOnlyList<FConsoleVariablePersistenceRecord> records);
    }
}
