/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FTransactionAction : ITransitionAction
    {
        #region Methods
        protected abstract void Perform();
        #endregion

        #region ITransactionAction Implementation
        void ITransitionAction.Perform() => Perform();
        #endregion
    }
}