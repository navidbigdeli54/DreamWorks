/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.EventManager;
using DreamMachineGameStudio.Dreamworks.SceneManager;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public sealed class CGameManagement : CComponent, IGameManagement
    {
        #region Fields
        private IGameMode _currentGameMode;
        #endregion

        #region Property
        public new static Type CLASS_TYPE => typeof(CGameManagement);
        #endregion

        #region Method
        protected async override Task InitializeComponentAsync()
        {
            await base.InitializeComponentAsync();

            name = CLASS_TYPE.Name;

            MakePersistent();

            FEventManager.Subscribe(FEventManager.ON_SCENE_LOADED, OnSceneLoaded);
            FEventManager.Subscribe(FEventManager.ON_SCENE_UNLOADED, OnSceneUnloaded);
        }

        protected override void TickComponent(float deltaTime)
        {
            base.TickComponent(deltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBeganPlay == false && _currentGameMode.CanTickBeforePlay == false) return;

            _currentGameMode.Tick(deltaTime);
        }

        protected override void LateTickComponent(float deltaTime)
        {
            base.LateTickComponent(deltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBeganPlay == false && _currentGameMode.CanLateTickBeforePlay == false) return;

            _currentGameMode.LateTick(deltaTime);
        }

        protected override void FixedTickComponent(float fixedDeltaTime)
        {
            base.FixedTickComponent(fixedDeltaTime);

            if (_currentGameMode == null) return;

            if (_currentGameMode.HasInitialized == false) return;

            if (_currentGameMode.HasInitialized && _currentGameMode.HasBeganPlay == false && _currentGameMode.CanFixedTickBeforePlay == false) return;

            _currentGameMode.FixedTick(fixedDeltaTime);
        }
        #endregion

        #region Helpers
        private async void OnSceneLoaded(EventArgs arg)
        {
            FSceneLoadedEventArg eventArg = (FSceneLoadedEventArg)arg;

            CLevelConfiguration metadata = FindObjectOfType<CLevelConfiguration>();

            if (metadata == null)
            {
                FLog.Warning(CLASS_TYPE.Name, $"Cannot find scene metadata `S{eventArg.Scene.name}`");
            }
            else if (metadata.GameMode == null)
            {
                FLog.Warning(CLASS_TYPE.Name, $"GameMode is not sat for this level.");
            }
            else
            {
                _currentGameMode = Activator.CreateInstance(metadata.GameMode.Type) as IGameMode;

                await _currentGameMode?.PreInitializeAsync();
                await _currentGameMode?.InitializeAsync();
                await _currentGameMode?.BeginPlayAsync();

                FEventManager.Publish(FEventManager.ON_GAME_MODE_LOADED);
            }
        }

        private async void OnSceneUnloaded(EventArgs arg)
        {
            await _currentGameMode.UninitializeAsync();

            _currentGameMode = null;
        }
        #endregion

        #region IGameManagement Implementation
        IGameMode IGameManagement.CurrentGameMode => _currentGameMode;
        #endregion
    }
}