/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public class FHeal : IHeal
    {
        #region Fields
        private int _amount;
        #endregion

        #region Constructors
        public FHeal(int amount)
        {
            _amount = amount;
        }
        #endregion

        #region IHeal Implementation
        int IHeal.Amount => _amount;
        #endregion
    }
}