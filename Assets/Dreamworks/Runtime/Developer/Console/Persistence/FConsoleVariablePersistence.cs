using System;
using System.IO;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence
{
    /// <summary>
    /// Loads and saves persistent console-variable snapshots using the project's JSON serialization system.
    /// </summary>
    internal sealed class FConsoleVariablePersistence : IConsoleVariablePersistence
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly string filePath;

        private readonly FConsoleVariablePersistenceRepository repository;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates persistence backed by a file in Unity's per-user persistent data directory.
        /// </summary>
        public FConsoleVariablePersistence(ILogProvider logProvider, string fileName)
        {
            this.logProvider = logProvider ?? FDefaultLogger.Instance;

            filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "DreamWorks", "Console", fileName);

            repository = new FConsoleVariablePersistenceRepository();
        }
        #endregion

        #region IConsoleVariablePersistence Implementation
        void IConsoleVariablePersistence.Load()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(json) as FJsonObject);
                }

                logProvider.Log("Console variable values have been loaded.");
            }
            catch (Exception exception)
            {
                repository.Records.Clear();

                logProvider.LogError($"Failed to load console variable values. {exception}");
            }
        }

        bool IConsoleVariablePersistence.TryGetValue(string variableName, out string value)
        {
            value = null;

            for (int index = repository.Records.Count - 1; index >= 0; index--)
            {
                FConsoleVariablePersistenceRecord record = repository.Records[index];
                if (string.Equals(record.Name, variableName, StringComparison.OrdinalIgnoreCase))
                {
                    value = record.Value; return true;
                }
            }

            return false;
        }

        void IConsoleVariablePersistence.Save(IReadOnlyList<FConsoleVariablePersistenceRecord> records)
        {
            try
            {
                repository.Records.Clear();

                for (int index = 0; index < records.Count; index++)
                {
                    repository.Records.Add(records[index]);
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, ((IJsonSerializable)repository).ToJson().ToString());
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Failed to save console variable values. {exception}");
            }
        }
        #endregion
    }
}
