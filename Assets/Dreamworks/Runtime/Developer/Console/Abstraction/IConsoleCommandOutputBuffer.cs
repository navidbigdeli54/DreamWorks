using System;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    internal interface IConsoleCommandOutputBuffer
    {
        IReadOnlyCollection<FConsoleCommandOutputEntry> OutputEntries { get; }

        event Action<FConsoleCommandOutputEntry> OnEntryAdded;
    }
}
