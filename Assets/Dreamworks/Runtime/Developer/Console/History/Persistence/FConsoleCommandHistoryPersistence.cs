using System.IO;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
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
        private readonly FConsoleCommandHistoryRepository repository;

        private readonly string filePath;
        #endregion

        #region Constructors
        public FConsoleCommandHistoryPersistence(FConsoleCommandHistoryRepository repository, string fileName)
        {
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
                return;
            }

            string strigifiedJson = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(strigifiedJson))
            {
                return;
            }

            ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(strigifiedJson) as FJsonObject);
        }

        private void Save()
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string jsonString = ((IJsonSerializable)repository).ToJson().ToString();

            File.WriteAllText(filePath, jsonString);
        }
        #endregion
    }
}
