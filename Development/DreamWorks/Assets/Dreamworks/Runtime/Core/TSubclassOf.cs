using System;
using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Log;

namespace DreamMachineGameStudio.DreamWorks.Core
{
    [Serializable]
    public class TSubclassOf<T> where T : class
    {
        #region Fields
        [SerializeField]
        private string assemblyQualifiedTypeName;
        #endregion

        #region Properties
        public Type Type
        {
            get
            {
                if (string.IsNullOrEmpty(assemblyQualifiedTypeName))
                    return null;

                return Type.GetType(assemblyQualifiedTypeName);
            }
            set
            {
                assemblyQualifiedTypeName = value?.AssemblyQualifiedName;
            }
        }
        #endregion

        #region Public Methods
        public static implicit operator Type(TSubclassOf<T> subclass)
        {
            return subclass?.Type;
        }

        public static implicit operator TSubclassOf<T>(Type type)
        {
            return new TSubclassOf<T>() { assemblyQualifiedTypeName = type.AssemblyQualifiedName };
        }

        public override string ToString()
        {
            return Type.Name;
        }
        #endregion

        #region Public Methods
        public T Construct(object[] args)
        {
            try
            {
                return Activator.CreateInstance(Type, args) as T;
            }
            catch (Exception exception)
            {
                FDefaultLogger.Instance.Log(exception.ToString());

                return null;
            }
        }
        #endregion
    }
}