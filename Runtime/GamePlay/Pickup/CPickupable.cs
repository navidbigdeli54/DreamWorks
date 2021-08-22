/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Pickup
{
    [RequireComponent(typeof(Collider))]
    public abstract class CPickupable : CComponent, IPickupable
    {
        #region Protected Methods
        protected abstract void OnCollisionEnter(Collision Collision);
        #endregion
    }
}