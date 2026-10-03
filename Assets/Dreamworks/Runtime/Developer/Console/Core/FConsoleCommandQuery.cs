using System;
using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Dummy;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Provides a composite implementation of <see cref="IConsoleCommandQuery"/> that aggregates suggestions from
    /// multiple repositories.
    /// </summary>
    /// <remarks>This class combines the results of querying two separate <see cref="IConsoleCommandQuery"/>
    /// instances: one for method-based commands and one for variable-based commands. The results are merged, sorted
    /// alphabetically by name, and returned as a single list of suggestions.</remarks>
    internal sealed class FConsoleCommandQuery : IConsoleCommandQuery
    {
        #region Fields
        private readonly IConsoleCommandQuery methodRepository;

        private readonly IConsoleCommandQuery variableRepository;
        #endregion

        #region Constructors
        internal FConsoleCommandQuery(IConsoleCommandQuery methodRepository, IConsoleCommandQuery variableRepository)
        {
            this.methodRepository = methodRepository ?? FDummyConsoleCommandQuery.Empty;

            this.variableRepository = variableRepository ?? FDummyConsoleCommandQuery.Empty;
        }
        #endregion

        #region IDeveloperConsoleQuery Implementation
        IReadOnlyList<FConsoleCommandSuggestion> IConsoleCommandQuery.QuerySuggestions(string text)
        {
            List<FConsoleCommandSuggestion> results = new();

            results.AddRange(methodRepository.QuerySuggestions(text));

            results.AddRange(variableRepository.QuerySuggestions(text));

            return results.OrderBy(x => x.Name).ToList();
        }
        #endregion
    }
}