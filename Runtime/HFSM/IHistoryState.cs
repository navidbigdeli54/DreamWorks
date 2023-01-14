/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface IHistoryState : IState
    {
        #region Properties
        IState LastState { get; set; }
        #endregion
    }
}