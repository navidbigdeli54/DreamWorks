/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0002

using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.EventManager;

using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace DreamMachineGameStudio.Dreamworks.SceneManager
{
    public class CSceneManagerProxy : CComponent, ISceneManagerProxy
    {
        #region CComponent Methods
        protected override Task PreInitializeComponenetAsync()
        {
            base.PreInitializeComponenetAsync();

            MakePersistent();

            USceneManager.activeSceneChanged += ActiveSceneChanged;
            USceneManager.sceneLoaded += SceneLoaded;
            USceneManager.sceneUnloaded += SceneUnloaded;

            return Task.CompletedTask;
        }
        #endregion

        #region ISceneManagerProxy Implementation
        Scene ISceneManagerProxy.ActiveScene => USceneManager.GetActiveScene();

        async Task ISceneManagerProxy.LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            AsyncOperation asyncOperation = USceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            asyncOperation.completed += (AsyncOperation ao) => taskCompletionSource.SetResult(ao.isDone);

            await taskCompletionSource.Task;

            FLog.Info(CLASS_TYPE.Name, $"Scene `{USceneManager.GetSceneByName(sceneName).name}` has been loaded.");
        }

        async Task ISceneManagerProxy.UnloadSceneAsycn(string sceneName)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            AsyncOperation asyncOperation = USceneManager.UnloadSceneAsync(sceneName);

            asyncOperation.completed += (AsyncOperation ao) => taskCompletionSource.SetResult(ao.isDone);

            await taskCompletionSource.Task;

            FLog.Info(CLASS_TYPE.Name, $"Scene `{USceneManager.GetSceneByName(sceneName).name}` has been unloaded.");
        }
        #endregion

        #region Private Methods
        private static void ActiveSceneChanged(Scene replacedScene, Scene nextScene)
        {
            FEventManager.Publish(FEventManager.ON_ACTIVE_SCENE_CHANGED, new FActiveSceneChangedEventArg(replacedScene, nextScene));
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            FEventManager.Publish(FEventManager.ON_SCENE_LOADED, new FSceneLoadedEventArg(scene, loadSceneMode));
        }

        private static void SceneUnloaded(Scene scene)
        {
            FEventManager.Publish(FEventManager.ON_SCENE_UNLOADED, new FSceneUnloadedEventArg(scene));
        }
        #endregion
    }
}