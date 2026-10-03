using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Represents a query interface for retrieving suggestions based on a given input text.
    /// </summary>
    /// <remarks>This interface is designed to provide a mechanism for querying and retrieving a list of 
    /// command suggestions that match or are relevant to the specified input text.</remarks>
    internal interface IConsoleCommandQuery
    {
        #region Methods
        /// <summary>
        /// Queries and retrieves a list of suggestions based on the provided input text.
        /// </summary>
        /// <param name="text">The input text to query suggestions for. Cannot be null or empty.</param>
        /// <returns>A read-only list of <see cref="FConsoleCommandSuggestion"/> objects representing the suggestions that match
        /// the input text. Returns an empty list if no suggestions are found.</returns>
        IReadOnlyList<FConsoleCommandSuggestion> QuerySuggestions(string text);
        #endregion
    }
}
