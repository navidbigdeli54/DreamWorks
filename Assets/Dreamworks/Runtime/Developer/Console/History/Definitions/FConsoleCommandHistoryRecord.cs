using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.History
{

    internal class FConsoleCommandHistoryRecord : IJsonSerializable, IJsonDeserializable
    {
        #region Properties
        public string Command { get; private set; }
        #endregion

        #region Constructors
        public FConsoleCommandHistoryRecord()
        {

        }

        public FConsoleCommandHistoryRecord(string command)
        {
            Command = command;
        }
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;
            jsonObject[nameof(Command)] = Command;
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
            Command = jsonObject[nameof(Command)];
        }
        #endregion
    }
}
