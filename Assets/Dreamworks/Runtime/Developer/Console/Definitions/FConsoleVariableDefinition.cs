using System;
using UnityEngine;
using System.Globalization;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions
{
    /// <summary>
    /// Stores the authoring data for a user-defined console variable.
    /// </summary>
    [Serializable]
    public sealed class FConsoleVariableDefinition
    {
        #region Properties
        /// <summary>
        /// Gets the unique console-variable name.
        /// </summary>
        [field: SerializeField]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the user-facing description.
        /// </summary>
        [field: SerializeField]
        public string Description { get; private set; }

        /// <summary>
        /// Gets the value type represented by this definition.
        /// </summary>
        [field: SerializeField]
        public EConsoleVariableType VariableType { get; private set; }

        /// <summary>
        /// Gets whether changes to this variable are persisted between runs.
        /// </summary>
        [field: SerializeField]
        public bool IsPersistent { get; private set; }

        /// <summary>
        /// Gets the default value in invariant serialized string form.
        /// </summary>
        [field: SerializeField]
        public string DefaultValue { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a definition initialized with safe values for editor authoring.
        /// </summary>
        public FConsoleVariableDefinition()
        {
            Name = string.Empty;
            Description = string.Empty;
            VariableType = EConsoleVariableType.Boolean;
            DefaultValue = "false";
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates the identifier and converts the serialized default to its declared runtime type.
        /// </summary>
        public bool TryGetDefaultValue(out object value)
        {
            value = null;
            if (!IsValidName(Name) || DefaultValue == null) { return false; }
            switch (VariableType)
            {
                case EConsoleVariableType.Boolean:
                    if (bool.TryParse(DefaultValue, out bool booleanValue)) { value = booleanValue; return true; }
                    return false;
                case EConsoleVariableType.String:
                    value = DefaultValue;
                    return true;
                case EConsoleVariableType.Float:
                    if (float.TryParse(DefaultValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue) && !float.IsNaN(floatValue) && !float.IsInfinity(floatValue)) { value = floatValue; return true; }
                    return false;
                case EConsoleVariableType.Integer:
                    if (int.TryParse(DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int integerValue)) { value = integerValue; return true; }
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks whether a name is a valid console identifier.
        /// </summary>
        public static bool IsValidName(string value)
        {
            if (string.IsNullOrEmpty(value) || !(char.IsLetter(value[0]) || value[0] == '_'))
            {
                return false;
            }

            for (int index = 1; index < value.Length; index++)
            {
                if (!(char.IsLetterOrDigit(value[index]) || value[index] == '_'))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
