using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Represents a console object with a name, description, and type.
    /// </summary>
    /// <remarks>This interface defines the basic structure for objects that can be used in a console-based
    /// application. Implementations of this interface should provide meaningful values for the <see cref="Name"/>, <see
    /// cref="Description"/>,  and <see cref="CommandType"/> properties to describe the object and its purpose.</remarks>
    public interface IConsoleCommand
    {
        #region Properties
        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description associated with the current object.
        /// </summary>
        string Description { get; }

        EConsoleCommandType CommandType { get; } 
        #endregion
    }
}
