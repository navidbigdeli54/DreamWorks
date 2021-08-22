using System.Linq;
using NUnit.Framework;
using DreamMachineGameStudio.Dreamworks.HFSM;

namespace DreamMachineGameStudio.Dreamworks.Test
{
    public class FHFSMTest
    {
        #region Tests
        [Test]
        public void SimpleHFSMTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");
            FDummyState s2 = new FDummyState("s2");

            s1.AddTransition("GoToS2", s2);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s1);

            machine.PushTrigger("GoToS2");

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s2);
        }

        [Test]
        public void TransitionBetweenHierarhcyInDifferentLevelsTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");
            FDummyState s11 = new FDummyState("s11", s1);
            FDummyState s12 = new FDummyState("s12", s1);
            FDummyState s2 = new FDummyState("s2");

            s1.SetInitialState(s11);
            s11.AddTransition("GoToS12", s12);

            s12.AddTransition("GoToS2", s2);

            s2.AddTransition("GoToS12", s12);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s11);

            machine.PushTrigger("GoToS12");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s12);

            machine.PushTrigger("GoToS2");

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s2);

            machine.PushTrigger("GoToS12");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s12);
        }

        [Test]
        public void FParallelStateTest()
        {
            FHFSM machine = new FHFSM(null);

            FParallelState s1 = new FParallelState("s1");
            FDummyState s11 = new FDummyState("s11", s1);
            FDummyState s111 = new FDummyState("s111", s11);
            FDummyState s112 = new FDummyState("s112", s11);
            s11.SetInitialState(s111);
            s111.AddTransition("GoToS112", s112);

            FDummyState s12 = new FDummyState("s12", s1);
            FDummyState s121 = new FDummyState("s121", s12);
            FDummyState s122 = new FDummyState("s122", s12);
            s12.SetInitialState(s121);
            s121.AddTransition("GoToS122", s122);

            FDummyState s2 = new FDummyState("s2");

            s1.AddTransition("GoToS2", s2);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 5);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
            Assert.IsTrue(machine.ActiveStates.Contains(s111));
            Assert.IsTrue(machine.ActiveStates.Contains(s12));
            Assert.IsTrue(machine.ActiveStates.Contains(s121));

            machine.PushTrigger("GoToS112");

            Assert.IsTrue(machine.ActiveStates.Count == 5);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
            Assert.IsTrue(machine.ActiveStates.Contains(s112));
            Assert.IsTrue(machine.ActiveStates.Contains(s12));
            Assert.IsTrue(machine.ActiveStates.Contains(s121));

            machine.PushTrigger("GoToS122");

            Assert.IsTrue(machine.ActiveStates.Count == 5);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
            Assert.IsTrue(machine.ActiveStates.Contains(s112));
            Assert.IsTrue(machine.ActiveStates.Contains(s12));
            Assert.IsTrue(machine.ActiveStates.Contains(s122));

            machine.PushTrigger("GoToS2");

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s2);
        }

        [Test]
        public void FParallelStateInTheMiddleOfHierarchyTest()
        {
            FHFSM machine = new FHFSM(null);

            FDummyState s1 = new FDummyState("s1");
            FDummyState s11 = new FDummyState("s11", s1);
            s1.SetInitialState(s11);

            FParallelState s12 = new FParallelState("s12", s1);
            FDummyState s121 = new FDummyState("s121", s12);
            FDummyState s1211 = new FDummyState("s1211", s121);
            s121.SetInitialState(s1211);

            FDummyState s122 = new FDummyState("s122", s12);
            FDummyState s1221 = new FDummyState("s1221", s122);
            s122.SetInitialState(s1221);

            s1221.AddTransition("GoToS11", s11);
            s11.AddTransition("GoToS1221", s1221);

            machine.AddState(s1);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));

            machine.PushTrigger("GoToS1221");

            Assert.IsTrue(machine.ActiveStates.Count == 6);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s12));
            Assert.IsTrue(machine.ActiveStates.Contains(s121));
            Assert.IsTrue(machine.ActiveStates.Contains(s1211));
            Assert.IsTrue(machine.ActiveStates.Contains(s122));
            Assert.IsTrue(machine.ActiveStates.Contains(s1221));

            machine.PushTrigger("GoToS11");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
        }

        [Test]
        public void ParallelSiblingsTest()
        {
            FHFSM machine = new FHFSM(null);

            FDummyState s1 = new FDummyState("s1");

            FParallelState s11 = new FParallelState("s11", s1);
            FParallelState s12 = new FParallelState("s12", s1);

            FDummyState s111 = new FDummyState("s111", s11);
            FDummyState s112 = new FDummyState("s112", s11);
            s11.SetInitialState(s111);

            FDummyState s121 = new FDummyState("s121", s12);
            FDummyState s122 = new FDummyState("s122", s12);
            s12.SetInitialState(s121);

            FDummyState s2 = new FDummyState("s2");

            s2.AddTransition("GoToS111", s111);
            s111.AddTransition("GoToS121", s121);

            s122.AddTransition("GoToS112", s112);
            s112.AddTransition("GoToS11", s2);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s2);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates.Contains(s2));

            machine.PushTrigger("GoToS111");

            Assert.IsTrue(machine.ActiveStates.Count == 4);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
            Assert.IsTrue(machine.ActiveStates.Contains(s111));
            Assert.IsTrue(machine.ActiveStates.Contains(s112));

            machine.PushTrigger("GoToS121");

            Assert.IsTrue(machine.ActiveStates.Count == 4);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s12));
            Assert.IsTrue(machine.ActiveStates.Contains(s121));
            Assert.IsTrue(machine.ActiveStates.Contains(s122));

            machine.PushTrigger("GoToS112");

            Assert.IsTrue(machine.ActiveStates.Count == 4);
            Assert.IsTrue(machine.ActiveStates.Contains(s1));
            Assert.IsTrue(machine.ActiveStates.Contains(s11));
            Assert.IsTrue(machine.ActiveStates.Contains(s111));
            Assert.IsTrue(machine.ActiveStates.Contains(s112));

            machine.PushTrigger("GoToS11");
            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates.Contains(s2));
        }

        [Test]
        public void HistroyNodeTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");
            FDummyState s11 = new FDummyState("s11", s1);
            FDummyState s12 = new FDummyState("s12", s1);
            FHistoryState h1 = new FHistoryState("h*", s1);

            FDummyState s2 = new FDummyState("s2");

            s1.SetInitialState(s11);
            s11.AddTransition("GoToS12", s12);

            s12.AddTransition("GoToS2", s2);

            s2.AddTransition("GoToHistory", h1);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s11);

            machine.PushTrigger("GoToS12");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s12);

            machine.PushTrigger("GoToS2");

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s2);

            machine.PushTrigger("GoToHistory");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s12);
        }

        [Test]
        public void EnterHierarchyofInitialStatesTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");
            FDummyState s11 = new FDummyState("s11", s1);
            s1.SetInitialState(s11);
            FDummyState s111 = new FDummyState("s111", s11);
            s11.SetInitialState(s111);
            FDummyState s1111 = new FDummyState("s1111", s111);
            s111.SetInitialState(s1111);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 4);
            Assert.IsTrue(machine.ActiveStates[0] == s1);
            Assert.IsTrue(machine.ActiveStates[1] == s11);
            Assert.IsTrue(machine.ActiveStates[2] == s111);
            Assert.IsTrue(machine.ActiveStates[3] == s1111);
        }

        [Test]
        public void EnterInitialStateInLeafStateTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");
            FDummyState s2 = new FDummyState("s2");
            FDummyState s21 = new FDummyState("s21", s2);
            s2.SetInitialState(s21);

            s1.AddTransition("GoToS2", s2);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s1);

            machine.PushTrigger("GoToS2");

            Assert.IsTrue(machine.ActiveStates.Count == 2);
            Assert.IsTrue(machine.ActiveStates[0] == s2);
            Assert.IsTrue(machine.ActiveStates[1] == s21);

        }

        [Test]
        public void EnterInitialStateInLeafOfFParallelStateTest()
        {
            FHFSM machine = new FHFSM(null);
            FDummyState s1 = new FDummyState("s1");

            FParallelState s2 = new FParallelState("s2");
            FDummyState s21 = new FDummyState("s21", s2);
            FDummyState s211 = new FDummyState("s211", s21);
            FDummyState s212 = new FDummyState("s212", s21);
            s21.SetInitialState(s211);

            FDummyState s22 = new FDummyState("s22", s2);
            FDummyState s221 = new FDummyState("s221", s22);
            FDummyState s2211 = new FDummyState("s2211", s221);
            s22.SetInitialState(s221);
            s221.SetInitialState(s2211);

            s1.AddTransition("GoToS212", s212);

            machine.AddState(s1);
            machine.AddState(s2);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 1);
            Assert.IsTrue(machine.ActiveStates[0] == s1);

            machine.PushTrigger("GoToS212");

            Assert.IsTrue(machine.ActiveStates.Count == 6);

            Assert.IsFalse(s1.IsActive);  // False.

            Assert.IsTrue(s2.IsActive);

            Assert.IsTrue(s21.IsActive);
            Assert.IsFalse(s211.IsActive);  // False.
            Assert.IsTrue(s212.IsActive);

            Assert.IsTrue(s22.IsActive);
            Assert.IsTrue(s221.IsActive);
            Assert.IsTrue(s2211.IsActive);
        }

        [Test]
        public void HierarchyOfFParallelStateTest()
        {
            FHFSM machine = new FHFSM(null);

            FParallelState s1 = new FParallelState("s1");
            FParallelState s11 = new FParallelState("s11", s1);
            FDummyState s111 = new FDummyState("s111", s11);
            FDummyState s112 = new FDummyState("s112", s11);

            FParallelState s12 = new FParallelState("s12", s1);
            FDummyState s121 = new FDummyState("s121", s12);
            FDummyState s122 = new FDummyState("s122", s12);

            machine.SetInitialState(s1);

            machine.Start();

            Assert.IsTrue(machine.ActiveStates.Count == 7);
        }
        #endregion

        #region Nested Types
        private class FDummyState : FState
        {
            public FDummyState(string name) : base(name) { }

            public FDummyState(string name, IState parent) : base(name, parent) { }
        }
        #endregion
    }
}
