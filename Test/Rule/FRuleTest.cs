using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.Rule;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;
using DreamMachineGameStudio.Dreamworks.Blackboard;

namespace DreamMachineGameStudio.Dreamworks.Test.Rule
{
    public class FRuleTest
    {
        [Test]
        public void EvaluateCriteriaEqualTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 1);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.Equal, new FInt(1));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaIsNotEqualTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.Equal, new FInt(1));

            FAssert.IsFalse(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaNotEqualTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.NotEqual, new FInt(1));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaGreaterThanTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.GreaterThan, new FInt(1));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaGreaterThanEqualTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.GreaterThanEqual, new FInt(2));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaLessThanTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.LessThan, new FInt(3));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void EvaluateCriteriaLessThanEqualTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FInt>("FloatValue", 2);

            ICriteria criteria = new FCriteria(blackboard, "FloatValue", EValueComparer.LessThanEqual, new FInt(2));

            FAssert.IsTrue(criteria.Evaluate());
        }

        [Test]
        public void RuleDBTest()
        {
            FBlackboard blackboard = new FBlackboard();
            blackboard.AddValue<FString>("Who", "Akira");
            blackboard.AddValue<FString>("Where", "Village");
            blackboard.AddValue<FInt>("Level", 10);
            blackboard.AddValue<FInt>("Food", 10);
            blackboard.AddValue<FInt>("Wounds", 1);

            FRuleDB ruleDB = new FRuleDB();
            ruleDB.AddRule(new FRule().AddCriteria(blackboard, "Who", new FString("Akira")).AddCriteria(blackboard, "Level", EValueComparer.GreaterThan, new FInt(10)));
            ruleDB.AddRule(new FRule().AddCriteria(blackboard, "Who", new FString("Akira")).AddCriteria(blackboard, "Wounds", EValueComparer.GreaterThan, new FInt(2)));
            ruleDB.AddRule(new FRule().AddCriteria(blackboard, "Where", new FString("Village")).AddCriteria(blackboard, "Food", EValueComparer.LessThan, new FInt(2)));

            FAssert.IsFalse(ruleDB.Evaluate());
        }
    }
}