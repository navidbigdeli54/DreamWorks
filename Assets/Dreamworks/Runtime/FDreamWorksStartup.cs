using System;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.LowLevel;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Application;
using DreamMachineGameStudio.DreamWorks.LogProvider;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.SceneManagement;
using DreamMachineGameStudio.DreamWorks.Developer.Console;
using DreamMachineGameStudio.DreamWorks.LogProvider.Abstraction;
using DreamMachineGameStudio.DreamWorks.LogProvider.Bootstrapper;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;

namespace DreamMachineGameStudio.DreamWorks
{
    /// <summary>
    /// Provides the startup and initialization logic for the DreamWorks framework.
    /// </summary>
    /// <remarks>This class is responsible for setting up the necessary components and systems required for
    /// the DreamWorks framework to function. It initializes log providers, console systems, and game-specific 
    /// bootstrappers, and integrates the DreamWorks tick system into Unity's player loop.   The startup process is
    /// triggered automatically before the first scene is loaded, ensuring that all  required systems are ready before
    /// gameplay begins. Additionally, it handles application quitting  and play mode state changes in the Unity Editor
    /// to ensure proper shutdown and cleanup.</remarks>
    public class FDreamWorksStartup
    {
        #region Fields
        private static FLogProviderBootstrapper logProviderBootstrapper;

        private static FConsoleBootstrapper consoleBootstrapper;

        private static FGameBootstrapper gameBoostrapper;
        #endregion

        #region Private Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Startup()
        {
            Initialize();
        }

        private static void Initialize()
        {
            RegisterOnApplicationQuitting();

            OverrideSceneManagerAPI();

            CreateLogProviderBootstrapper();

            CreateConsoleBootstrapper();

            CreateGameBootstrapper();

            RegisterDreamWorksTickInPlayerLoop();
        }

        private static void RegisterOnApplicationQuitting()
        {
            UnityEngine.Application.quitting -= OnApplicationQuite;
            UnityEngine.Application.quitting += OnApplicationQuite;

            RegisterPlayModeStateChanged_Editor();
        }

        private static void OnApplicationQuite()
        {
UnityEngine.Application.quitting -= OnApplicationQuite;

            ShutDown();
        }

        [Conditional("UNITY_EDITOR")]
        private static void RegisterPlayModeStateChanged_Editor()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
        }

#if UNITY_EDITOR
        private static void OnPlayModeChanged(UnityEditor.PlayModeStateChange change)
        {
            if (change != UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                return;
            }

            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;

            ShutDown();
        }
#endif

        private static void OverrideSceneManagerAPI()
        {
            SceneManagerAPI.overrideAPI = new DreamWorkSceneManagerAPI();
        }

        private static void CreateLogProviderBootstrapper()
        {
            logProviderBootstrapper = new FLogProviderBootstrapper();
            ((IDreamWorksObject)logProviderBootstrapper).Initialize();
        }

        private static void CreateConsoleBootstrapper()
        {
            var logProvider = new FScopedLogProvider(new FLogCategory("DeveloperConsole", ELogVerbosity.Log, Color.white));

            consoleBootstrapper = new FConsoleBootstrapper(logProvider);

            ((IDreamWorksObject)consoleBootstrapper).Initialize();
        }

        private static void CreateGameBootstrapper()
        {
            var logProvider = new FScopedLogProvider(new FLogCategory("GameBootstrapper", ELogVerbosity.Log, Color.white));

            gameBoostrapper = new FGameBootstrapper(logProvider);

            ((IDreamWorksObject)gameBoostrapper).Initialize();
        }

        private static void RegisterDreamWorksTickInPlayerLoop()
        {
            var dremWorksPlayerLoop = new PlayerLoopSystem
            {
                type = typeof(FDreamWorksStartup),

                updateDelegate = Tick
            };

            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var newSubSystens = new List<PlayerLoopSystem>(currentPlayerLoop.subSystemList)
            {
                dremWorksPlayerLoop
            };

            currentPlayerLoop.subSystemList = newSubSystens.ToArray();

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }

        private static void Tick()
        {
            FFrameContext frameContext = new(Time.deltaTime, Time.frameCount);

            ((IDreamWorksObject)logProviderBootstrapper).Tick(frameContext);
            ((IDreamWorksObject)consoleBootstrapper).Tick(frameContext);
            ((IDreamWorksObject)gameBoostrapper).Tick(frameContext);
        }

        private static void ShutDown()
        {
            RemoveDreamWorksTickSystem();

            ((IDreamWorksObject)gameBoostrapper).ShutDown();
            ((IDreamWorksObject)consoleBootstrapper).ShutDown();
            ((IDreamWorksObject)logProviderBootstrapper).ShutDown();
        }

        private static void RemoveDreamWorksTickSystem()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var retainedSubSystems = new List<PlayerLoopSystem>(currentPlayerLoop.subSystemList);
            foreach (var subSystem in currentPlayerLoop.subSystemList)
            {
                if(subSystem.type == typeof(FDreamWorksStartup))
                {
                    continue;
                }

                retainedSubSystems.Add(subSystem);
            }

            currentPlayerLoop.subSystemList = retainedSubSystems.ToArray();

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }
        #endregion
    }
}
