using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    internal interface IConsoleCommandQuery
    {
        #region Methods
        IReadOnlyList<FConsoleCommandSuggestion> QuerySuggestions(string text);
        #endregion
    }
}
