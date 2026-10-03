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
    public class FConsoleVariable<TVariableType> : IConsoleVariable
    {
        #region Properties
        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public TVariableType Value { get; private set; }

        public EConsoleCommandType CommandType => EConsoleCommandType.Variable;
        #endregion

        #region Events
        public event Action<object> OnValueChanged;
        #endregion

        #region Constrcutors
        public FConsoleVariable(string name, string description, TVariableType defaultValue)
        {
            Name = name;

            Description = description;

            Value = defaultValue;
        }
        #endregion

        #region IConsoleVariable Implementation
        object IConsoleVariable.GetValue()
        {
            return Value;
        }

        void IConsoleVariable.SetValue(object value)
        {
            if (value is not TVariableType typedValue)
            {
                throw new InvalidCastException();
            }

            Value = typedValue;

            OnValueChanged?.Invoke(Value);
        }

        bool IConsoleVariable.TrySetValue(string value)
        {
            try
            {
                Value = (TVariableType)ParseValue(value);

                OnValueChanged?.Invoke(Value);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        private object ParseValue(string value)
        {
            Type conversionType = Nullable.GetUnderlyingType(typeof(TVariableType)) ?? typeof(TVariableType);

            if (conversionType == typeof(string))
            {
                return value;
            }

            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value, true);
            }

            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}