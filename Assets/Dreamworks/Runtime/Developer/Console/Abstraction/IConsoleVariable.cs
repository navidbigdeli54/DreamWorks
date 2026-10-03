using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    /// <summary>
    /// Represents a typed, runtime-editable console variable.
    /// </summary>
    public interface IConsoleVariable : IConsoleCommand
    {
        #region Properties
        /// <summary>
        /// Gets whether value changes for this variable are saved across application runs.
        /// </summary>
        bool IsPersistent { get; }

        /// <summary>
        /// Gets the exact runtime type of this variable's value.
        /// </summary>
        Type ValueType { get; }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the value changes, without identifying the source of the change.
        /// </summary>
        event Action<object> OnValueChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current value.
        /// </summary>
        object GetValue();

        /// <summary>
        /// Sets a value of the declared runtime type.
        /// </summary>
        void SetValue(object value);

        /// <summary>
        /// Parses and applies a value string using invariant culture for numeric types.
        /// </summary>
        bool TrySetValue(string value);
        #endregion
    }
}
