using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Definitions
{
    internal sealed class FLogCategoryRuntimeSettings
    {
        #region Properties
        public string Name { get; private set; }

        public bool IsEnabled { get; private set; }

        public ELogVerbosity DefaultVerbositry { get; private set; }
        #endregion

        #region Constructors
        public FLogCategoryRuntimeSettings(string name, bool isEnable, ELogVerbosity defaultVerbosity)
        {
            Name = name;
            IsEnabled = isEnable;
            DefaultVerbositry = defaultVerbosity;
        }
        #endregion
    }
}
