using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleVariable : IConsoleObject
    {

        #region Events
        public event Action<object> OnValueChanged;
        #endregion

        #region Methods
        object GetValue();

        void SetValue(object value);

        bool TrySetValue(string value); 
        #endregion
    }
}
