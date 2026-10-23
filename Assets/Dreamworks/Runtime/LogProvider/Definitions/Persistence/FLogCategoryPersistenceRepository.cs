using System;
using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Definitions.Persitence
{
    /// <summary>
    /// Provides functionality for managing and persisting log category settings, including retrieval, insertion, and
    /// serialization to and from JSON.
    /// </summary>
    /// <remarks>This repository is designed to store and manage instances of <see
    /// cref="FLogCategorySaveableData" />. It supports operations such as retrieving a specific log category by name,
    /// adding or updating log category records, and serializing/deserializing the data to and from JSON
    /// format.</remarks>
    internal sealed class FLogCategoryPersistenceRepository : IJsonSerializable, IJsonDeserializable
    {
        #region Properties
        public List<FLogCategorySaveableData> SaveableDatas { get; } = new List<FLogCategorySaveableData>();
        #endregion

        #region Public Methods
        public bool TryGet(string categoryName, out FLogCategorySaveableData record)
        {
            for (int index = SaveableDatas.Count - 1; index >= 0; index--)
            {
                if (string.Equals(SaveableDatas[index].Category, categoryName, StringComparison.OrdinalIgnoreCase))
                {
                    record = SaveableDatas[index];
                    return true;
                }
            }

            record = null;
            return false;
        }

        public void Upsert(FLogCategorySaveableData record)
        {
            if (record == null || !record.IsValid())
            {
                throw new ArgumentException("A valid log category settings record is required.", nameof(record));
            }

            for (int index = SaveableDatas.Count - 1; index >= 0; index--)
            {
                if (string.Equals(SaveableDatas[index].Category, record.Category, StringComparison.OrdinalIgnoreCase))
                {
                    SaveableDatas[index] = record;
                    return;
                }
            }

            SaveableDatas.Add(record);
        }
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;

            FJsonArray records = new FJsonArray();
            for (int index = 0; index < SaveableDatas.Count; index++)
            {
                records.Add(((IJsonSerializable)SaveableDatas[index]).ToJson());
            }

            jsonObject[nameof(SaveableDatas)] = records;
            return jsonObject;
        }
        #endregion

        #region IJsonDeserializable Implementation
        void IJsonDeserializable.FromJson(FJsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            int version = jsonObject[nameof(IJsonSerializable.Version)];
            if (version == 1)
            {
                FromJsonV1(jsonObject);
            }
            else
            {
                throw new FInvalidJsonVersionException();
            }

            
        }
        #endregion

        #region Private Methods
        private void FromJsonV1(FJsonObject jsonObject)
        {
            FJsonArray records = jsonObject[nameof(SaveableDatas)] as FJsonArray;
            if (records == null)
            {
                return;
            }

            for (int index = 0; index < records.Count; index++)
            {
                if (records[index] is not FJsonObject recordJson)
                {
                    continue;
                }

                try
                {
                    FLogCategorySaveableData record = new FLogCategorySaveableData();
                    ((IJsonDeserializable)record).FromJson(recordJson);
                    Upsert(record);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Skipping invalid log category persistence record at index {index}. {exception}");
                }
            }
        }
        #endregion
    }
}
