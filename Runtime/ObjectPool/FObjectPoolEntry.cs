using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    [Serializable]
    public abstract class FObjectPoolEntry
    {
        #region Fields
        [SerializeField]
        private string _name;

        [SerializeField]
        private int _count;

        private List<IPoolableObject> _instances = new List<IPoolableObject>();
        #endregion

        #region Properties
        public string Name => _name;

        protected GameObject Parent { get; private set; }
        #endregion

        #region Public Methods
        public void Initialize(GameObject parent)
        {
            Parent = parent;

            for (int i = 0; i < _count; ++i)
            {
                IPoolableObject poolableObject = Instantiate();

                poolableObject.Owner = new FPoolableInstanceOwner(_name);

                _instances.Add(poolableObject);
            }
        }

        public IPoolableObject Get()
        {
            IPoolableObject poolableGameObject = _instances.FirstOrDefault(x => x.IsFree);

            if (poolableGameObject == null)
            {
                poolableGameObject = Instantiate();

                poolableGameObject.Owner = new FPoolableInstanceOwner(_name);

                _instances.Add(poolableGameObject);
            }

            poolableGameObject.Acquired();

            return poolableGameObject;
        }

        public void Retrive(IPoolableObject poolableGameObject)
        {
            poolableGameObject.Released();
        }
        #endregion

        #region Protected Methods
        protected abstract IPoolableObject Instantiate();
        #endregion
    }
}