/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System.Collections;

namespace DreamMachineGameStudio.Dreamworks.Coroutine
{
    /// <summary>
    /// CoroutineManagement is responsible for running coroutines for non-monobehaviour classes.
    /// </summary>
    public interface ICoroutine
    {
        #region Methods
        UnityEngine.Coroutine Start(IEnumerator coroutine);

        void Stop(IEnumerator coroutine);

        void Stop(UnityEngine.Coroutine coroutine);

        void StopAll();
        #endregion
    }
}