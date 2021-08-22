/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0051

using System;
using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Persistent;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public abstract class CComponent : MonoBehaviour, IFObject
    {
        #region Field
        private bool _canEverTick = false;

        private bool _canEverLateTick = false;

        private bool _canEverFixedTick = false;
        #endregion

        #region Property
        /// <summary>
        /// The type of CComponent
        /// </summary>
        public static Type CLASS_TYPE { get; } = typeof(CComponent);

        /// <summary>
        /// If true, object has registered to the framework otherwise not.
        /// </summary>
        protected bool HasRegistered { get; private set; } = false;

        /// <summary>
        /// If true, component's PreInitialize and Initialize has been called, otherwise not.
        /// </summary>
        protected bool HasInitialized { get; private set; } = false;

        /// <summary>
        /// If true, component's BeginPlay has been called, otherwise not.
        /// </summary>
        protected bool HasBeganPlay { get; private set; } = false;

        /// <summary>
        /// If true, this component will get Tick after all objects have been initialized.
        /// </summary>
        protected bool CanEverTick { get => _canEverTick; set => SetCanEverTick(value); }

        /// <summary>
        /// If true, this component will get LateTick after all objects have been initialized.
        /// </summary>
        protected bool CanEverLateTick { get => _canEverLateTick; set => SetCanEverLateTick(value); }

        /// <summary>
        /// If true, this component will get FixedTick after all objects have been initialized.
        /// </summary>
        protected bool CanEverFixedTick { get => _canEverFixedTick; set => SetCanEverFixedTick(value); }

        /// <summary>
        /// If true, this component will get Tick before BeginPlay.
        /// </summary>
        protected bool CanTickBeforePlay { get; set; } = false;

        /// <summary>
        /// If true, this component will get LateTick before BeginPlay.
        /// </summary>
        protected bool CanLateTickBeforePlay { get; set; } = false;

        /// <summary>
        /// If true, this component will get FixedTick beforeBeginPlay.
        /// </summary>
        protected bool CanFixedTickBeforePlay { get; set; } = false;
        #endregion

        #region MonoBehaviour Methods
        private void Awake()
        {
            MDreamwork.Instance.Register(this);

            HasRegistered = true;
        }

        private void OnEnable() => OnEnableComponenet();

        private void OnDisable() => OnDisableComponent();

        private async Task OnDestroy() => await MDreamwork.Instance.UnregisterAsync(this);
        #endregion

        #region Method
        /// <summary>
        /// PreInitialize is called when the script instance is being loaded.
        /// PreInitialize is used to initialize any variables or game state before the game starts.
        /// PreInitialize is called only once during the lifetime of the script instance.
        /// PreInitialize is called after all objects are initialized so you can safely speak to other objects or query them using eg. GameObject.FindWithTag.
        /// Each GameObject's PreInitialize is called in a random order between objects.
        /// Because of this, you should use PreInitialize to set up references between scripts, and use Initialize to pass any information back and forth.
        /// If you want to change execution order value, you should set it in here.
        /// PreInitialize is called once, just like the constructor.
        /// </summary>
        protected virtual Task PreInitializeComponenetAsync() => Task.CompletedTask;

        /// <summary>
        /// Initialize is called on the frame when a script is enabled just before any of the Tick methods is called the first time.
        /// Like the PreInitialize function, Initialize is called exactly once in the lifetime of the script.
        /// Each GameObject's Initialize is called based on the ExecutionOrder value.
        /// Initialize can be used as awaitable method. If you use it as sync method you must return Task.CompletedTask, otherwise await for desire method.
        /// </summary>
        protected virtual Task InitializeComponentAsync() => Task.CompletedTask;

        /// <summary>
        /// BeginPlay is called after all objects had been initialized and game is about to be ready.
        /// BeginPlay can be used as awaitable method. If you use it as sync method you must return Task.CompletedTask, otherwise await for desire method.
        /// In BeginPlay all objects had been initialized and you can send data back and forth to them and work with them.
        /// Tick callbacks can be set to invoke before BeginPlay.
        /// </summary>
        protected virtual Task BeginPlayAsync() => Task.CompletedTask;

        protected virtual Task UninitializeCompoonentAsync() => Task.CompletedTask;

        /// <summary>
        /// Tick is called every frame.
        /// Tick is the most commonly used function to implement any kind of game behavior.
        /// </summary>
        /// <param name="deltaTime">The time in seconds it took to complete the last frame.</param>
        protected virtual void TickComponent(float deltaTime) { }

        /// <summary>
        /// LateTick is called every frame.
        /// LateTick is called after all Tick functions have been called.This is useful to order script execution.
        /// For example a follow camera should always be implemented in LateTick because it tracks objects that might have moved inside Tick.
        /// </summary>
        /// <param name="deltaTime">The time in seconds it took to complete the last frame.</param>
        protected virtual void LateTickComponent(float deltaTime) { }

        /// <summary>
        /// This function is called every fixed framerate frame.
        /// FixedTick should be used instead of Update when dealing with Rigidbody.
        /// For example when adding a force to a rigidbody, you have to apply the force every fixed frame inside FixedTick instead of every frame inside Tick.
        /// </summary>
        /// <param name="fixedDeltaTime">The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed.</param>
        protected virtual void FixedTickComponent(float fixedDeltaTime) { }

        /// <summary>
        /// OnEnableComponent will calls when Component become enable.
        /// </summary>
        protected virtual void OnEnableComponenet() { }

        /// <summary>
        /// OnDisable will calls when Component become disable.
        /// </summary>
        protected virtual void OnDisableComponent() { }

        public void SetActive(bool value) => gameObject.SetActive(value);

        public void SetParent(Transform parent) => transform.SetParent(parent);

        public void SetParent(Component parent) => transform.SetParent(parent?.transform);

        public void SetParent(GameObject parent) => transform.SetParent(parent?.transform);

        public Transform Parent => transform.parent;

        public void UnParent() => transform.SetParent(null);

        public void MakePersistent() => CPersistent.MakePersistent(gameObject);

        public void MakeTransient() => CPersistent.MakeTransient(gameObject);

        public TComponent AddComponent<TComponent>() where TComponent : MonoBehaviour => gameObject.AddComponent<TComponent>();
        #endregion

        #region Helpers
        private void SetCanEverTick(bool canEverTick)
        {
            _canEverTick = canEverTick;

            if (_canEverTick) MDreamwork.Instance.RegisterTick(this);
            else MDreamwork.Instance.UnregisterTick(this);
        }

        private void SetCanEverLateTick(bool canEverLateTick)
        {
            _canEverLateTick = canEverLateTick;

            if (_canEverLateTick) MDreamwork.Instance.RegisterLateTick(this);
            else MDreamwork.Instance.UnregisterLateTick(this);
        }

        private void SetCanEverFixedTick(bool canEverFixedTick)
        {
            _canEverFixedTick = canEverFixedTick;

            if (_canEverFixedTick) MDreamwork.Instance.RegisterFixedTick(this);
            else MDreamwork.Instance.UnregisterFixedTick(this);
        }
        #endregion

        #region Static Methods
        public static T Instantiate<T>() where T : CComponent => new GameObject(typeof(T).Name).AddComponent<T>();

        public static Component Instantiate(Type type) => new GameObject(type.Name).AddComponent(type);
        #endregion

        #region IFObject Implementation
        bool IInitializableObject.HasRegistered => HasRegistered;

        bool IInitializableObject.HasInitialized => HasInitialized;

        bool IInitializableObject.HasBeganPlay => HasBeganPlay;

        bool ITickableObject.CanEverTick => CanEverTick;

        bool ITickableObject.CanEverLateTick => CanEverLateTick;

        bool ITickableObject.CanEverFixedTick => CanEverFixedTick;

        bool ITickableObject.CanTickBeforePlay => CanTickBeforePlay;

        bool ITickableObject.CanLateTickBeforePlay => CanLateTickBeforePlay;

        bool ITickableObject.CanFixedTickBeforePlay => CanFixedTickBeforePlay;

        FName INameable.Name => name;

        async Task IInitializable.PreInitializeAsync() => await PreInitializeComponenetAsync();

        async Task IInitializable.InitializeAsync()
        {
            await InitializeComponentAsync();

            HasInitialized = true;
        }

        async Task IInitializable.BeginPlayAsync()
        {
            await BeginPlayAsync();

            HasBeganPlay = true;
        }

        async Task IInitializable.UninitializeAsync() => await UninitializeCompoonentAsync();

        void ITickable.Tick(float deltaTime)
        {
            if (gameObject.activeInHierarchy == false) return;

            if (enabled == false) return;

            if (HasBeganPlay == false && CanTickBeforePlay == false) return;

            TickComponent(deltaTime);
        }

        void ITickable.LateTick(float deltaTime)
        {
            if (gameObject.activeInHierarchy == false) return;

            if (enabled == false) return;

            if (HasBeganPlay == false && CanLateTickBeforePlay == false) return;

            LateTickComponent(deltaTime);
        }

        void ITickable.FixedTick(float fixedDeltaTime)
        {
            if (gameObject.activeInHierarchy == false) return;

            if (enabled == false) return;

            if (HasBeganPlay == false && CanFixedTickBeforePlay == false) return;

            FixedTickComponent(fixedDeltaTime);
        }
        #endregion
    }
}