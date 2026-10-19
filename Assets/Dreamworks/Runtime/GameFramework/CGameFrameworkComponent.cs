using UnityEngine;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
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

        public IGameInstance GameInstance { get; private set; }

        public ILogProvider LogProvider { get; private set; } = FGameplayLogProvider.Instance;
        #endregion

        #region IGameFrameworkComponent Implementation
        GameObject IGameFrameworkComponent.GameObject => gameObject;

        void IGameFrameworkComponent.SetGameWorld(IGameWorld gameWorld)
        {
            if (GameWorld == gameWorld)
            {
                return;
            }

            if (GameWorld != null)
            {
                LogProvider.LogError($"{name}:{GetType().Name} already blong to a world!");

                return;
            }

            GameWorld = gameWorld;

            GameInstance = gameWorld.GameInstance;
        }
        #endregion

        #region IInitializable Implementation
        void IGameFrameworkInitializable.PreInitialize()
        {
            PreInitialize();

            LifecycleState = EGameFrameworkLifecycleState.PreInitialized;
        }

        void IGameFrameworkInitializable.Initialize()
        {
            Initialize();

            LifecycleState = EGameFrameworkLifecycleState.Initialized;
        }

        void IGameFrameworkInitializable.PostInitialize()
        {
            PostInitialize();

            LifecycleState = EGameFrameworkLifecycleState.PostInitialized;
        }

        void IGameFrameworkInitializable.BeginPlay()
        {
            BeginPlay();

            LifecycleState = EGameFrameworkLifecycleState.BegunPlay;
        }

        void IGameFrameworkInitializable.EndPlay()
        {
            EndPlay();

            LifecycleState = EGameFrameworkLifecycleState.EndedPlay;
        }

        void IGameFrameworkInitializable.Uninitialize()
        {
            Uninitialize();

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
        protected virtual void PreInitialize()
        {
            
        }

        protected virtual void Initialize()
        {
            
        }

        protected virtual void PostInitialize()
        {

        }

        protected virtual void BeginPlay()
        {

        }

        protected virtual void Tick(float deltaTime)
        {

        }

        protected virtual void EndPlay()
        {

        }

        protected virtual void Uninitialize()
        {
            
        }

        protected virtual TGameMode GetGameMode<TGameMode>() where TGameMode : class, IGameMode
        {
            return GameWorld.GameMode as TGameMode;
        }
        #endregion
    }
}