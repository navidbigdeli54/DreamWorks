using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    internal class DreamWorkSceneManagerAPI : SceneManagerAPI
    {
        public static event Action<string, LoadSceneMode> OnSceneAboutToLoad;

        public static event Action<Scene, LoadSceneMode> OnSceneLoaded;

        public static event Action<Scene> OnSceneAboutToUnload;

        public static event Action<Scene> OnSceneUnloaded;

        protected override AsyncOperation LoadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
        {
            if (parameters.loadSceneMode == LoadSceneMode.Single)
            {
                Scene scene = SceneManager.GetActiveScene();

                OnSceneAboutToUnload?.Invoke(scene);
            }

            OnSceneAboutToLoad?.Invoke(sceneName, parameters.loadSceneMode);

            AsyncOperation sceneLoadOperation = base.LoadSceneAsyncByNameOrIndex(sceneName, sceneBuildIndex, parameters, mustCompleteNextFrame);

            void OnSceneLoadCompleted(AsyncOperation operation)
            {
                operation.completed-= OnSceneLoadCompleted;

                Scene scene = SceneManager.GetSceneByName(sceneName);
                LoadSceneMode loadMode = parameters.loadSceneMode;

                OnSceneLoaded?.Invoke(scene, loadMode);
            }

            sceneLoadOperation.completed += OnSceneLoadCompleted;

            return sceneLoadOperation;
        }

        protected override AsyncOperation UnloadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            OnSceneAboutToUnload?.Invoke(scene);

            AsyncOperation sceneUnloadOperation = base.UnloadSceneAsyncByNameOrIndex(sceneName, sceneBuildIndex, immediately, options, out outSuccess);

            void OnSceneUnloadCompleted(AsyncOperation operation)
            {
                operation.completed -= OnSceneUnloadCompleted;

                OnSceneUnloaded?.Invoke(scene);
            }

            sceneUnloadOperation.completed += OnSceneUnloadCompleted;

            return sceneUnloadOperation;
        }
    }
}