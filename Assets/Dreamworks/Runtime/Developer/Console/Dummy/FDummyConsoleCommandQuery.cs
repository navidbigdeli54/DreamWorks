using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Dummy
{
    internal sealed class FDummyConsoleCommandQuery : IConsoleCommandQuery
    {
        #region Fields
        private readonly IReadOnlyList<FConsoleCommandSuggestion> dummyResult = new List<FConsoleCommandSuggestion>();
        #endregion

        #region Properties
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