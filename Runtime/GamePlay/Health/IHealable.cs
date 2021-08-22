/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public interface IHealable
    {
        bool CanHeal { get; }

        #region Properties
        Action<IHeal> OnHeal { get; set; }
        #endregion

        #region Methods
        void Heal(IHeal heal);
        #endregion
    }
}