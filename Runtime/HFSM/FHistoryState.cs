/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public class FHistoryState : FState, IHistoryState
    {
        #region Constructors
        public FHistoryState(IState parent) : base(parent) { }

        public FHistoryState(string name, IState parent) : base(name, parent) { }
        #endregion

        #region IHistoryState Implementation
        IState IHistoryState.LastState { get; set; }
        #endregion
    }
}