using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public sealed class FTrigger : FStringId
    {
        #region Fields
        public readonly static FTrigger Empty = new FTrigger(string.Empty);
        #endregion

        #region Constructor
        public FTrigger(string str) : base(str) { }
        #endregion

        #region OperatorOverloading
        public static implicit operator FTrigger(string trigger) => new FTrigger(trigger);
        #endregion
    }
}