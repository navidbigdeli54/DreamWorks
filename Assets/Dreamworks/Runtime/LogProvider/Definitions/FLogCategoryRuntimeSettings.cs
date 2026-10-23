using System.Threading;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Definitions
{
    /// <summary>
    /// Represents the runtime settings for a specific logging category, including its name,  enabled state, and
    /// verbosity levels.
    /// </summary>
    /// <remarks>This class provides configuration for logging categories at runtime, allowing control  over
    /// whether logging is enabled and the verbosity levels used for filtering log messages.  The settings include a
    /// default verbosity level and a threshold level that can be adjusted  dynamically.</remarks>
    internal sealed class FLogCategoryRuntimeSettings
    {
        #region Properties
        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the feature is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the default verbosity level for logging operations.
        /// </summary>
        public ELogVerbosity DefaultVerbosity { get; private set; }

        /// <summary>
        /// Gets the verbosity threshold level for logging.
        /// </summary>
        public ELogVerbosity VerbosityThreshold { get; private set; }
        #endregion

        #region Constructors
        public FLogCategoryRuntimeSettings(string name, bool isEnabled, ELogVerbosity defaultVerbosity)
        {
            Name = name;

            DefaultVerbosity = defaultVerbosity;

            Apply(isEnabled, defaultVerbosity);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Configures the logging settings by enabling or disabling logging and setting the verbosity threshold.
        /// </summary>
        /// <param name="enabled">A value indicating whether logging is enabled. <see langword="true"/> to enable logging; otherwise, <see
        /// langword="false"/>.</param>
        /// <param name="threshold">The verbosity level threshold for logging. Messages below this threshold will not be logged.</param>
        public void Apply(bool enabled, ELogVerbosity threshold)
        {
            IsEnabled = enabled;

            VerbosityThreshold = threshold;
        }

        /// <summary>
        /// Enables or disables the current instance based on the specified value.
        /// </summary>
        /// <param name="enabled">A value indicating whether the instance should be enabled.  <see langword="true"/> to enable the instance;
        /// otherwise, <see langword="false"/>.</param>
        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        /// <summary>
        /// Sets the verbosity threshold for logging messages.
        /// </summary>
        /// <param name="threshold">The minimum verbosity level of messages to be logged. Messages with a verbosity level lower than this
        /// threshold will be ignored.</param>
        public void SetVerbosityThreshold(ELogVerbosity threshold)
        {
            VerbosityThreshold = threshold;
        }
        #endregion
    }
}
