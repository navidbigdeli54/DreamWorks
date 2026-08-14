using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Developer.Console.UI;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;

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
        #region Properties

        public FConsoleHistory History { get; }

        public FConsoleOutputBuffer OutputBuffer { get; }

        public FConsoleManager ConsoleManager { get; }

        public UConsoleWidgetBootstrapper WidgetBootstrapper { get; }
        #endregion

        #region Constructors
        public FConsoleSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
            History = new FConsoleHistory(FDefaultLogger.Instance);

            OutputBuffer = new FConsoleOutputBuffer();

            ConsoleManager = new FConsoleManager(FDefaultLogger.Instance);

            WidgetBootstrapper = new GameObject(nameof(UConsoleWidgetBootstrapper)).AddComponent<UConsoleWidgetBootstrapper>();
        }
        #endregion

        #region Protected Methods
        protected override Task InitializeAsync()
        {
            History.Load();

            ConsoleManager.Initialize();
            ConsoleManager.OnCommandEntered += HandleCommandEntered;
            ConsoleManager.OnCommandExecuted += HandleCommandExecuted;

            WidgetBootstrapper.Initialize(this);

            return Task.CompletedTask;
        }

        protected override Task ShutDownAsync()
        {
            History.Save();

            ConsoleManager.OnCommandEntered -= HandleCommandEntered;
            ConsoleManager.OnCommandExecuted -= HandleCommandExecuted;
            ConsoleManager.ShutDown();

            WidgetBootstrapper.ShutDown();

            return Task.CompletedTask;
        }
        #endregion

        #region Private Methods
        private void HandleCommandEntered(string command)
        {
            History.Add(command);
        }

        private void HandleCommandExecuted(FConsoleExecutionResult result)
        {
            OutputBuffer.Add(result.Message, result.WasSuccessful ? EConsoleOutputType.Log : EConsoleOutputType.Error);
        }
        #endregion
    }
}