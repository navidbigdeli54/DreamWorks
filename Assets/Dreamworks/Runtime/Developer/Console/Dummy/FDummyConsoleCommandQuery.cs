using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Dummy
{
    /// <summary>
    /// Represents a no-operation implementation of the <see cref="IConsoleCommandQuery"/> interface.
    /// </summary>
    /// <remarks>This class provides an empty result for all query operations and is intended to be used as a
    /// default or placeholder implementation.</remarks>
    internal sealed class FDummyConsoleCommandQuery : IConsoleCommandQuery
    {
        #region Fields
        private readonly IReadOnlyList<FConsoleCommandSuggestion> dummyResult = new List<FConsoleCommandSuggestion>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets an empty instance of <see cref="IConsoleCommandQuery"/> that represents a no-operation query.
        /// </summary>
        public static IConsoleCommandQuery Empty { get; } = new FDummyConsoleCommandQuery();
        #endregion

        #region Constructors
        private FDummyConsoleCommandQuery()
        {

        }
        #endregion

        #region IDeveloperConsoleCommandQuery Implementation
        IReadOnlyList<FConsoleCommandSuggestion> IConsoleCommandQuery.QuerySuggestions(string text)
        {
            return new List<FConsoleCommandSuggestion>();
        } 
        #endregion
    }
}