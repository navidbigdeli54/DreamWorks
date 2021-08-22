/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;
using DreamMachineGameStudio.Dreamworks.Blackboard;

namespace DreamMachineGameStudio.Dreamworks.Rule
{
    public sealed class FCriteria : ICriteria
    {
        #region Fields
        private readonly FStringId _key;

        private readonly IVariant _expectedValue;

        private readonly EValueComparer _comparer;

        private readonly FBlackboard _blackboard;
        #endregion

        #region Constructors
        public FCriteria(FBlackboard blackboard, FStringId key, EValueComparer comparer, IVariant expectedValue)
        {
            this._key = key;
            this._comparer = comparer;
            this._blackboard = blackboard;
            this._expectedValue = expectedValue;
        }
        #endregion

        #region Private Methods
        private bool CheckEquality(IVariant currentValue)
        {
            FAssert.AreEqual(_expectedValue.GetType(), currentValue.GetType(), $"Expected value is {_expectedValue.GetType().Name} but current value is {currentValue.GetType()}");

            if (currentValue == _expectedValue) return true;

            return currentValue.Equals(_expectedValue);
        }

        private int CompareValue(IVariant currentValue)
        {
            FAssert.AreEqual(_expectedValue.GetType(), currentValue.GetType(), $"Expected value is {_expectedValue.GetType().Name} but current value is {currentValue.GetType()}");

            return currentValue.CompareTo(_expectedValue);
        }
        #endregion

        #region ICriteria Implementation
        FStringId ICriteria.Key => _key;

        EValueComparer ICriteria.Comparer => _comparer;

        FBlackboard ICriteria.Blackboard => _blackboard;

        bool ICriteria.Evaluate()
        {
            if (_blackboard.TryGetValue(_key, out IVariant currentValue))
            {
                switch (_comparer)
                {
                    case EValueComparer.Equal:
                        return CheckEquality(currentValue);
                    case EValueComparer.NotEqual:
                        return !CheckEquality(currentValue);
                    case EValueComparer.GreaterThan:
                        return CompareValue(currentValue) > 0;
                    case EValueComparer.GreaterThanEqual:
                        return CompareValue(currentValue) >= 0;
                    case EValueComparer.LessThan:
                        return CompareValue(currentValue) < 0;
                    case EValueComparer.LessThanEqual:
                        return CompareValue(currentValue) <= 0;
                }
            }

            return false;
        }
        #endregion
    }
}