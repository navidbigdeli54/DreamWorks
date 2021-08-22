/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0051

using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Persistent;
using DreamMachineGameStudio.Dreamworks.EventManager;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public sealed class MDreamwork : MonoBehaviour
    {
        #region Field
        private readonly List<IInitializableObject> _registeredObjects = new List<IInitializableObject>();

        private readonly List<IInitializableObject> _pendingRegiserationRequests = new List<IInitializableObject>();

        private readonly List<ITickableObject> _registeredTicks = new List<ITickableObject>();

        private readonly List<ITickableObject> _registeredLateTicks = new List<ITickableObject>();

        private readonly List<ITickableObject> _registeredFixedTick = new List<ITickableObject>();

        private readonly HashSet<IInitializableObject> _initializablesHolder = new HashSet<IInitializableObject>();

        private readonly HashSet<ITickableObject> _ticksHolder = new HashSet<ITickableObject>();

        private readonly HashSet<ITickableObject> _lateTicksHolder = new HashSet<ITickableObject>();

        private readonly HashSet<ITickableObject> _fixedTicksHolder = new HashSet<ITickableObject>();
        #endregion

        #region Property
        public Type CLASS_TYPE => typeof(MDreamwork);

        public static MDreamwork Instance { get; private set; }

        public bool HasInitialized { get; set; }

        public bool HasBeganPlay { get; set; }
        #endregion

        #region MonoBehaviour Methods
        private void Awake()
        {
            if (Instance == null)
            {
                name = nameof(MDreamwork);

                CPersistent.MakePersistent(gameObject);

                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _registeredTicks.Count; ++i)
            {
                ITickableObject tickable = _registeredTicks[i];

                if (tickable == null) continue;

                if (HasInitialized == false) continue;

                if (tickable is IInitializableObject initializable && initializable.HasInitialized == false) continue;

                if (tickable.CanEverTick == false) continue;

                if (HasBeganPlay == false && tickable.CanTickBeforePlay == false) continue;

                try
                {
                    tickable.Tick(Time.deltaTime);
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception.Message);
                }
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _registeredLateTicks.Count; ++i)
            {
                ITickableObject tickable = _registeredLateTicks[i];

                if (tickable == null) continue;

                if (this.HasInitialized == false) continue;

                if (tickable is IInitializableObject initializable && initializable.HasInitialized == false) continue;

                if (tickable.CanEverLateTick == false) continue;

                if (HasBeganPlay == false && tickable.CanLateTickBeforePlay == false) continue;

                try
                {
                    tickable.LateTick(Time.deltaTime);
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception.Message);
                }

                InitializePendingObjects();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _registeredFixedTick.Count; ++i)
            {
                ITickableObject tickable = _registeredFixedTick[i];

                if (tickable == null) continue;

                if (this.HasInitialized == false) continue;

                if (tickable is IInitializableObject initializable && initializable.HasInitialized == false) continue;

                if (tickable.CanEverFixedTick == false) continue;

                if (HasBeganPlay == false && tickable.CanFixedTickBeforePlay == false) continue;

                try
                {
                    tickable.FixedTick(Time.fixedDeltaTime);
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception.Message);
                }
            }
        }
        #endregion

        #region Method
        public async void StartUp()
        {
            await InitializeServicesAsync();

            FEventManager.Subscribe(FEventManager.ON_GAME_MODE_LOADED, OnGameModeLoaded);

            FEventManager.Subscribe(FEventManager.ON_SCENE_UNLOADED, OnSceneUnloaded);

            PublishFakeSceneLoadedEvent();
        }

        public void Register(IInitializableObject initializable)
        {
            try
            {
                if (_initializablesHolder.Contains(initializable))
                {
                    FLog.Warning(CLASS_TYPE.Name, $"{initializable.Name} has already registered");

                    return;
                }

                if (HasInitialized == false)
                {
                    if (initializable is IService && (initializable is IGameService) == false)
                    {
                        _registeredObjects.Add(initializable);
                    }
                    else
                    {
                        _pendingRegiserationRequests.Add(initializable);
                    }
                }
                else
                {
                    if (initializable is IService && (initializable is IGameService) == false)
                    {
                        FLog.Error(CLASS_TYPE.Name, $"{initializable.Name} is a service and should not be registered during game play, use GameStartup to register them. Registration aborted.");

                        return;
                    }

                    _pendingRegiserationRequests.Add(initializable);
                }

                _initializablesHolder.Add(initializable);
            }
            catch (Exception exception)
            {
                FLog.Error(nameof(MDreamwork), exception.Message);
            }
        }

        public async Task UnregisterAsync(IInitializableObject initializable)
        {
            if (_initializablesHolder.Contains(initializable) == false)
            {
                FLog.Warning(CLASS_TYPE.Name, $"{initializable.Name} has not registered but wants to unregister itself.");

                return;
            }

            await initializable.UninitializeAsync();

            _registeredObjects.Remove(initializable);

            _initializablesHolder.Remove(initializable);

            if (initializable is ITickableObject tickable)
            {
                if (_ticksHolder.Contains(tickable))
                    UnregisterTick(tickable);

                if (_lateTicksHolder.Contains(tickable))
                    UnregisterLateTick(tickable);

                if (_fixedTicksHolder.Contains(tickable))
                    UnregisterFixedTick(tickable);
            }
        }

        public void RegisterTick(ITickableObject tickable)
        {
            if (_ticksHolder.Contains(tickable))
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} already registered for tick.");

                return;
            }

            _registeredTicks.Add(tickable);

            _ticksHolder.Add(tickable);
        }

        public void UnregisterTick(ITickableObject tickable)
        {
            if (_ticksHolder.Contains(tickable) == false)
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} has not registered for tick but wants to unregister.");

                return;
            }

            _registeredTicks.Remove(tickable);

            _ticksHolder.Remove(tickable);
        }

        public void RegisterLateTick(ITickableObject tickable)
        {
            if (_lateTicksHolder.Contains(tickable))
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} already registered for late tick.");

                return;
            }

            _registeredLateTicks.Add(tickable);

            _lateTicksHolder.Add(tickable);
        }

        public void UnregisterLateTick(ITickableObject tickable)
        {
            if (_lateTicksHolder.Contains(tickable) == false)
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} has not registered for late tick but wants to unregister.");

                return;
            }

            _registeredLateTicks.Remove(tickable);

            _lateTicksHolder.Remove(tickable);
        }

        public void RegisterFixedTick(ITickableObject tickable)
        {
            if (_fixedTicksHolder.Contains(tickable))
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} already registered for fixed tick.");

                return;
            }

            _registeredFixedTick.Add(tickable);

            _fixedTicksHolder.Add(tickable);
        }

        public void UnregisterFixedTick(ITickableObject tickable)
        {
            if (_fixedTicksHolder.Contains(tickable) == false)
            {
                FLog.Warning(CLASS_TYPE.Name, $"{tickable.Name} has not registered for fixed tick but wants to unregister.");

                return;
            }

            _registeredFixedTick.Remove(tickable);

            _fixedTicksHolder.Remove(tickable);
        }
        #endregion

        #region Private Methods
        private async Task InitializeServicesAsync()
        {
            /*
             * In registration process, if framework was not initialized yet, we only register IService instances and put all other instances to pending list.
             * So we safely assume that at this moment, only IService instances are in _registeredObjects list.
             */

            await PreinitializeAsync(_registeredObjects);

            await InitializeAsync(_registeredObjects);

            await BeginPlayAsync(_registeredObjects);
        }

        private async Task PreinitializeAsync(IReadOnlyList<IInitializableObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    IInitializableObject initializable = list[i];

                    if (initializable.HasInitialized) continue;

                    await initializable.PreInitializeAsync();
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception);
                }
            }
        }

        private async Task InitializeAsync(IReadOnlyList<IInitializableObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    IInitializableObject initializable = list[i];

                    if (initializable.HasInitialized) continue;

                    await initializable.InitializeAsync();
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception);
                }
            }
        }

        private async Task BeginPlayAsync(IReadOnlyList<IInitializableObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    IInitializableObject initializable = list[i];

                    if (initializable.HasBeganPlay) continue;

                    await initializable.BeginPlayAsync();
                }
                catch (Exception exception)
                {
                    FLog.Error(CLASS_TYPE.Name, exception);
                }
            }
        }

        private void PublishFakeSceneLoadedEvent()
        {
            FEventManager.Publish(FEventManager.ON_SCENE_LOADED, new SceneManager.FSceneLoadedEventArg(SceneManager.FSceneManager.ActiveScene, UnityEngine.SceneManagement.LoadSceneMode.Single));
        }

        private async void OnGameModeLoaded(object arg)
        {
            HasInitialized = true;

            await InitializePendingObjectsAsync();

            HasBeganPlay = true;
        }

        private async void InitializePendingObjects()
        {
            await InitializePendingObjectsAsync();
        }

        private async Task InitializePendingObjectsAsync()
        {
            if (_pendingRegiserationRequests.Count > 0)
            {
                await PreinitializeAsync(_pendingRegiserationRequests);

                await InitializeAsync(_pendingRegiserationRequests);

                await BeginPlayAsync(_pendingRegiserationRequests);

                _registeredObjects.AddRange(_pendingRegiserationRequests);

                _pendingRegiserationRequests.Clear();
            }
        }

        private void OnSceneUnloaded(object arg)
        {
            HasBeganPlay = false;
        }
        #endregion
    }
}