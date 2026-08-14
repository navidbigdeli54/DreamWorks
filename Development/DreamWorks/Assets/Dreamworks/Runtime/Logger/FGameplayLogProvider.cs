namespace DreamMachineGameStudio.DreamWorks.Log
{
    public class FGameplayLogProvider : FScopedLogger
    {
        #region Properties
        public static FGameplayLogProvider Instance { get; } = new FGameplayLogProvider(new FLogCategory("Gameplay", ELogVerbosity.Display));
        #endregion

        #region Constructors
        public FGameplayLogProvider(FLogCategory category) : base(category)
        {
        }
        #endregion
    }
}