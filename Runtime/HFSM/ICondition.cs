/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface ICondition
    {
        #region Methods
        bool Evaluate(FHFSM machine);
        #endregion
    }
}