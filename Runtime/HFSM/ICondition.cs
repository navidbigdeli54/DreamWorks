/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface ICondition
    {
        #region Methods
        bool Evaluate();
        #endregion
    }
}