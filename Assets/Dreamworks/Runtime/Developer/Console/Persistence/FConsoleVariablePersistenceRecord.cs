using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence
{
    /// <summary>
    /// Stores one persistent console variable as its name and serialized current value.
    /// </summary>
    public sealed class FConsoleVariablePersistenceRecord : IJsonSerializable, IJsonDeserializable
    {
        #region Properties
        /// <summary>
        /// Gets the registered variable name used as the persistence key.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the invariant string representation of the variable's current value.
        /// </summary>
        public string Value { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty record for JSON deserialization.
        /// </summary>
        public FConsoleVariablePersistenceRecord() { }

        /// <summary>
        /// Creates a persistence record for a variable's current value.
        /// </summary>
        public FConsoleVariablePersistenceRecord(string name, string value)
        {
            Name = name;

            Value = value;
        }
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new();
            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;
            jsonObject[nameof(Name)] = Name;
            jsonObject[nameof(Value)] = Value;
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
            Name = jsonObject[nameof(Name)];
            Value = jsonObject[nameof(Value)];
        }
        #endregion
    }
}
