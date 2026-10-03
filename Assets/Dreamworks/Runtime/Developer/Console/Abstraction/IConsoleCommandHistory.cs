using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Represents a read-only collection of console command history records.
    /// </summary>
    /// <remarks>This interface provides access to the history of executed console commands through the  <see
    /// cref="HistoryRecords"/> property. The collection is immutable and reflects the current  state of the command
    /// history at the time of access.</remarks>
    internal interface IConsoleCommandHistory
    {
        /// <summary>
        /// Gets the collection of command history records.
        /// </summary>
        IReadOnlyCollection<FConsoleCommandHistoryRecord> HistoryRecords { get; }
    }
}
