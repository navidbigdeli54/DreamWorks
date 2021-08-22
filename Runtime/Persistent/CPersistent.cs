/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Persistent
{
    [DisallowMultipleComponent]
    public class CPersistent : MonoBehaviour
    {
        #region Fields
        public readonly static Type CLASS_TYPE = typeof(CPersistent);
        #endregion

        #region MonoBehaviours
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Method
        protected void OnDestroyComponent()
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Persist a game object that means it will not destroy between levels.
        /// </summary>
        /// <param name="gameObject">Desire game object to make it persistent.</param>
        public static void MakePersistent(GameObject gameObject)
        {
            if (gameObject == null)
            {
                FLog.Error(CLASS_TYPE.Name, $"Given game object to making it persistent is null. it is not accepted.");
                return;
            }

            if (gameObject.GetComponent<CPersistent>() != null)
            {
                FLog.Warning(CLASS_TYPE.Name, $"`{gameObject.name}` has marked as persistent already.");
                return;
            }

            gameObject.AddComponent<CPersistent>();
        }

        /// <summary>
        /// Make a game object transient that means it will destroy when level has been unloaded.
        /// </summary>
        /// <param name="gameObject">Desire game object to make it transient</param>
        public static void MakeTransient(GameObject gameObject)
        {
            if (gameObject == null)
            {
                FLog.Error(CLASS_TYPE.Name, $"Given game object to making it persistent is null. it is not accepted.");
                return;
            }

            CPersistent component = gameObject.GetComponent<CPersistent>();

            if (component == null)
            {
                FLog.Warning(CLASS_TYPE.Name, $"`{gameObject.name}` has marked as transient already.");
                return;
            }

            Destroy(component);
        }
        #endregion
    }
}