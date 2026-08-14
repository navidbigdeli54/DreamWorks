namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    public readonly struct FConsoleExecutionResult
    {
        #region Properties
        public bool WasSuccessful { get; }

        public string Message { get; }
        #endregion

        #region Constructors
        public FConsoleExecutionResult(bool wasSuccessful, string message)
        {
            WasSuccessful = wasSuccessful;
            Message = message;
        }
        #endregion
    }
}