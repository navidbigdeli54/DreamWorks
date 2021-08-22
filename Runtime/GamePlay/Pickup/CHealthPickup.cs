/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using UnityEngine;
using DreamMachineGameStudio.Dreamworks.GamePlay.Health;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Pickup
{
    public class CHealthPickup : CPickupable
    {
        #region Fields
        [SerializeField]
        private int _amount;
        #endregion

        #region Protected Methods
        protected override void OnCollisionEnter(Collision Collision)
        {
            IHealable healable = Collision.gameObject.GetComponent<IHealable>();
            if (healable != null && healable.CanHeal)
            {
                healable.Heal(new FHeal(_amount));
                Destroy(gameObject);
            }
        }
        #endregion
    }
}