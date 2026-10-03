using System;
using System.Globalization;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    /// <summary>
    /// Represents a console variable with a specified type, allowing dynamic configuration and runtime updates.
    /// </summary>
    /// <remarks>This class provides functionality to define a console variable with a name, description, and
    /// default value. The value can be retrieved or updated dynamically, and changes to the value trigger the <see
    /// cref="OnValueChanged"/> event. The class implements the <see cref="IConsoleVariable"/> interface, enabling
    /// integration with a console command system.</remarks>
    /// <typeparam name="TVariableType">The type of the value stored by the console variable, such as int, float, bool, and string.</typeparam>
    /// <summary>Represents a typed console variable with source-agnostic value-change notification.</summary>
    /// <typeparam name="TVariableType">The variable's value type.</typeparam>
    public sealed class FConsoleVariable<TVariableType> : IConsoleVariable
    {
        #region Fields
        private TVariableType value;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the registered variable name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the variable description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets whether changes to this variable should be persisted.
        /// </summary>
        public bool IsPersistent { get; }

        /// <summary>
        /// Gets the exact type stored by this variable.
        /// </summary>
        public Type ValueType => typeof(TVariableType);

        /// <summary>
        /// Gets the current typed variable value.
        /// </summary>
        public TVariableType Value => value;

        /// <summary>
        /// Gets the command category for this variable.
        /// </summary>
        public EConsoleCommandType CommandType => EConsoleCommandType.Variable;
        #endregion

        #region Events
        /// <summary>
        /// Occurs only when the current value changes; the event carries no update-source information.
        /// </summary>
        public event Action<object> OnValueChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a console variable initialized to its default value.
        /// </summary>
        public FConsoleVariable(string name, string description, TVariableType defaultValue, bool isPersistent = false)
        {
            Name = name;
            Description = description;
            value = defaultValue;
            IsPersistent = isPersistent;
        }
        #endregion

        #region IConsoleVariable Implementation
        object IConsoleVariable.GetValue() => value;

        void IConsoleVariable.SetValue(object nextValue)
        {
            if (nextValue is not TVariableType typedValue) { throw new InvalidCastException($"Value for '{Name}' must be of type {typeof(TVariableType).Name}."); }
            SetValue(typedValue);
        }

        bool IConsoleVariable.TrySetValue(string serializedValue)
        {
            try
            {
                SetValue((TVariableType)ParseValue(serializedValue));
                return true;
            }
            catch (FormatException) { return false; }
            catch (OverflowException) { return false; }
            catch (ArgumentException) { return false; }
        }
        #endregion

        #region Private Methods
        private void SetValue(TVariableType nextValue)
        {
            if (System.Collections.Generic.EqualityComparer<TVariableType>.Default.Equals(value, nextValue)) { return; }
            value = nextValue;
            OnValueChanged?.Invoke(value);
        }

        private object ParseValue(string serializedValue)
        {
            Type conversionType = Nullable.GetUnderlyingType(typeof(TVariableType)) ?? typeof(TVariableType);
            if (conversionType == typeof(string)) { return serializedValue; }
            if (conversionType == typeof(bool)) { return bool.Parse(serializedValue); }
            if (conversionType.IsEnum) { return Enum.Parse(conversionType, serializedValue, true); }
            return Convert.ChangeType(serializedValue, conversionType, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
