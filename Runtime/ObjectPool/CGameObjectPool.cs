using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public class CGameObjectPool : CComponent, IGameObjectPool
    {
        #region Fields
        [SerializeField]
        private SObjectPoolEntries entries;
        #endregion

        #region Protected Methods
        protected async override Task PreInitializeComponenetAsync()
        {
            await base.PreInitializeComponenetAsync();

            entries.Initialize(gameObject);
        }
        #endregion

        #region IGameObjectPool Implementation
        CPoolableGameObject IGameObjectPool.Get(string name)
        {
            FAssert.IsFalse(string.IsNullOrEmpty(name), $"name parameter in IGameObjectPool.Get(string) should not be null!");

            FObjectPoolEntry entry = entries.Get(name);
            if(entry == null)
            {
                FLog.Error(nameof(CGameObjectPool), $"Can't find {name} game object pool entry.");

                return null;
            }

            return (CPoolableGameObject)entry.Get();
        }

        void IGameObjectPool.Retrive(CPoolableGameObject poolableObject)
        {
            FAssert.IsNotNull(poolableObject, $"poolableObject parameter in IGameObjectPool.Retrive(CPoolableGameObject) should not be null!");
            FAssert.IsFalse(string.IsNullOrEmpty(poolableObject.Owner.Name), $"The owner of the {poolableObject} should not be null!");

            FObjectPoolEntry entry = entries.Get(poolableObject.Owner.Name);

            FAssert.IsNotNull(entry, $"Can't find the {poolableObject.Owner.Name} entry to retrive the {poolableObject} object to the pool!");

            if (entry != null)
            {
                poolableObject.SetParent(this);

                poolableObject.SetActive(false);

                entry.Retrive(poolableObject);
            }
        }
        #endregion
    }
}