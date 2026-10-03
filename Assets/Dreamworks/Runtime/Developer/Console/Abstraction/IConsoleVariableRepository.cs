namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleVariableRepository
    {
        #region Methods
        void RegisterVariable(IConsoleVariable variable);

        void RegisterVariable<TVariableType>(string name, TVariableType defaultValue, string description);

        void UnregisterVariable(string variableName);

        bool TryGetVariable(string name, out IConsoleVariable variable);

        bool TryGetVariableValue<TVariableType>(string name, out TVariableType value);
        #endregion
    }
}
