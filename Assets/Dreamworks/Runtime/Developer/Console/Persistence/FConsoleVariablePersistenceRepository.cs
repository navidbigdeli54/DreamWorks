using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence
{
    /// <summary>
    /// Owns the versioned set of persistent console-variable records.
    /// </summary>
    internal sealed class FConsoleVariablePersistenceRepository : IJsonSerializable, IJsonDeserializable
    {
        #region Properties
        /// <summary>
        /// Gets the currently persisted records, keyed case-insensitively by variable name.
        /// </summary>
        public List<FConsoleVariablePersistenceRecord> Records { get; } = new();
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new();

            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;

            FJsonArray recordArray = new();
            for (int index = 0; index < Records.Count; index++)
            {
                recordArray.Add(((IJsonSerializable)Records[index]).ToJson());
            }
            jsonObject[nameof(Records)] = recordArray;

            return jsonObject;
        }
        #endregion

        #region IJsonDeserializable Implementation
        void IJsonDeserializable.FromJson(FJsonObject jsonObject)
        {
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
            Records.Clear();

            FJsonArray recordArray = jsonObject[nameof(Records)] as FJsonArray;

            if (recordArray == null)
            {
                return;
            }

            for (int index = 0; index < recordArray.Count; index++)
            {
                FConsoleVariablePersistenceRecord record = new();

                ((IJsonDeserializable)record).FromJson(recordArray[index] as FJsonObject);

                if (!string.IsNullOrWhiteSpace(record.Name))
                {
                    Records.Add(record);
                }
            }
        }
        #endregion
    }
}
