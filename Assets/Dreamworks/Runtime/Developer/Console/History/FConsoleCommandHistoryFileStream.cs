using System;
using System.IO;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History
{
    internal class FConsoleCommandHistoryFileStream : IDeveloperConsoleInitializer
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly FConsoleCommandHistoryRepository repository;

        private readonly string filePath;
        #endregion

        #region Constructors
        public FConsoleCommandHistoryFileStream(ILogProvider logProvider, FConsoleCommandHistoryRepository repository, string filePath)
        {
            this.logProvider = logProvider;

            this.repository = repository;
            this.filePath = filePath;
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
