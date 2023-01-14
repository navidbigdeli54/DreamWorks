/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Tween
{
    public static partial class FTween
    {
        #region Fields
        private readonly static Coroutine.ICoroutine _coroutine;

        private readonly static WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        #endregion

        #region Constructors
        static FTween()
        {
            _coroutine = new GameObject("Tweener").AddComponent<Coroutine.CCoroutine>();
        }
        #endregion
    }
}