using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider
{
    /// <summary>
    /// Provides a scoped logging provider for gameplay-related log messages.
    /// </summary>
    /// <remarks>This class is a singleton, with the <see cref="Instance"/> property providing the global
    /// instance. It is designed to log messages categorized under "Gameplay" with a default verbosity level of <see
    /// cref="ELogVerbosity.Display"/>.</remarks>
    public class FGameplayLogProvider : FScopedLogProvider
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