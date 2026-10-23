using DreamMachineGameStudio.DreamWorks.Developer.Console;

namespace DreamMachineGameStudio.DreamWorks.SubSystems.Settings
{
    public static class FSubSystemRegistryDefaults
    {
        public static FSubSystemSettings GetConsoleSubSystemSetting()
        {
            return new FSubSystemSettings(typeof(FConsoleBootstrapper));
        }
    }
}