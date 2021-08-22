/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Variant;
using DreamMachineGameStudio.Dreamworks.Blackboard;

namespace DreamMachineGameStudio.Dreamworks.Rule
{
    public sealed class FRule
    {
        #region Fields
        private readonly List<ICriteria> _criterias = new List<ICriteria>();
        #endregion

        #region Properties
        public IResponse Response { get; private set; }

        public IReadOnlyList<ICriteria> Criterias => _criterias;
        #endregion

        #region Methods
        public FRule SetResponse(IResponse respone)
        {
            this.Response = respone;

            return this;
        }

        public FRule AddCriteria(FBlackboard blackboard, FStringId key, IVariant expectedValue)
        {
            _criterias.Add(new FCriteria(blackboard, key, EValueComparer.Equal, expectedValue));

            return this;
        }

        public FRule AddCriteria(FBlackboard blackboard, FStringId key, EValueComparer comparer, IVariant expectedValue)
        {
            _criterias.Add(new FCriteria(blackboard, key, comparer, expectedValue));

            return this;
        }

        public bool Evaluate()
        {
            for (int i = 0; i < _criterias.Count; i++)
            {
                if (_criterias[i].Evaluate() == false)
                    return false;
            }

            return true;
        }
        #endregion
    }
}