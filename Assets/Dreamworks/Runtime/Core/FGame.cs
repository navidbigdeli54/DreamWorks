using Unity.Scripting.LifecycleManagement;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public sealed partial class FGame : IGame, IDreamWorksObject
    {
        #region Fields
        private readonly ILogProvider logProvider;

        private readonly UDreamWorksSettings settings;
        #endregion

        #region Properties
        [AutoStaticsCleanup]
        public static FGame Instance { get; private set; }

        public IGameInstance GameInstance { get; private set; }
        #endregion

        #region Constructor
        public FGame(UDreamWorksSettings settings, ILogProvider logProvider)
        {
            Instance = this;

            this.settings = settings;

            this.logProvider = logProvider;

            CreateGameInstance();
        }
        #endregion

        #region IDreamWorksObject Implementation
        void IDreamWorksObject.Initialize()
        {
            logProvider.Log("Initializing.");

            InitializeGameInstance();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            TickGameInstnace(frameContext);
        }

        void IDreamWorksObject.ShutDown()
        {
            logProvider.Log("Shutting Down.");

            ShutDownGameInstance();
        }
        #endregion

        #region Private Methods
        private void CreateGameInstance()
        {
            logProvider.Log("Creating GameInstnace.");

            TSubclassOf<IGameInstance> gameInstanceClass = settings.GameInstanceClass;

            object[] constructorArguments = {
                new FScopedLogProvider(new FLogCategory($"{gameInstanceClass.Type.Name}", ELogVerbosity.Display)),
                settings.GameWorldClass,
                settings.GameModeSettings
            };

            GameInstance = gameInstanceClass.Construct(constructorArguments);

            if (GameInstance == null)
            {
                logProvider.LogError($"Could not create determined GameInstance: {settings.GameInstanceClass.Type.Name}!");
            }
        }

        private void InitializeGameInstance()
        {
            ((IDreamWorksObject)GameInstance).Initialize();
        }

        private void TickGameInstnace(FFrameContext context)
        {
            if (GameInstance == null)
            {
                return;
            }

            ((IDreamWorksObject)GameInstance).Tick(context);
        }

        private void ShutDownGameInstance()
        {
            if (GameInstance == null)
            {
                return;
            }

            ((IDreamWorksObject)GameInstance).ShutDown();
        }
        #endregion
    }
}