using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;

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
            FObjectPoolEntry entry = entries.Get(name);
            if (entry != null)
            {
                return (CPoolableGameObject)entry.Get();
            }

            return null;
        }

        void IGameObjectPool.Retrive(CPoolableGameObject poolableObject)
        {
            FObjectPoolEntry entry = entries.Get(poolableObject.Owner.Name);
            if (entry != null)
            {
                entry.Retrive(poolableObject);
            }
        }
        #endregion
    }
}