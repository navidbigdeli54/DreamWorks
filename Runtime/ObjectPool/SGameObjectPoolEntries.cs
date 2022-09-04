using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    [FScriptableObjectWizard("GameObjectPoolEntries")]
    public class SGameObjectPoolEntries : SObjectPoolEntries
    {
        #region Fields
        [SerializeField]
        private List<FGameObjectPoolEntry> entries;
        #endregion

        #region Public Methods
        public override void Initialize(GameObject parent)
        {
            for (int i = 0; i < entries.Count; ++i)
            {
                entries[i].Initialize(parent);
            }
        }

        public override FObjectPoolEntry Get(string name) => entries.SingleOrDefault(x=>x.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        #endregion
    }
}