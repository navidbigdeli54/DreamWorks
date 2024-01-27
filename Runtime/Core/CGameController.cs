/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.SceneManager;
using DreamMachineGameStudio.Dreamworks.ServiceLocator;
using DreamMachineGameStudio.Dreamworks.Console;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public sealed class CGameController : CComponent, IGameController
    {
        #region Fields
        private IGameMode _currentGameMode;
        #endregion

        #region Method
        protected async override Task InitializeComponentAsync()
        {
            await base.InitializeComponentAsync();

            MakePersistent();

            FServiceLocator.Get<ISceneManagerProxy>().OnSceneLoaded += OnSceneLoaded;

            FServiceLocator.Get<ISceneManagerProxy>().OnSceneAboutToUnload += OnSceneAboutToUnloaded;

            FServiceLocator.Get<ISceneManagerProxy>().OnSceneUnloaded += OnSceneUnloaded;
        }

        protected override void TickComponent(float deltaTime)
        {
            base.TickComponent(deltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBegunPlay == false && _currentGameMode.CanTickBeforePlay == false) return;

            _currentGameMode.Tick(deltaTime);
        }

        protected override void LateTickComponent(float deltaTime)
        {
            base.LateTickComponent(deltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBegunPlay == false && _currentGameMode.CanLateTickBeforePlay == false) return;

            _currentGameMode.LateTick(deltaTime);
        }

        protected override void FixedTickComponent(float fixedDeltaTime)
        {
            base.FixedTickComponent(fixedDeltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBegunPlay == false && _currentGameMode.CanFixedTickBeforePlay == false) return;

            _currentGameMode.FixedTick(fixedDeltaTime);
        }
        #endregion

        #region Private Methods
        private async void OnSceneLoaded(Scene scene, LoadSceneMode _)
        {
            CLevelConfiguration levelConfiguration = FindObjectOfType<CLevelConfiguration>();

            if (levelConfiguration == null)
            {
                FLog.Warning(nameof(CGameController), $"Cannot find scene configuration `S{scene.name}`");

                return;
            }

            if (levelConfiguration.GameMode == null)
            {
                FLog.Warning(nameof(CGameController), $"GameMode is not set for this level.");

                return;
            }

            _currentGameMode = Activator.CreateInstance(levelConfiguration.GameMode.Type) as IGameMode;

            FConsoleUtility.TryAddCommands(_currentGameMode);

            await _currentGameMode?.PreInitializeAsync();

            await _currentGameMode?.InitializeAsync();

            await _currentGameMode?.BeginPlayAsync();

            ((IGameController)this).OnGameModeBegunPlay?.Invoke();
        }

        private async void OnSceneAboutToUnloaded(Scene _)
        {
            await _currentGameMode.EndPlayAsync();

			FConsoleUtility.TryRemoveCommands(_currentGameMode);

			((IGameController)this).OnGameModeEnded?.Invoke();
        }

        private async void OnSceneUnloaded(Scene scene)
        {
            await _currentGameMode.UninitializeAsync();

            _currentGameMode = null;
        }
        #endregion

        #region IGameManagement Implementation
        IGameMode IGameController.CurrentGameMode => _currentGameMode;

        Action IGameController.OnGameModeBegunPlay { get; set; }

        Action IGameController.OnGameModeEnded { get; set; }
        #endregion
    }
}