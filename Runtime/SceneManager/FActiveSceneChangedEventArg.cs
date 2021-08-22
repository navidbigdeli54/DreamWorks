/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine.SceneManagement;

namespace DreamMachineGameStudio.Dreamworks.SceneManager
{
    public class FActiveSceneChangedEventArg : EventArgs
    {
        #region Property
        public readonly Scene ReplacedScene;

        public readonly Scene NextScene;
        #endregion

        #region Constructor
        public FActiveSceneChangedEventArg(Scene replacedScene, Scene nextScene)
        {
            ReplacedScene = replacedScene;

            NextScene = nextScene;
        }
        #endregion
    }
}
