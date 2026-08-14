using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public sealed class FConsoleHistory
    {
        #region Constants
        private const string DefaultFileName = "ConsoleHistory.txt";
        #endregion

        #region Fields
        private readonly ILogProvider logProvider;

        private readonly List<string> entries = new();

        private readonly string historyFilePath;

        private readonly int maxEntries;

        private int historyIndex;
        #endregion

        #region Properties
        public IReadOnlyList<string> Entries => entries;
        #endregion

        #region Constructors
        public FConsoleHistory(ILogProvider logProvider, string historyFilePath = null, int maxEntries = 256)
        {
            this.logProvider = logProvider;

            this.maxEntries = Math.Max(1, maxEntries);

            this.historyFilePath = string.IsNullOrWhiteSpace(historyFilePath) ? Path.Combine(UnityEngine.Application.persistentDataPath, DefaultFileName) : historyFilePath;

            historyIndex = 0;
        }
        #endregion

        #region Public Methods
        public void Load()
        {
            entries.Clear();

            if (!File.Exists(historyFilePath))
            {
                historyIndex = 0;

                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(historyFilePath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    entries.Add(line);
                }

                TrimToMaximum();

                historyIndex = entries.Count;

                logProvider.Log($"Loaded {entries.Count} console history entries.");
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Failed to load console history.\n{exception}");
            }
        }

        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(historyFilePath);

                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllLines(historyFilePath, entries);

                logProvider.Log($"Saved {entries.Count} console history entries.");
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Failed to save console history.\n{exception}");
            }
        }

        public void Clear()
        {
            entries.Clear();

            historyIndex = 0;
        }

        public void Add(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            command = command.Trim();

            if (entries.Count > 0 && string.Equals(entries[^1], command, StringComparison.Ordinal))
            {
                historyIndex = entries.Count;

                return;
            }

            entries.Add(command);

            TrimToMaximum();

            historyIndex = entries.Count;
        }

        public string GetPrevious()
        {
            if (entries.Count == 0)
            {
                return string.Empty;
            }

            historyIndex = Math.Max(0, historyIndex - 1);

            return entries[historyIndex];
        }

        public string GetNext()
        {
            if (entries.Count == 0)
            {
                return string.Empty;
            }

            historyIndex = Math.Min(entries.Count, historyIndex + 1);

            if (historyIndex >= entries.Count)
            {
                return string.Empty;
            }

            return entries[historyIndex];
        }

        public void ResetNavigation()
        {
            historyIndex = entries.Count;
        }

        public bool Contains(string command)
        {
            return entries.Contains(command);
        }

        public IEnumerable<string> Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return entries;
            }

            return entries.Where(x => x.Contains(text, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Private Methods
        private void TrimToMaximum()
        {
            while (entries.Count > maxEntries)
            {
                entries.RemoveAt(0);
            }
        }
        #endregion
    }
}