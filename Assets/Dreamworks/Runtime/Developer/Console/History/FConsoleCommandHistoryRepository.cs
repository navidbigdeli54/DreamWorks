using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History
{

    internal class FConsoleCommandHistoryRepository : IJsonSerializable, IJsonDeserializable
    {
        #region Fields
        private readonly int maxEntries;
        #endregion

        #region Properties
        public List<FConsoleCommandHistoryRecord> Records { get; }
        #endregion

        #region Constructors
        public FConsoleCommandHistoryRepository(int maxEntries)
        {
            this.maxEntries = System.Math.Max(1, maxEntries);

            Records = new List<FConsoleCommandHistoryRecord>(this.maxEntries);
        }
        #endregion

        #region Public Methods
        public void AddRecord(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            command = command.Trim();
            if (Records.Count > 0 && Records[^1].Command == command)
            {
                return;
            }

            AddRecord(new FConsoleCommandHistoryRecord(command));
        }
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;

            var recordArray = new FJsonArray();
            for (int i = 0; i < Records.Count; ++i)
            {
                recordArray.Add(((IJsonSerializable)Records[i]).ToJson());
            }

            jsonObject[nameof(Records)] = recordArray;
            return jsonObject;
        }
        #endregion

        #region IJsonDeserializable Implementation
        void IJsonDeserializable.FromJson(FJsonObject jsonObject)
        {
            int version = jsonObject[nameof(IJsonSerializable.Version)];
            if (version != 1)
            {
                throw new FInvalidJsonVersionException();
            }

            FromJsonV1(jsonObject);
        }
        #endregion

        #region Private Methods
        private void FromJsonV1(FJsonObject jsonObject)
        {
            Records.Clear();

            var recordArray = jsonObject[nameof(Records)] as FJsonArray;
            if (recordArray == null)
            {
                return;
            }

            for (int i = 0; i < recordArray.Count; ++i)
            {
                var record = new FConsoleCommandHistoryRecord();
                ((IJsonDeserializable)record).FromJson(recordArray[i] as FJsonObject);
                AddRecord(record);
            }
        }

        private void AddRecord(FConsoleCommandHistoryRecord record)
        {
            if (Records.Count == maxEntries)
            {
                Records.RemoveAt(0);
            }

            Records.Add(record);
        }
        #endregion
    }
}
