﻿/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.SceneManager
{
    public interface ISceneManagerProxy : IService
    {
        #region Properties
        public Scene ActiveScene { get; }

        public Action<Scene, Scene> OnActiveSceneChanged { get; set; }

        public Action<Scene, LoadSceneMode> OnSceneLoaded { get; set; }

        public Action<Scene> OnSceneAboutToUnload { get; set; }

        public Action<Scene> OnSceneUnloaded { get; set; }
        #endregion

        #region Methods
        public Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);

        public Task UnloadSceneAsycn(string sceneName);
        #endregion
    }
}