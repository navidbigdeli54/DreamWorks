/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/


using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

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

        Action<Scene, Scene> ISceneManagerProxy.OnActiveSceneChanged { get; set; }

        Action<Scene, LoadSceneMode> ISceneManagerProxy.OnSceneLoaded { get; set; }

        Action<Scene> ISceneManagerProxy.OnSceneAboutToUnload { get; set; }

        Action<Scene> ISceneManagerProxy.OnSceneUnloaded { get; set; }

        async Task ISceneManagerProxy.LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            ((ISceneManagerProxy)this).OnSceneAboutToUnload?.Invoke(USceneManager.GetActiveScene());

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            AsyncOperation asyncOperation = USceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            asyncOperation.completed += (AsyncOperation ao) => taskCompletionSource.SetResult(ao.isDone);

            await taskCompletionSource.Task;

            FLog.Info(nameof(CSceneManagerProxy), $"Scene `{USceneManager.GetSceneByName(sceneName).name}` has been loaded.");
        }

        async Task ISceneManagerProxy.UnloadSceneAsycn(string sceneName)
        {
            ((ISceneManagerProxy)this).OnSceneAboutToUnload?.Invoke(USceneManager.GetSceneByName(sceneName));

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            AsyncOperation asyncOperation = USceneManager.UnloadSceneAsync(sceneName);

            asyncOperation.completed += (AsyncOperation ao) => taskCompletionSource.SetResult(ao.isDone);

            await taskCompletionSource.Task;

            FLog.Info(nameof(CSceneManagerProxy), $"Scene `{USceneManager.GetSceneByName(sceneName).name}` has been unloaded.");
        }
        #endregion

        #region Private Methods
        private void ActiveSceneChanged(Scene replacedScene, Scene nextScene)
        {
            ((ISceneManagerProxy)this).OnActiveSceneChanged?.Invoke(replacedScene, nextScene);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            ((ISceneManagerProxy)this).OnSceneLoaded?.Invoke(scene, loadSceneMode);
        }

        private void SceneUnloaded(Scene scene)
        {
            ((ISceneManagerProxy)this).OnSceneUnloaded?.Invoke(scene);
        }
        #endregion
    }
}