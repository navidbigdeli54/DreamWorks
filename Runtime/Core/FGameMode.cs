/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public abstract class FGameMode : IGameMode
    {
        #region Property
        protected FName Name { get; set; }

        protected bool HasInitialized { get; private set; }

        protected bool HasBegunPlay { get; private set; }

        protected bool HasEndedPlay { get; private set; }

        protected bool CanEverTick { get; set; }

        protected bool CanEverLateTick { get; set; }

        protected bool CanEverFixedTick { get; set; }

        protected bool CanTickBeforePlay { get; set; }

        protected bool CanLateTickBeforePlay { get; set; }

        protected bool CanFixedTickBeforePlay { get; set; }
        #endregion

        #region Method
        protected virtual Task PreInitializeAsync() => Task.CompletedTask;

        protected virtual Task InitializeAsync() => Task.CompletedTask;

        protected virtual Task BeginPlayAsync() => Task.CompletedTask;

        protected virtual Task EndPlayAsync() => Task.CompletedTask;

        protected virtual Task UninitializeAsync() => Task.CompletedTask;

        protected virtual void Tick(float deltaTime) { }

        protected virtual void LateTick(float deltaTime) { }

        protected virtual void FixedTick(float fixedDeltaTime) { }

        #endregion

        #region IGameMode Implementation
        FName INameable.Name { get => Name; }

        bool IInitializableObject.HasRegistered => false;

        bool IInitializableObject.HasInitialized => HasInitialized;

        bool IInitializableObject.HasBegunPlay => HasBegunPlay;

        bool IInitializableObject.HasEndedPlay => HasEndedPlay;

        bool IInitializableObject.IsTransient => true;

        bool ITickableObject.CanEverTick => CanEverTick;

        bool ITickableObject.CanEverLateTick => CanEverLateTick;

        bool ITickableObject.CanEverFixedTick => CanEverFixedTick;

        bool ITickableObject.CanTickBeforePlay => CanTickBeforePlay;

        bool ITickableObject.CanLateTickBeforePlay => CanLateTickBeforePlay;

        bool ITickableObject.CanFixedTickBeforePlay => CanFixedTickBeforePlay;

        async Task IInitializable.PreInitializeAsync() => await PreInitializeAsync();

        async Task IInitializable.InitializeAsync()
        {
            await InitializeAsync();

            HasInitialized = true;
        }

        async Task IInitializable.BeginPlayAsync()
        {
            await BeginPlayAsync();

            HasBegunPlay = true;
        }

        async Task IInitializable.EndPlayAsync()
        {
            await EndPlayAsync();

            HasEndedPlay = true;
        }

        async Task IInitializable.UninitializeAsync() => await UninitializeAsync();

        void ITickable.Tick(float deltaTime) => Tick(deltaTime);

        void ITickable.LateTick(float deltaTime) => LateTick(deltaTime);

        void ITickable.FixedTick(float fixedDeltaTime) => FixedTick(fixedDeltaTime);
        #endregion
    }
}