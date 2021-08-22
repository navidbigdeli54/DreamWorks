/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using DreamMachineGameStudio.Dreamworks.Rule;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FCondition : ICondition
    {
        #region Fields
        private readonly FRuleDB _ruleDB;
        #endregion

        #region Constructors
        public FCondition(FRuleDB ruleDB) => this._ruleDB = ruleDB;
        #endregion

        #region ICondition Implementation
        bool ICondition.Evaluate() => _ruleDB.Evaluate();
        #endregion
    }
}