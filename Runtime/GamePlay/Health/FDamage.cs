/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public class FDamage : IDamage
    {
        #region Fields
        [SerializeField]
        private int _amount;
        #endregion

        #region Constructors
        public FDamage() { }

        public FDamage(int amount) => _amount = amount;
        #endregion

        #region IDamage Implementation
        int IDamage.Amount => _amount;
        #endregion
    }
}