using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Definitions;

namespace DreamMachineGameStudio.DreamWorks.LogProvider
{
    /// <summary>
    /// Represents a logging category used to group and filter log messages by name, verbosity, and color.
    /// </summary>
    /// <remarks>A log category defines a unique name, a default verbosity level, and an associated color for
    /// visual distinction. It is used to organize log messages and control their visibility based on verbosity
    /// settings.</remarks>
    public sealed class FLogCategory
    {
        #region Properties
        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the color represented as a 32-bit color value.
        /// </summary>
        public Color32 Color { get; }

        /// <summary>
        /// Gets the default verbosity level for logging operations.
        /// </summary>
        public ELogVerbosity DefaultVerbosity { get; }

        /// <summary>
        /// Gets the runtime settings associated with the logging category.
        /// </summary>
        internal FLogCategoryRuntimeSettings RuntimeSettings { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FLogCategory"/> class with the specified name, default
        /// verbosity, and a default color of white.
        /// </summary>
        /// <param name="name">The name of the log category. This value cannot be null or empty.</param>
        /// <param name="defaultVerbosity">The default verbosity level for the log category.</param>
        public FLogCategory(string name, ELogVerbosity defaultVerbosity)
            : this(name, defaultVerbosity, new Color32(255, 255, 255, 255))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FLogCategory"/> class with the specified name, default
        /// verbosity, and color.
        /// </summary>
        /// <remarks>This constructor registers the log category with the <see
        /// cref="FLogCategoryRegistry"/> and initializes its runtime settings.</remarks>
        /// <param name="name">The name of the log category. This value must be a non-empty string.</param>
        /// <param name="defaultVerbosity">The default verbosity level for the log category. Must be a valid value of <see cref="ELogVerbosity"/> and
        /// cannot be <see cref="ELogVerbosity.NoLogging"/>.</param>
        /// <param name="color">The color associated with the log category, used for visual representation.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null, empty, or consists only of whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="defaultVerbosity"/> is not a valid <see cref="ELogVerbosity"/> value or is set to
        /// <see cref="ELogVerbosity.NoLogging"/>.</exception>
        public FLogCategory(string name, ELogVerbosity defaultVerbosity, Color32 color)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A log category must have a non-empty name.", nameof(name));
            }

            if (defaultVerbosity == ELogVerbosity.NoLogging || !Enum.IsDefined(typeof(ELogVerbosity), defaultVerbosity))
            {
                throw new ArgumentOutOfRangeException(nameof(defaultVerbosity), "Use a concrete verbosity as the category default.");
            }

            Name = name;
            Color = color;
            DefaultVerbosity = defaultVerbosity;
            RuntimeSettings = FLogCategoryRegistry.RegisterCategory(this);
        }
        #endregion
    }
}
