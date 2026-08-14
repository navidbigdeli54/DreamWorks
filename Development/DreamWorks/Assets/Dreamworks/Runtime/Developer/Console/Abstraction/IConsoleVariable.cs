using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleVariable : IConsoleObject
    {
        Type ValueType { get; }

        object GetValue();

        void SetValue(object value);

        bool TrySetValue(string value);
    }
}
