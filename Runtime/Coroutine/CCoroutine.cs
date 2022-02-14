/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Coroutine
{
    public class CCoroutine : MonoBehaviour, ICoroutine
    {
        #region MonoBehaviour Methods
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region ICoroutineManagement Implementation
        UnityEngine.Coroutine ICoroutine.Start(System.Collections.IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        void ICoroutine.Stop(System.Collections.IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        void ICoroutine.Stop(UnityEngine.Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        void ICoroutine.StopAll()
        {
            StopAllCoroutines();
        }
        #endregion
    }
}