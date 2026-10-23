using System;
using DreamMachineGameStudio.DreamWorks.Serialization.Json;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Exceptions;
using DreamMachineGameStudio.DreamWorks.Serialization.Json.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Definitions.Persitence
{
    /// <summary>
    /// Represents the saveable data for a log category, including its name, enabled state, and verbosity threshold.
    /// </summary>
    /// <remarks>This class is used to serialize and deserialize log category settings to and from JSON. It
    /// ensures that the log category data is valid and adheres to the expected format. The <see cref="IsValid"/> method
    /// can be used to verify the integrity of the data.</remarks>
    internal sealed class FLogCategorySaveableData : IJsonSerializable, IJsonDeserializable
    {
        #region Properties
        /// <summary>
        /// Gets the category associated with the current instance.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the feature is currently enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the minimum verbosity level required for log messages to be processed.
        /// </summary>
        /// <remarks>Log messages with a verbosity level lower than this threshold will be
        /// ignored.</remarks>
        public ELogVerbosity VerbosityThreshold { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FLogCategorySaveableData"/> class.
        /// </summary>
        public FLogCategorySaveableData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FLogCategorySaveableData"/> class with the specified category
        /// name, enabled state, and verbosity threshold.
        /// </summary>
        /// <param name="category">The name of the log category. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="isEnabled">A value indicating whether logging is enabled for this category. <see langword="true"/> if enabled;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="verbosityThreshold">The verbosity level threshold for this log category. Messages below this threshold will not be logged.</param>
        public FLogCategorySaveableData(string category, bool isEnabled, ELogVerbosity verbosityThreshold)
        {
            Category = category;
            IsEnabled = isEnabled;
            VerbosityThreshold = verbosityThreshold;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the current configuration is valid.
        /// </summary>
        /// <remarks>A configuration is considered valid if the <see cref="Category"/> property is not
        /// null, empty, or whitespace, and the <see cref="VerbosityThreshold"/> property is a defined value in the <see
        /// cref="ELogVerbosity"/> enumeration.</remarks>
        /// <returns><see langword="true"/> if the configuration is valid; otherwise, <see langword="false"/>.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Category) && Enum.IsDefined(typeof(ELogVerbosity), VerbosityThreshold);
        }
        #endregion

        #region IJsonSerializable Implementation
        int IJsonSerializable.Version => 1;

        FJsonObject IJsonSerializable.ToJson()
        {
            FJsonObject jsonObject = new FJsonObject();
            jsonObject[nameof(IJsonSerializable.Version)] = ((IJsonSerializable)this).Version;
            jsonObject[nameof(Category)] = Category;
            jsonObject[nameof(IsEnabled)] = IsEnabled;
            jsonObject[nameof(VerbosityThreshold)] = (int)VerbosityThreshold;
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
        void FromJsonV1(FJsonObject jsonObject)
        {
            Category = jsonObject[nameof(Category)];
            IsEnabled = jsonObject[nameof(IsEnabled)];
            VerbosityThreshold = (ELogVerbosity)(int)jsonObject[nameof(VerbosityThreshold)];

            if (!IsValid())
            {
                throw new FormatException("The log category settings record contains invalid values.");
            }
        }
        #endregion
    }
}
