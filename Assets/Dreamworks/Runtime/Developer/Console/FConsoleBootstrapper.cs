using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Application;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.Developer.Console.UI;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Core;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Output;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History;
using DreamMachineGameStudio.DreamWorks.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Definitions;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Persistence;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;

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
    internal sealed class FConsoleBootstrapper : IDreamWorksObject
    {
        #region Fields
        private readonly ILogProvider logProvider;

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
        public FConsoleBootstrapper(ILogProvider logProvider)
        {
            Instance = this;

            this.logProvider = logProvider;

            methodRepository = new FConsoleMethodRepository(logProvider);

            IConsoleVariablePersistence variablePersistence = new FConsoleVariablePersistence(logProvider, "variables.bin");

            IConsoleVariableDefinitionProvider definitionProvider = new FConsoleVariableDefinitionResourcesProvider(logProvider);

            variableRepository = new FConsoleVariableRepository(logProvider, definitionProvider, variablePersistence);

            developerConsole = new FDeveloperConsole(methodRepository, variableRepository);

            commandActivator = new FConsoleCommandActivator(logProvider, methodRepository, variableRepository);

            commandQuery = new FConsoleCommandQuery(methodRepository as IConsoleCommandQuery, variableRepository as IConsoleCommandQuery);

            commandHistory = new FConsoleCommandHistory(logProvider, commandActivator, 255, "commands.bin");

            commandOutputBuffer = new FConsoleCommandOutputBuffer(commandActivator);

            widgetBootstrapper = new FConsoleWidgetBootstrapper(logProvider, commandQuery, commandOutputBuffer, commandHistory);

            logProvider.LogVerbose("Developer console services created.");
        }
        #endregion

        #region IDreamWorksObject Implementation
        void IDreamWorksObject.Initialize()
        {
            logProvider.LogVerbose("Initializing developer console.");

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

            logProvider.LogDisplay("Developer console initialized.");
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            widgetBootstrapper.Tick(frameContext.DeltaTime);
        }

        void IDreamWorksObject.ShutDown()
        {
            logProvider.LogVerbose("Shutting down developer console.");

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

            logProvider.LogMessage("Developer console shut down.");
        }
        #endregion

        #region Private Methods
        private void OnCommandEntered(string command)
        {
            logProvider.LogVeryVerbose($"Command submitted from console widget: \"{command}\".");

            commandActivator.ExecuteCommand(command);
        }
        #endregion
    }
}
