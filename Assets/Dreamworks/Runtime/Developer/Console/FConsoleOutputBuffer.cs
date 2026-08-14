using System;
using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public sealed class FConsoleOutputBuffer
    {
        #region Events
        public event Action<FConsoleOutputEntry> OnEntryAdded;
        #endregion

        #region Fields
        private readonly int maxEntries;

        private readonly List<FConsoleOutputEntry> entries = new();
        #endregion

        #region Properties
        public IReadOnlyList<FConsoleOutputEntry> Entries => entries;
        #endregion

        #region Constructors
        public FConsoleOutputBuffer(int maxEntries = 1000)
        {
            this.maxEntries = Math.Max(1, maxEntries);
        }
        #endregion

        #region Public Methods
        public void Clear()
        {
            entries.Clear();
        }

        public void Add(string message, EConsoleOutputType messageType = EConsoleOutputType.Log)
        {
            FConsoleOutputEntry entry = new(message, messageType);

            entries.Add(entry);

            while (entries.Count > maxEntries)
            {
                entries.RemoveAt(0);
            }

            OnEntryAdded?.Invoke(entry);
        }

        public void Log(string message)
        {
            Add(message, EConsoleOutputType.Log);
        }

        public void Warning(string message)
        {
            Add(message, EConsoleOutputType.Warning);
        }

        public void Error(string message)
        {
            Add(message, EConsoleOutputType.Error);
        }

        public void Command(string message)
        {
            Add(message, EConsoleOutputType.Command);
        }
        #endregion
    }
}