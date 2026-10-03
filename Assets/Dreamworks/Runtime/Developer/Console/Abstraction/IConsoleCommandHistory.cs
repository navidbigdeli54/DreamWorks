using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    internal interface IConsoleCommandHistory
    {
        IReadOnlyCollection<FConsoleCommandHistoryRecord> HistoryRecords { get; }
    }
}
