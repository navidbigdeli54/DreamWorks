/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public interface IDamageable
    {
        #region Properties
        Action OnDeath { get; set; }

        Action<IDamage> OnTakeDamage { get; set; }
        #endregion

        #region Methods
        void TakeDamage(IDamage damage);
        #endregion
    }
}