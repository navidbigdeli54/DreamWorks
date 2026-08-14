using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Assets;
using DreamMachineGameStudio.DreamWorks.ResourceProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public static class FSubSystemRegisteryProvider
    {
        #region Fields
        private static readonly IResourceKey SettingResourceKey = new FResourcesKey<UDreamWorksSettings>("DreamWorks/DA_SubSystemRegistery");

        private static readonly ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(FDreamWorkSettingsProvider), ELogVerbosity.Display, Color.blue));

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