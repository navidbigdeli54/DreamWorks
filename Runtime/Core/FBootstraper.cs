/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    internal static class FBootstraper
    {
        #region Property
        private static Type CLASS_TYPE => typeof(FBootstraper);
        #endregion

        #region Method
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartUp()
        {
            SDreamworksConfiguration configuration = Resources.Load<SDreamworksConfiguration>(nameof(SDreamworksConfiguration));

            if (configuration == null)
            {
                FLog.Error(CLASS_TYPE.Name, "Can't read framework configuration.");

                return;
            }

            if (configuration.DontLoadFramework)
            {
                FLog.Info(CLASS_TYPE.Name, "DontLoadFramework is true.");

                return;
            }

            Type startupType = configuration.StartupClass;

            if (startupType == null)
            {
                FLog.Error(CLASS_TYPE.Name, "Startup type has not been selected, Please select a startup type in framework's configuration.");

                return;
            }

            if (startupType.IsSubclassOf(typeof(FStartup)) == false)
            {
                FLog.Error(CLASS_TYPE.Name, "Startup class should be a subclass of `SStartup`.");

                return;
            }


            IStartup startup = (IStartup)Activator.CreateInstance(startupType);
            startup.Run();
        }
        #endregion
    }
}