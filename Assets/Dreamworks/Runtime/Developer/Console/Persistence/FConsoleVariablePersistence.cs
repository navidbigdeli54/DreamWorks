using System;
using System.IO;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence
{
    /// <summary>
    /// Provides functionality for persisting console variable values to a file in Unity's per-user persistent data
    /// directory.
    /// </summary>
    /// <remarks>This class implements <see cref="IConsoleVariablePersistence"/> to enable saving, loading,
    /// and retrieving console variable values. The persistence mechanism uses a JSON file stored in Unity's persistent
    /// data path, under a "DreamWorks/Console" subdirectory.</remarks>
    internal sealed class FConsoleVariablePersistence : IConsoleVariablePersistence
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly string filePath;

        private readonly FConsoleVariablePersistenceRepository repository;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FConsoleVariablePersistence"/> class, specifying the log
        /// provider and the file name for persistence.
        /// </summary>
        /// <remarks>The file path for persistence is constructed using the application's persistent data
        /// path, combined with the "DreamWorks/Console" directory and the specified file name.</remarks>
        /// <param name="logProvider">The log provider used for logging operations. Cannot be <see langword="null"/>.</param>
        /// <param name="fileName">The name of the file used to persist console variable data. Must not be <see langword="null"/> or empty.</param>
        public FConsoleVariablePersistence(ILogProvider logProvider, string fileName)
        {
            this.logProvider = logProvider;

            filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "DreamWorks", "Console", fileName);

            repository = new FConsoleVariablePersistenceRepository();
        }
        #endregion

        #region IConsoleVariablePersistence Implementation
        void IConsoleVariablePersistence.Load()
        {
            if (!File.Exists(filePath))
            {
                logProvider.LogVerbose($"No saved console variables found at \"{filePath}\"; defaults will be used.");

                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    logProvider.LogWarning($"Saved console variable file \"{filePath}\" is empty; defaults will be used.");

                    return;
                }

                ((IJsonDeserializable)repository).FromJson(FJsonNode.Parse(json) as FJsonObject);
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception, $"Failed to load saved console variables from \"{filePath}\".");

                return;
            }

            logProvider.LogMessage($"Loaded {repository.Records.Count} saved console variable value(s) from \"{filePath}\".");
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

            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, ((IJsonSerializable)repository).ToJson().ToString());
            }
            catch (Exception exception)
            {
                logProvider.LogError(exception, $"Failed to save {records.Count} console variable value(s) to \"{filePath}\".");

                return;
            }

            logProvider.LogVeryVerbose($"Wrote {records.Count} console variable value(s) to \"{filePath}\".");
        }
        #endregion
    }
}
