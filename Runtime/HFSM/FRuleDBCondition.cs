/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using DreamMachineGameStudio.Dreamworks.Rule;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FRuleDBCondition : ICondition
    {
        #region Fields
        private readonly FRuleDB _ruleDB;
        #endregion

        #region Constructors
        public FRuleDBCondition(FRuleDB ruleDB) => this._ruleDB = ruleDB;
        #endregion

        #region ICondition Implementation
        bool ICondition.Evaluate(FHFSM _) => _ruleDB.Evaluate();
        #endregion
    }
}