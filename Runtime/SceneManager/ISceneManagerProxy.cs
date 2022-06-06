/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable IDE0002

using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.SceneManager
{
    public interface ISceneManagerProxy : IService
    {
        #region Properties
        public Scene ActiveScene { get; }
        #endregion

        #region Methods
        public Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);

        public Task UnloadSceneAsycn(string sceneName);
        #endregion
    }
}