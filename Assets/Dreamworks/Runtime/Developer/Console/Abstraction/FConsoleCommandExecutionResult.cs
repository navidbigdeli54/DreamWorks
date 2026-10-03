namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction
{
    internal readonly struct FConsoleCommandExecutionResult
    {
        #region Properties
        public bool WasSuccessful { get; }

        public string Message { get; }

        public static FConsoleCommandExecutionResult Empty = new(false, string.Empty);
        #endregion

        #region Constructors
        public FConsoleCommandExecutionResult(bool wasSuccessful, string message)
        {
            WasSuccessful = wasSuccessful;

            Message = message;
        }
        #endregion
    }
}