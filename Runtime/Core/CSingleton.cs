/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public abstract class CSingleton<T> where T : MonoBehaviour
    {
        #region Fields
        private static T _instance;
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

                return _instance;
            }
        }
        #endregion
    }
}