using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Output
{
    internal sealed class FConsoleCommandOutputBuffer : IConsoleCommandOutputBuffer, IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly List<FConsoleCommandOutputEntry> entries;

        private readonly ILogProvider logProvider;

        private readonly IConsoleCommandActivator commandActivator;
        #endregion

        #region Constructors
        internal FConsoleCommandOutputBuffer(ILogProvider logProvider, IConsoleCommandActivator commandActivator)
        {
            this.logProvider = logProvider;

            this.commandActivator = commandActivator;

            entries = new List<FConsoleCommandOutputEntry>();
        }
        #endregion

        #region IConsoleCommandOutputBuffer Implementation
        public event Action<FConsoleCommandOutputEntry> OnEntryAdded;

        IReadOnlyCollection<FConsoleCommandOutputEntry> IConsoleCommandOutputBuffer.OutputEntries => entries;
        #endregion

        #region IDeveloperConsoleInitializer Implementation
        void IDeveloperConsoleInitializer.Initialize()
        {
            commandActivator.OnCommandExecuted += OnCommandExecuted;
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {
            commandActivator.OnCommandExecuted -= OnCommandExecuted;
        }
        #endregion

        #region Private Methods
        private void OnCommandExecuted(FConsoleCommandExecutionResult result)
        {
            var entry = new FConsoleCommandOutputEntry(result.Message, result.WasSuccessful ? EConsoleCommandOutputType.Log : EConsoleCommandOutputType.Error);

            entries.Add(entry);

            if (result.WasSuccessful)
            {
                logProvider.Log(result.Message);
            }
            else
            {
                logProvider.LogError(result.Message);
            }

            OnEntryAdded?.Invoke(entry);
        }
        #endregion
    }
}