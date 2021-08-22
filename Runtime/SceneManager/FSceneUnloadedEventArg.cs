/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine.SceneManagement;

namespace DreamMachineGameStudio.Dreamworks.SceneManager
{
    public class FSceneUnloadedEventArg : EventArgs
    {
        #region Property
        public Scene Scene { get; }
        #endregion

        #region Constructor
        public FSceneUnloadedEventArg(Scene scene) => Scene = scene;
        #endregion
    }
}
