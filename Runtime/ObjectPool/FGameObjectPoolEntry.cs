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

            return instance.AddComponent<CPoolableGameObject>();
        }
        #endregion
    }
}