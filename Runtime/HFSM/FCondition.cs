/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FCondition : ICondition
    {
        #region Protected Methods
        protected abstract bool Evaluate(FHFSM machine);
        #endregion

        #region ICondition Implementation
        bool ICondition.Evaluate(FHFSM machine) => Evaluate(machine);
        #endregion
    }
}