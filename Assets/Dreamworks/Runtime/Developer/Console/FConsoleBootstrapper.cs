using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.UI;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Core;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Output;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    [ADreamWorksSubSystem(
        displayName: "Console",
        description: "Provides in-game developer console with vars, commands, and runtime debugging tools.",
        category: "Developer",
        order: 0,
        Experimental = false,
        Advanced = false,
        Keywords = "console cvar command debug runtime shell")]
    public sealed class FConsoleBootstrapper
    {
        #region Fields
        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;

        private readonly IDeveloperConsole developerConsole;

        private readonly IConsoleCommandActivator commandActivator;

        private readonly IConsoleCommandQuery commandQuery;

        private readonly IConsoleCommandHistory commandHistory;

        private readonly IConsoleCommandOutputBuffer commandOutputBuffer;

        private readonly FConsoleWidgetBootstrapper widgetBootstrapper;
        #endregion

        #region Properties
        internal static FConsoleBootstrapper Instance { get; private set; }
        #endregion

        #region Constructors
        public FConsoleBootstrapper()
        {
            Instance = this;

            methodRepository = new FConsoleMethodRepository();

            IConsoleVariablePersistence variablePersistence = new FConsoleVariablePersistence("variables.bin");

            IConsoleVariableDefinitionProvider definitionProvider = new FConsoleVariableDefinitionResourcesProvider();

            variableRepository = new FConsoleVariableRepository(definitionProvider, variablePersistence);

            developerConsole = new FDeveloperConsole(methodRepository, variableRepository);

            commandActivator = new FConsoleCommandActivator(methodRepository, variableRepository);

            commandQuery = new FConsoleCommandQuery(methodRepository as IConsoleCommandQuery, variableRepository as IConsoleCommandQuery);

            commandHistory = new FConsoleCommandHistory(commandActivator, 255, "commands.bin");

            commandOutputBuffer = new FConsoleCommandOutputBuffer(commandActivator);

            widgetBootstrapper = new FConsoleWidgetBootstrapper(commandQuery, commandOutputBuffer, commandHistory);
        }
        #endregion

        #region IDreamWorksObject Implementation
        internal void Initialize()
        {
            if (developerConsole is IDeveloperConsoleInitializer developerConsoleInitializer)
            {
                developerConsoleInitializer.Initialize();
            }

            if (commandOutputBuffer is IDeveloperConsoleInitializer commandOutputBufferInitializer)
            {
                commandOutputBufferInitializer.Initialize();
            }

            if (commandHistory is IDeveloperConsoleInitializer commandHistoryInitializer)
            {
                commandHistoryInitializer.Initialize();
            }

            if (widgetBootstrapper is IDeveloperConsoleInitializer widgetBootstrapperInitializer)
            {
                widgetBootstrapperInitializer.Initialize();
            }

            widgetBootstrapper.ConsoleWidget.OnCommandEntered += OnCommandEntered;
        }

        internal void Tick(FFrameContext frameContext)
        {
            widgetBootstrapper.Tick(frameContext.DeltaTime);
        }

        internal void ShutDown()
        {
            widgetBootstrapper.ConsoleWidget.OnCommandEntered -= OnCommandEntered;

            if (widgetBootstrapper is IDeveloperConsoleInitializer widgetBootstrapperInitializer)
            {
                widgetBootstrapperInitializer.ShutDown();
            }

            if (commandOutputBuffer is IDeveloperConsoleInitializer commandOutputBufferInitializer)
            {
                commandOutputBufferInitializer.ShutDown();
            }

            if (commandHistory is IDeveloperConsoleInitializer commandHistoryInitializer)
            {
                commandHistoryInitializer.ShutDown();
            }

            if (developerConsole is IDeveloperConsoleInitializer developerConsoleInitializer)
            {
                developerConsoleInitializer.ShutDown();
            }
        }
        #endregion

        #region Private Methods
        private void OnCommandEntered(string command)
        {
            commandActivator.ExecuteCommand(command);
        }
        #endregion
    }
}
