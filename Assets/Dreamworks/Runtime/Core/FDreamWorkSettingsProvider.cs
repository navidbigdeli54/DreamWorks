using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Assets;
using DreamMachineGameStudio.DreamWorks.ResourceProvider;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public static class FDreamWorkSettingsProvider
    {
        #region Fields
        private static readonly IResourceKey SettingResourceKey = new FResourcesKey<UDreamWorksSettings>("DreamWorks/DA_DreamWorksSettings");

        private static readonly ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(FDreamWorkSettingsProvider), ELogVerbosity.Display, Color.blue));
        #endregion

        #region Properties
        public static UDreamWorksSettings Settings { get; private set; }
        #endregion

        #region Public Methods
        public static void Load()
        {
            logProvider.Log($"Loading {SettingResourceKey}.");

            IResourceProvider resourceProvider = new FResourceProvider(logProvider);

            Settings = resourceProvider.LoadResource<UDreamWorksSettings>(SettingResourceKey);
            if (Settings == null)
            {
                logProvider.LogError($"Can not locate {SettingResourceKey} file in resource folder, using default settings instead.");

                Settings = UDataAsset.CreateInstance<UDreamWorksSettings>();
            }

            logProvider.Log($"Loaded DreamWorks Settings.");
        }
        #endregion
    }
}