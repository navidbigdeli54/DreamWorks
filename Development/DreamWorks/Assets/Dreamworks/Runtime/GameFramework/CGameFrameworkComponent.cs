using System;
using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Log;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.GameFramework
{

    public class CGameFrameworkComponent : MonoBehaviour, IGameFrameworkComponent
    {
        #region Properties
        [field: SerializeField]
        public FTickSetting TickSetting { get; private set; } = new FTickSetting();

        public FTickState TickState { get; private set; } = new FTickState();

        public EGameFrameworkLifecycleState LifecycleState { get; private set; } = EGameFrameworkLifecycleState.None;

        public IGameWorld GameWorld { get; private set; }

        public ILogProvider LogProvider { get; private set; } = FGameplayLogProvider.Instance;
        #endregion

        #region IGameFrameworkComponent Implementation
        GameObject IGameFrameworkComponent.GameObject => gameObject;

        void IGameFrameworkComponent.SetGameWorld(IGameWorld gameWorld)
        {
            if (GameWorld != null)
            {
                throw new InvalidOperationException($"{name}:{this.GetType()} already blong to a world!");
            }

            GameWorld = gameWorld;
        }
        #endregion

        #region IInitializable Implementation
        async Task IGameFrameworkInitializable.PreInitializeAsync()
        {
            await PreInitializeAsync();

            LifecycleState = EGameFrameworkLifecycleState.PreInitialized;
        }

        async Task IGameFrameworkInitializable.InitializeAsync()
        {
            await InitializeAsync();

            LifecycleState = EGameFrameworkLifecycleState.Initialized;
        }

        async Task IGameFrameworkInitializable.PostInitializeAsync()
        {
            await PostInitializeAsync();

            LifecycleState = EGameFrameworkLifecycleState.PostInitialized;
        }

        async Task IGameFrameworkInitializable.BeginPlayAsync()
        {
            await BeginPlayAsync();

            LifecycleState = EGameFrameworkLifecycleState.BegunPlay;
        }

        async Task IGameFrameworkInitializable.EndPlayAsync()
        {
            await EndPlayAsync();

            LifecycleState = EGameFrameworkLifecycleState.EndedPlay;
        }

        async Task IGameFrameworkInitializable.UninitializeAsync()
        {
            await UninitializeAsync();

            LifecycleState = EGameFrameworkLifecycleState.UnInitialized;
        }
        #endregion

        #region ITickable Implementation
        void IGameFrameworkTickable.Tick(float deltaTime)
        {
            Tick(deltaTime);
        }
        #endregion

        #region Protected Methods
        protected virtual Task PreInitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostInitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task BeginPlayAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void Tick(float deltaTime)
        {

        }

        protected virtual Task EndPlayAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}