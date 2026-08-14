using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;
using DreamMachineGameStudio.DreamWorks.Log;
using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class FDreamWorksStartup
    {
        #region Fields
        private static readonly ILogProvider logProvider = new FScopedLogger(new FLogCategory(nameof(FDreamWorksStartup), ELogVerbosity.Display));
        #endregion

        #region Private Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Startup()
        {
            CreateDreamWorksBootstrapper();
        }

        private static void CreateDreamWorksBootstrapper()
        {
            logProvider.Log($"Creating {nameof(FDreamWorksBootstrapper)}");

            new GameObject(nameof(FDreamWorksBootstrapper)).AddComponent<FDreamWorksBootstrapper>();
        }
        #endregion
    }
}