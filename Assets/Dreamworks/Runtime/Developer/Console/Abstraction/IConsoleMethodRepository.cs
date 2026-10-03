namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    public interface IConsoleMethodRepository
    {
        #region Methods
        void RegisterMethod(IConsoleMethod method);

        void UnregisterMethod(string method);

        bool TryGetMethod(string name, out IConsoleMethod method);
        #endregion
    }
}
