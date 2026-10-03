using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History.Persistence;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History
{
    /// <summary>
    /// Manages the history of console commands entered by the user, providing access to past commands and persisting
    /// the history to a file for future sessions.
    /// </summary>
    /// <remarks>This class implements <see cref="IConsoleCommandHistory"/> to expose the history of commands
    /// and <see cref="IDeveloperConsoleInitializer"/> to handle initialization and shutdown tasks related to the
    /// developer console. It maintains an in-memory repository of command history and persists it to a file for
    /// durability.</remarks>
    internal sealed class FConsoleCommandHistory : IConsoleCommandHistory, IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly IConsoleCommandActivator commandActivator;

        private readonly FConsoleCommandHistoryRepository repository;

        private readonly FConsoleCommandHistoryPersistence fileStream;
        #endregion

        #region Constructors
        internal FConsoleCommandHistory(ILogProvider logProvider, IConsoleCommandActivator commandActivator, int maxEntries = 256, string fileName = "commands.bin")
        {
            this.logProvider = logProvider;

            this.commandActivator = commandActivator;

            repository = new FConsoleCommandHistoryRepository(maxEntries);

            fileStream = new FConsoleCommandHistoryPersistence(logProvider, repository, fileName);
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