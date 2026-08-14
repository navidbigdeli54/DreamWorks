using DreamMachineGameStudio.DreamWorks.Developer.Console;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public static class FSubSystemRegistryDefaults
    {
        public static FSubSystemSettings GetConsoleSubSystemSetting()
        {
            return new FSubSystemSettings(typeof(FConsoleSubSystem));
        }
    }
}