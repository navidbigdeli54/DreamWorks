/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FTransactionAction : ITransitionAction
    {
        #region Methods
        protected abstract void Perform(FHFSM machine);
        #endregion

        #region ITransactionAction Implementation
        void ITransitionAction.Perform(FHFSM machine) => Perform(machine);
        #endregion
    }
}