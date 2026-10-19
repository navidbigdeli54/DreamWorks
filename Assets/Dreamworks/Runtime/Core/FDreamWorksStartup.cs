using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using DreamMachineGameStudio.DreamWorks.Developer.Console;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    public class FDreamWorksStartup
    {
        #region Fields
        private static FDreamWorksBootstrapper dreamWorksBootstrapper;
        #endregion

        #region Private Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Startup()
        {
            Application.quitting -= OnApplicationQuite;
            Application.quitting += OnApplicationQuite;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif

            RemoveDreamWorksTickFromPlayerLoop();
            CreateDeveloperConsoleBootstrapper();
            CreateDreamWorksBootstrapper();
            RegisterDreamWorksTickInPlayerLoop();
        }

        private static void CreateDeveloperConsoleBootstrapper()
        {
            FConsoleBootstrapper console = new FConsoleBootstrapper();
            console.Initialize();
        }

        private static void OnApplicationQuite()
        {
            Application.quitting -= OnApplicationQuite;
            ShutDown();
        }

        private static void CreateDreamWorksBootstrapper()
        {
            dreamWorksBootstrapper = new GameObject(nameof(FDreamWorksBootstrapper)).AddComponent<FDreamWorksBootstrapper>();
            dreamWorksBootstrapper.Initialize();
        }

        private static void RegisterDreamWorksTickInPlayerLoop()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            playerLoop = RemoveTickSystem(playerLoop);

            PlayerLoopSystem tickSystem = new PlayerLoopSystem
            {
                type = typeof(FDreamWorksStartup),
                updateDelegate = TickDreamWorks
            };

            if (!InsertTickSystem(ref playerLoop, tickSystem))
            {
                Debug.LogError("Could not register DreamWorks tick: Unity PlayerLoop Update phase was not found.");
                return;
            }

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static void RemoveDreamWorksTickFromPlayerLoop()
        {
            PlayerLoop.SetPlayerLoop(RemoveTickSystem(PlayerLoop.GetCurrentPlayerLoop()));
        }

        private static PlayerLoopSystem RemoveTickSystem(PlayerLoopSystem system)
        {
            PlayerLoopSystem[] children = system.subSystemList;
            if (children == null || children.Length == 0)
            {
                return system;
            }

            List<PlayerLoopSystem> retainedChildren = new List<PlayerLoopSystem>(children.Length);
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].type == typeof(FDreamWorksStartup))
                {
                    continue;
                }

                retainedChildren.Add(RemoveTickSystem(children[i]));
            }

            system.subSystemList = retainedChildren.ToArray();
            return system;
        }

        private static bool InsertTickSystem(ref PlayerLoopSystem system, PlayerLoopSystem tickSystem)
        {
            if (system.type == typeof(Update))
            {
                PlayerLoopSystem[] existingChildren = system.subSystemList ?? Array.Empty<PlayerLoopSystem>();
                List<PlayerLoopSystem> children = new List<PlayerLoopSystem>(existingChildren.Length + 1);
                children.AddRange(existingChildren);
                children.Add(tickSystem);
                system.subSystemList = children.ToArray();
                return true;
            }

            PlayerLoopSystem[] systems = system.subSystemList;
            if (systems == null)
            {
                return false;
            }

            for (int i = 0; i < systems.Length; i++)
            {
                if (InsertTickSystem(ref systems[i], tickSystem))
                {
                    system.subSystemList = systems;
                    return true;
                }
            }

            return false;
        }

        private static void TickDreamWorks()
        {
            if (dreamWorksBootstrapper != null)
            {
                dreamWorksBootstrapper.Tick();
            }
        }

        private static void ShutDown()
        {
            RemoveDreamWorksTickFromPlayerLoop();

            if (dreamWorksBootstrapper != null)
            {
                dreamWorksBootstrapper.ShutDown();
            }

            if (FConsoleBootstrapper.Instance != null)
            {
                FConsoleBootstrapper.Instance.ShutDown();
            }
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
        #endregion
    }
}
