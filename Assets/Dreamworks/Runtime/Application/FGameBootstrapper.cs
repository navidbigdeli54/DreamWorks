using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.SubSystems;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.Application.Abstraction;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.Application
{
    /// <summary>
    /// Provides the core functionality to bootstrap, initialize, tick, and shut down a game instance.
    /// </summary>
    /// <remarks>The <see cref="FGameBootstrapper"/> class is responsible for managing the lifecycle of a game
    /// instance, including loading settings, initializing subsystems, and delegating frame updates and shutdown
    /// operations to the game instance. It implements the <see cref="IDreamWorksObject"/> interface to integrate with
    /// the DreamWorks framework.</remarks>
    internal sealed class FGameBootstrapper : IDreamWorksObject
    {
        #region Fields
        private readonly ILogProvider logProvider;
        #endregion

        #region Properties
        public IGame Game { get; private set; }
        #endregion

        #region Constructors
        public FGameBootstrapper(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }
        #endregion

        #region IDreamWorkObject Implementation
        void IDreamWorksObject.Initialize()
        {
            Initialize();
        }

        void IDreamWorksObject.Tick(FFrameContext frameContext)
        {
            Tick(frameContext);
        }

        void IDreamWorksObject.ShutDown()
        {
            ShutDown();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            logProvider.LogVerbose("Bootstrapping game.");

            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

            LoadDreamWorkSettings();

            LoadSubSystemRegistry();

            CreateAndInitializeGame();

            stopwatch.Stop();

            logProvider.LogDisplay($"Game bootstrapped in {stopwatch.ElapsedMilliseconds} ms.");
        }

        private void LoadDreamWorkSettings()
        {
            logProvider.LogVerbose("Loading DreamWorks settings.");

            FDreamWorksSettingsProvider.Load();

            UDreamWorksSettings settings = FDreamWorksSettingsProvider.Settings;

            logProvider.LogMessage($"Using DreamWorks settings \"{settings.name}\" (GameInstance: {settings.GameInstanceClass?.Type?.Name ?? "<none>"}, GameWorld: {settings.GameWorldClass?.Type?.Name ?? "<none>"}).", settings);
        }

        private void LoadSubSystemRegistry()
        {
            logProvider.LogVerbose("Loading subsystem registry.");

            FSubSystemRegisteryProvider.Load();
        }

        private void CreateAndInitializeGame()
        {
            try
            {
                logProvider.LogVerbose("Creating game.");

                ILogProvider gameLogProvider = new FScopedLogProvider(new FLogCategory(nameof(FGame), ELogVerbosity.Display, Color.blue));

                Game = new FGame(gameLogProvider, FDreamWorksSettingsProvider.Settings);

                logProvider.LogVerbose("Initializing game.");

                ((IDreamWorksObject)Game).Initialize();
            }
            catch (Exception exception)
            {
                logProvider.LogFatal(exception, "Game bootstrapping failed; the game will not tick.");

                throw;
            }
        }

        private void Tick(FFrameContext frameContext)
        {
            // Skip silently: a failed bootstrap is already reported, and logging here would repeat every frame.
            if (Game == null)
            {
                return;
            }

            ((IDreamWorksObject)Game).Tick(frameContext);
        }

        private void ShutDown()
        {
            if (Game == null)
            {
                logProvider.LogWarning("Game shutdown requested, but no game was created.");

                return;
            }

            logProvider.LogVerbose("Shutting down game.");

            ((IDreamWorksObject)Game).ShutDown();

            logProvider.LogMessage("Game shut down.");
        }
        #endregion
    }
}
