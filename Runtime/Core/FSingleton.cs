/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public abstract class FSingleton<T> where T : class, new()
    {
        #region Fields
        private static readonly T _instance;
        #endregion

        #region Constructors
        static FSingleton()
        {
            _instance = new T();
        }
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion
    }
}