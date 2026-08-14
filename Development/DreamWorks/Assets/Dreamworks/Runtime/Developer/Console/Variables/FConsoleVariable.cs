using System;
using System.Globalization;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Variables
{
    public abstract class FConsoleVariable
    {
        #region Events
        public event Action<object> ValueChanged;
        #endregion

        #region Protected Methods
        protected void RaiseValueChanged(object value)
        {
            ValueChanged?.Invoke(value);
        }
        #endregion
    }

    public class FConsoleVariable<T> : FConsoleVariable, IConsoleVariable
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }

        public T Value { get; private set; }

        public Type ValueType => typeof(T);

        public EConsoleObjectType ObjectType => EConsoleObjectType.Variable;
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

            RaiseValueChanged(Value);
        }

        bool IConsoleVariable.TrySetValue(string value)
        {
            try
            {
                Value = (T)Parse(value);

                RaiseValueChanged(Value);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Public Methods
        public T GetValue()
        {
            return Value;
        }
        #endregion

        #region Private Methods
        private object Parse(string value)
        { 
            if (ValueType == typeof(string))
                return value;

            if (ValueType == typeof(bool))
                return bool.Parse(value);

            if (ValueType == typeof(int))
                return int.Parse(value, CultureInfo.InvariantCulture);

            if (ValueType == typeof(float))
                return float.Parse(value, CultureInfo.InvariantCulture);

            if (ValueType == typeof(double))
                return double.Parse(value, CultureInfo.InvariantCulture);

            if (ValueType.IsEnum)
                return Enum.Parse(ValueType, value, true);

            return Convert.ChangeType(value, ValueType, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}