using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core.Assets;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.ResourceProvider;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public static class FSubSystemRegisteryProvider
    {
        #region Fields
        private static readonly IResourceKey SettingResourceKey = new FResourcesKey<UDreamWorksSettings>("DreamWorks/DA_SubSystemRegistery");

        private static readonly ILogProvider logProvider = new FScopedLogProvider(new FLogCategory(nameof(FDreamWorkSettingsProvider), ELogVerbosity.Display, Color.blue));

        private static USubSystemRegistery registery;
        #endregion

        #region Public Methods
        public static void Load()
        {
            logProvider.Log($"Loading {SettingResourceKey}.");

            IResourceProvider resourceProvider = new FResourceProvider(logProvider);

            registery = resourceProvider.LoadResource<USubSystemRegistery>(SettingResourceKey);
            if (registery == null)
            {
                logProvider.LogError($"Can not locate {SettingResourceKey} file in resource folder, using default settings instead.");

                registery = UDataAsset.CreateInstance<USubSystemRegistery>();
            }

            logProvider.Log($"Loaded DreamWorks Settings.");
        }

        public static IReadOnlyList<FSubSystemSettings> GetSubSystemsOf<TSubSystem>() where TSubSystem : class, ISubSystem
        {
            return registery.SubSystemSettings.Where(x=>x.IsEnable && x.SubSystem.Type.GetTypeInfo().IsSubclassOf(typeof(TSubSystem))).ToList();
        }
        #endregion
    }
}