using System;
using System.IO;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
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

            this.filePath = Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
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
                return;
            }

            try
            {
                string strigifiedJson = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(strigifiedJson))
                {
                    return;
                }

                ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(strigifiedJson) as FJsonObject);

                logProvider.Log($"Console command history has been loaded.");
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Failed to load console command history. {exception}");
            }
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

                logProvider.Log($"Console command history has been saved.");
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Failed to save console command history.\n {exception}");
            }
        }
        #endregion
    }
}
