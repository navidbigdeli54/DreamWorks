using System;
using System.IO;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History.Persistence
{
    /// <summary>
    /// Manages the persistence of console command history to and from a file.
    /// </summary>
    /// <remarks>This class implements <see cref="IDeveloperConsoleInitializer"/> to handle the initialization
    /// and shutdown of the developer console's command history. During initialization, it loads the command history
    /// from the specified file. During shutdown, it saves the current command history back to the file.</remarks>
    internal class FConsoleCommandHistoryPersistence : IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly FConsoleCommandHistoryRepository repository;

        private readonly string filePath;
        #endregion

        #region Constructors
        public FConsoleCommandHistoryPersistence(ILogProvider logProvider, FConsoleCommandHistoryRepository repository, string fileName)
        {
            this.logProvider = logProvider;

            this.repository = repository;

            this.filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "DreamWorks", "Console", fileName); ;
        }
        #endregion

        #region IConsoleInitializer Implementation 
        void IDeveloperConsoleInitializer.Initialize()
        {
            Load();
        }

        void IDeveloperConsoleInitializer.ShutDown()
        {
            Save();
        }
        #endregion

        #region Private Methods
        private void Load()
        {
            if (!File.Exists(filePath))
            {
                logProvider.LogVerbose($"No saved console command history found at \"{filePath}\".");
                return;
            }

            try
            {
                string strigifiedJson = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(strigifiedJson))
                {
                    logProvider.LogWarning($"Saved console command history file \"{filePath}\" is empty.");
                    return;
                }

                ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(strigifiedJson) as FJsonObject);
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception, $"Failed to load console command history from \"{filePath}\".");
                throw;
            }

            logProvider.LogMessage($"Loaded {repository.Records.Count} console command history record(s) from \"{filePath}\".");
        }

        private void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = ((IJsonSerializable)repository).ToJson().ToString();

                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception, $"Failed to save console command history to \"{filePath}\".");
                throw;
            }

            logProvider.LogMessage($"Saved {repository.Records.Count} console command history record(s) to \"{filePath}\".");
        }
        #endregion
    }
}
