using System;
using System.IO;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
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
        private readonly string filePath;

        private readonly FConsoleVariablePersistenceRepository repository;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates persistence backed by a file in Unity's per-user persistent data directory.
        /// </summary>
        public FConsoleVariablePersistence(string fileName)
        {
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

            string json = File.ReadAllText(filePath);

            if (!string.IsNullOrWhiteSpace(json))
            {
                ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(json) as FJsonObject);
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
        #endregion
    }
}
