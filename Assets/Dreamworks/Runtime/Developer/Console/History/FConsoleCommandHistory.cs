using System;
using System.Collections.Generic;
using System.IO;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History
{
    internal sealed class FConsoleCommandHistory : IConsoleCommandHistory, IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IConsoleCommandActivator commandActivator;

        private readonly FConsoleCommandHistoryRepository repository;

        private readonly FConsoleCommandHistoryFileStream fileStream;
        #endregion

        #region Constructors
        internal FConsoleCommandHistory(ILogProvider logProvider, IConsoleCommandActivator commandActivator, int maxEntries = 256, string fileName = "commands.bin")
        {
            this.logProvider = logProvider;

            this.commandActivator = commandActivator;

            repository = new FConsoleCommandHistoryRepository(maxEntries);

            fileStream = new FConsoleCommandHistoryFileStream(logProvider, repository, Path.Combine(UnityEngine.Application.persistentDataPath, fileName));
        }
        #endregion

        #region IConsoleCommandHistory Implementation
        IReadOnlyCollection<FConsoleCommandHistoryRecord> IConsoleCommandHistory.HistoryRecords => repository.Records;
        #endregion

        #region IConsoleInitializer
        void IDeveloperConsoleInitializer.Initialize()
        {
            commandActivator.OnCommandEntered += OnCommandEntered;

            if (fileStream is IDeveloperConsoleInitializer fileStreamInitializer)
            {
                fileStreamInitializer.Initialize();
            }
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {
            commandActivator.OnCommandEntered -= OnCommandEntered;

            if (fileStream is IDeveloperConsoleInitializer fileStreamInitializer)
            {
                fileStreamInitializer.ShutDown();
            }
        }
        #endregion

        #region Private Methods
        private void OnCommandEntered(string command)
        {
            repository.AddRecord(command);
        }
        #endregion
    }
}