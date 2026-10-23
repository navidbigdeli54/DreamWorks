using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.ResourceProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Application
{
    /// <summary>
    /// Provides functionality to load and access the DreamWorks settings.
    /// </summary>
    /// <remarks>This class is responsible for loading the <see cref="UDreamWorksSettings"/> resource from a
    /// predefined location. If the resource cannot be found, default settings are created and used instead. The
    /// settings can be accessed through the <see cref="Settings"/> property after calling the <see cref="Load"/>
    /// method.</remarks>
    public static class FDreamWorksSettingsProvider
    {
        #region Fields
        private static readonly IResourceKey settingResourceKey = new FResourcesPath<UDreamWorksSettings>("DreamWorks/DA_DreamWorksSettings");

        private static readonly ILogProvider logProvider = new FScopedLogProvider(new FLogCategory(nameof(FDreamWorksSettingsProvider), ELogVerbosity.Display, Color.blue));
        #endregion

        #region Properties
        public static UDreamWorksSettings Settings { get; private set; }
        #endregion

        #region Public Methods
        public static void Load()
        {
            logProvider.Log($"Loading {settingResourceKey}.");

            IResourceProvider resourceProvider = new FDefaultResourceProvider(logProvider);

            Settings = resourceProvider.LoadResource<UDreamWorksSettings>(settingResourceKey);
            if (Settings == null)
            {
                logProvider.LogError($"Can not locate {settingResourceKey} file in resource folder, using default settings instead.");

                Settings = UDataAsset.CreateInstance<UDreamWorksSettings>();
            }

            logProvider.Log($"Loaded DreamWorks Settings.");
        }
        #endregion
    }
}