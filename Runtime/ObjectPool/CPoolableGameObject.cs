using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public class CPoolableGameObject : CComponent, IPoolableObject
    {
        #region Properties
        public bool IsFree { get; private set; } = true;

        public FPoolableInstanceOwner Owner => ((IPoolableObject)this).Owner;
        #endregion

        #region Protected Methods
        protected override async Task BeginPlayAsync()
        {
            await base.BeginPlayAsync();

            if(Parent == null)
            {
                MakePersistent();
            }
        }
        #endregion

        #region IPoolableObject Implementation
        FPoolableInstanceOwner IPoolableObject.Owner { get; set; }

        void IPoolableObject.Acquired() => IsFree = false;

        void IPoolableObject.Released() => IsFree = true;
        #endregion
    }
}