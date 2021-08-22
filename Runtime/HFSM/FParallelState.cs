/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public class FParallelState : FState, IParallelState
    {
        #region Constructors
        public FParallelState() : base() { }

        public FParallelState(string name) : base(name) { }

        public FParallelState(IState parent) : base(parent) { }

        public FParallelState(string name, IState parent) : base(name, parent) { }
        #endregion
    }
}