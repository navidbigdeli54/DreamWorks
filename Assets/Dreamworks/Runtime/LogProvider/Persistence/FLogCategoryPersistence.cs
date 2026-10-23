using System;
using System.IO;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions.Persitence;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction.Persistence;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Persistence
{
    /// <summary>
    /// Provides functionality for persisting log category data to a file.
    /// </summary>
    /// <remarks>This class implements the <see cref="ILogCategoryPersistence"/> interface to manage the
    /// storage and retrieval of log category settings. Data is serialized to and deserialized from a JSON file located
    /// in the application's persistent data path. If the file does not exist, default settings are used.</remarks>
    internal sealed class FLogCategoryPersistence : ILogCategoryPersistence
    {
        #region Fields
        private readonly string filePath;

        private readonly FLogCategoryPersistenceRepository repository = new();
        #endregion

        #region Constructors
        public FLogCategoryPersistence(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("A persistence file name is required.", nameof(fileName));
            }

            filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "DreamWorks", "LogProvider", fileName);
        }
        #endregion

        #region ILogCategoryPersistence Implementation
        void ILogCategoryPersistence.Load()
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
                    FJsonObject jsonObject = FJsonNode.Parse(json) as FJsonObject;

                    ((IJsonDeserializable)repository).FromJson(jsonObject);
                }
            }
            catch (Exception exception)
            {
                repository.SaveableDatas.Clear();
                Debug.LogWarning($"Failed to load log category settings. Defaults will be used. {exception}");
            }
        }

        bool ILogCategoryPersistence.TryGet(string categoryName, out FLogCategorySaveableData record)
        {
            return repository.TryGet(categoryName, out record);
        }

        void ILogCategoryPersistence.Save(FLogCategorySaveableData record)
        {
            repository.Upsert(record);

            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string temporaryPath = filePath + ".tmp";
            File.WriteAllText(temporaryPath, ((IJsonSerializable)repository).ToJson().ToString(0));

            if (File.Exists(filePath))
            {
                File.Copy(temporaryPath, filePath, true);
                File.Delete(temporaryPath);
            }
            else
            {
                File.Move(temporaryPath, filePath);
            }
        }
        #endregion
    }
}
