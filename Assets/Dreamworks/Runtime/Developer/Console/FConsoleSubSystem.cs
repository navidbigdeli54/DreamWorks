using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.UI;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Core;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Output;
using DreamMachineGameStudio.DreamWorks.Developer.Console.History;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Developer.Console.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console
{
    [ADreamWorksSubSystem(
        displayName: "Console",
        description: "Provides in-game developer console with cvars, commands, and runtime debugging tools.",
        category: "Developer",
        order: 0,
        Experimental = false,
        Advanced = false,
        Keywords = "console cvar command debug runtime shell")]
    public sealed class FConsoleSubSystem : FGameInstanceSubSystem
    {
        #region Fields
        private readonly IConsoleMethodRepository methodRepository;

        private readonly IConsoleVariableRepository variableRepository;

        private readonly IConsoleCommandActivator commandActivator;

        private readonly IConsoleCommandQuery commandQuery;

        private readonly IConsoleCommandHistory commandHistory;

        private readonly IConsoleCommandOutputBuffer commandOutputBuffer;

        private readonly FConsoleWidgetBootstrapper widgetBootstrapper;
        #endregion

        #region Properties
        public IDeveloperConsole DeveloperConsole { get; }

        public override bool CanTick => true;
        #endregion

        #region Constructors
        public FConsoleSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
            var scopedLogger = new FScopedLogger(new FLogCategory(nameof(FConsoleSubSystem), ELogVerbosity.Verbose));

            methodRepository = new FConsoleMethodRepository(scopedLogger);

            variableRepository = new FConsoleVariableRepository(scopedLogger);

            DeveloperConsole = new FDeveloperConsole(methodRepository, variableRepository);

            commandActivator = new FConsoleCommandActivator(methodRepository, variableRepository);

            commandQuery = new FConsoleCommandQuery(methodRepository as IConsoleCommandQuery, variableRepository as IConsoleCommandQuery);

            commandHistory = new FConsoleCommandHistory(scopedLogger, commandActivator);

            commandOutputBuffer = new FConsoleCommandOutputBuffer(scopedLogger, commandActivator);

            widgetBootstrapper = new FConsoleWidgetBootstrapper(commandQuery, commandOutputBuffer, commandHistory);
        }
        #endregion

        #region Protected Methods
        protected override Task InitializeAsync()
        {
            if (DeveloperConsole is IDeveloperConsoleInitializer developerConsoleInitializer)
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

            return Task.CompletedTask;
        }

        protected override void Tick(float deltaTime)
        {
            widgetBootstrapper.Tick(deltaTime);
        }

        protected override Task ShutDownAsync()
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

            if (DeveloperConsole is IDeveloperConsoleInitializer developerConsoleInitializer)
            {
                developerConsoleInitializer.ShutDown();
            }


            return Task.CompletedTask;
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