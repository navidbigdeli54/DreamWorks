using DreamMachineGameStudio.Dreamworks.Core;
using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public abstract class SObjectPoolEntries : SScriptableObject
    {
        #region Public Methods
        public abstract void Initialize(GameObject parent);

        public abstract FObjectPoolEntry Get(string name);
        #endregion
    }
}