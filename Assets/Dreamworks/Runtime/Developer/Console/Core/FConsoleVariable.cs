using System;
using System.Globalization;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Core
{
    public class FConsoleVariable<T> : IConsoleVariable
    {
        #region Properties
        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public T Value { get; private set; }

        public EConsoleObjectType ObjectType => EConsoleObjectType.Variable;
        #endregion

        #region Events
        public event Action<object> OnValueChanged;
        #endregion

        #region Constrcutors
        public FConsoleVariable(string name, string description, T defaultValue)
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
            if (value is not T typedValue)
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
                Value = (T)ParseValue(value);

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
            Type conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

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