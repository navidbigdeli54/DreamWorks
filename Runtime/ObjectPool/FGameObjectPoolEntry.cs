using DreamMachineGameStudio.Dreamworks.Debug;
using System;
using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    [Serializable]
    public class FGameObjectPoolEntry : FObjectPoolEntry
    {
        #region Fields
        [SerializeField]
        private GameObject _asset;
        #endregion

        #region Protected Methods
        protected override IPoolableObject Instantiate()
        {
            GameObject instance = UnityEngine.Object.Instantiate(_asset, Parent.transform);

            instance.gameObject.SetActive(false);

            if(instance.TryGetComponent(out CPoolableGameObject poolableGameObject) == false)
            {
                FLog.Warning(nameof(CGameObjectPool), $"{Name} prefab does not have poolableInstance component, adding it manually");

                poolableGameObject = instance.AddComponent<CPoolableGameObject>();
            }

            return poolableGameObject;
        }
        #endregion
    }
}