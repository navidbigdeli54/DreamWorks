using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.ResourceProvider;
using DreamMachineGameStudio.DreamWorks.Application;

using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.SubSystems.Settings;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.SubSystems.Abstraction;
using DreamMachineGameStudio.DreamWorks.Foundation;

namespace DreamMachineGameStudio.DreamWorks.SubSystems
{
    public static class FSubSystemRegisteryProvider
    {
        #region Fields
        private static readonly IResourceKey SettingResourceKey = new FResourcesPath<USubSystemRegistery>("DreamWorks/DA_SubSystemRegistery");

        private static readonly ILogProvider logProvider = new FScopedLogProvider(new FLogCategory(nameof(FDreamWorksSettingsProvider), ELogVerbosity.Display, Color.blue));

        private static USubSystemRegistery registery;
        #endregion

        #region Public Methods
        public static void Load()
        {
            logProvider.Log($"Loading {SettingResourceKey}.");

            IResourceProvider resourceProvider = new FDefaultResourceProvider(logProvider);

            registery = resourceProvider.LoadResource<USubSystemRegistery>(SettingResourceKey);
            if (registery == null)
            {
                logProvider.LogError($"Can not locate {SettingResourceKey} file in resource folder, using default settings instead.");

                registery = UDataAsset.CreateInstance<USubSystemRegistery>();
            }

            logProvider.Log($"Loaded DreamWorks SubSystem Registery.");
        }

        public static IReadOnlyList<FSubSystemSettings> GetSubSystemsOf<TSubSystem>() where TSubSystem : class, ISubSystem
        {
            return registery.SubSystemSettings.Where(x=>x.IsEnable && x.SubSystem.Type.GetTypeInfo().IsSubclassOf(typeof(TSubSystem))).ToList();
        }
        #endregion
    }
}