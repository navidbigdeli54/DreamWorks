using System;
using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Dummy;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
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