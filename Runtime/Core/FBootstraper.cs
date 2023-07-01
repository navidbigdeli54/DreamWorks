/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    internal static class FBootstraper
    {
        #region Method
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartUp()
        {
            SDreamworksConfiguration configuration = Resources.Load<SDreamworksConfiguration>(nameof(SDreamworksConfiguration));

            if (configuration == null)
            {
                FLog.Error(nameof(FBootstraper), "Dreamwork configuration dose not exist at the `Assets/Resources/SDreamworksConfiguration` path.");

                return;
            }

            if (configuration.DontLoadFramework)
            {
                FLog.Info(nameof(FBootstraper), "Starting the Dreamwork has beem cancelled because DontLoadFramework is true.");

                return;
            }

            Type startupType = configuration.StartupClass;

            if (startupType == null)
            {
                FLog.Error(nameof(FBootstraper), "Startup class has not assigned to the configuration, Please assign one in framework's configuration.");

                return;
            }

            if (startupType.IsSubclassOf(typeof(FStartup)) == false)
            {
                FLog.Error(nameof(FBootstraper), "Startup class should be a subclass of `SStartup`.");

                return;
            }


            IStartup startup = (IStartup)Activator.CreateInstance(startupType);
            startup.Run();
        }
        #endregion
    }
}