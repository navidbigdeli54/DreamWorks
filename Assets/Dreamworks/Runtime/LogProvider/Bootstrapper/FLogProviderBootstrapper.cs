using DreamMachineGameStudio.DreamWorks.Application;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.LogProvider.Persistence;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction.Persistence;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.LogProvider.Bootstrapper
{
    /// <summary>
    /// Provides initialization, update, and shutdown functionality for the logging provider system.
    /// </summary>
    /// <remarks>This class is responsible for managing the lifecycle of the logging provider system,
    /// including initializing the log category registry with persistent storage, handling periodic updates, and
    /// performing cleanup during shutdown. It implements the <see cref="IDreamWorksObject"/> interface to integrate
    /// with the broader application lifecycle.</remarks>
    internal sealed class FLogProviderBootstrapper : IDreamWorksObject
    {
        #region Fields
        private ILogCategoryPersistence persistence;
        #endregion

        #region IDreamWorksObject Implementation
        void IDreamWorksObject.Initialize()
        {
            persistence = new FLogCategoryPersistence("categories.bin");

            FLogCategoryRegistry.Initialize(persistence);
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {

        }

        void IDreamWorksObject.ShutDown()
        {
            FLogCategoryRegistry.Shutdown();

            persistence = null;
        }
        #endregion
    }
}
