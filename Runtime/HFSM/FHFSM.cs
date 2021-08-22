/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Blackboard;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public sealed class FHFSM : IHFSM
    {
        #region Fields
        public readonly FBlackboard Blackboard;

        public readonly CHFSMBrain Owner;

        private IState _initialState;

        private bool _isCheckingTransitions;

        private readonly List<IState> _activeStates = new List<IState>();

        private readonly List<IState> _activeStatesThisFrame = new List<IState>();

        private readonly Dictionary<FName, IState> _pooledStates = new Dictionary<FName, IState>();

        private readonly Queue<FTrigger> _pushedTriggers = new Queue<FTrigger>();
        #endregion

        #region Constructor
        public FHFSM(CHFSMBrain owner)
        {
            Owner = owner;
            Blackboard = new FBlackboard();
        }
        #endregion

        #region Properties
        public IReadOnlyList<IState> ActiveStates => _activeStates;
        #endregion

        #region Public Methods
        public void PushTrigger(FTrigger trigger)
        {
            _pushedTriggers.Enqueue(trigger);

            if (_isCheckingTransitions == false)
            {
                _isCheckingTransitions = true;
                CheckTransitions();
                _isCheckingTransitions = false;
            }
        }

        public void AddState(IState state)
        {
            FAssert.IsFalse(_pooledStates.ContainsKey(state.Name), $"State `{state.Name}` is already exist in HFSM.");

            _pooledStates.Add(state.Name, state);
            state.SetMachine(this);

            for (int i = 0; i < state.Children.Count; ++i)
            {
                AddState(state.Children[i]);
            }
        }

        public void SetInitialState(IState initialState)
        {
            _initialState = initialState;
        }

        public void Start()
        {
            EnterHierarhcyFromClosestCommonAncestor(null, new FTransition(_initialState));
        }
        #endregion

        #region Private Methods
        private void CheckTransitions()
        {
            while (_pushedTriggers.Count != 0)
            {
                FTrigger trigger = _pushedTriggers.Dequeue();

                for (int i = 0; i < _activeStates.Count; ++i)
                {
                    IState state = _activeStates[i];

                    ITransition transition = state.CheckTransitions(trigger);

                    if (transition != null)
                    {
                        if (transition.Target is IHistoryState historyState)
                        {
                            if (historyState.LastState == null)
                            {
                                FAssert.IsNotNull(historyState.Parent, $"`{historyState.Name}` parent is null. A history state should always have parent!");
                                historyState.LastState = historyState.Parent.InitialState;
                            }

                            FireTransition(state, new FTransition(transition.Trigger, historyState.LastState, transition.Condition, transition.Actions));
                        }
                        else
                        {
                            FireTransition(state, transition);
                        }

                        break;
                    }
                }
            }
        }

        private void FireTransition(IState source, ITransition transition)
        {
            FAssert.IsTrue(_pooledStates.ContainsKey(transition.Target.Name), $"{transition.Target.Name} is not exist in this machine.");

            ExitHierarchyToClosestCommonAncestor(source, transition);
            transition.PerformActions();
            EnterHierarhcyFromClosestCommonAncestor(source, transition);
        }

        private void ExitHierarchyToClosestCommonAncestor(IState source, ITransition transition)
        {
            IReadOnlyList<IState> hierarchyToExit = GetOverallHierarchyToExit(source, transition.Target);

            for (int i = 0; i < hierarchyToExit.Count; ++i)
            {
                ExitState(hierarchyToExit[i]);
            }
        }

        private IReadOnlyList<IState> GetOverallHierarchyToExit(IState source, IState target)
        {
            List<IState> result = new List<IState>();

            IReadOnlyList<IState> sourceAncestors = source.GetAncestors(); // Returns the source itself too!
            for (int i = 0; i < sourceAncestors.Count; ++i)
            {
                IState ancestor = sourceAncestors[i];
                if (target.HasAsAncestor(ancestor))
                {
                    // We have reached to the common ancestor of source and target state, so should not proceed further.
                    break;
                }

                IReadOnlyList<IState> activeChildrenState = GetActiveChildrenStates(ancestor);
                for (int j = 0; j < activeChildrenState.Count; ++j)
                {
                    IState state = activeChildrenState[j];
                    if (result.Contains(state) == false)
                    {
                        result.Add(state);
                    }
                }
            }

            return result;
        }

        private IReadOnlyList<IState> GetActiveChildrenStates(IState state)
        {
            List<IState> result = new List<IState>();

            for (int i = _activeStates.Count - 1; i >= 0; --i)
            {
                IState activeState = ActiveStates[i];

                if (activeState.HasAsAncestor(state) || activeState == state)
                {
                    result.Add(activeState);
                }
            }

            return result;
        }

        private void ExitState(IState state)
        {
            state.OnExit();
            _activeStates.Remove(state);
        }

        private void EnterHierarhcyFromClosestCommonAncestor(IState source, ITransition transition)
        {
            IReadOnlyList<IState> hierarchyToEnter = GetOverallHierarchyToEnter(source, transition.Target);

            for (int i = 0; i < hierarchyToEnter.Count; ++i)
            {
                EnterState(hierarchyToEnter[i], transition);
            }
        }

        private IReadOnlyList<IState> GetOverallHierarchyToEnter(IState source, IState target)
        {
            List<IState> result = new List<IState>();

            IReadOnlyList<IState> targetAncestors = target.GetAncestors(); // Returns the source itself too!

            // We should enter to states from top to down, so we loop through ancestors backward.
            for (int i = targetAncestors.Count - 1; i >= 0; --i)
            {
                IState ancestor = targetAncestors[i];

                // Ignore common ancestors.
                if (source != null && source.HasAsAncestor(ancestor))
                {
                    continue;
                }

                result.AddRange(GetStateHierarchyToEnter(ancestor, target));
            }

            return result;
        }

        private IReadOnlyList<IState> GetStateHierarchyToEnter(IState state, IState target)
        {
            List<IState> result = new List<IState>
            {
                state
            };

            if (state is IParallelState parallelState)
            {
                result.AddRange(GetParallelStateHierarhcyToEnter(parallelState, target));
            }
            else if (target.HasAsAncestor(state) == false && state.InitialState != null)
            {
                //We should activate initialState of states that are NOT on the path to target.
                result.AddRange(GetStateHierarchyToEnter(state.InitialState, target));
            }

            return result;
        }

        private IReadOnlyList<IState> GetParallelStateHierarhcyToEnter(IParallelState parallelState, IState target)
        {
            List<IState> result = new List<IState>();

            for (int i = 0; i < parallelState.Children.Count; ++i)
            {
                IState child = parallelState.Children[i];

                // Ignore parallel children on the path to target, they will be handled on top level functions.
                if (child == target || target.HasAsAncestor(child))
                {
                    continue;
                }

                result.AddRange(GetStateHierarchyToEnter(child, target));
            }

            return result;
        }

        private void EnterState(IState state, ITransition transition)
        {
            _activeStates.Add(state);
            state.OnEnter();

            if (transition != null && transition.Trigger == FTrigger.Empty)
            {
                ITransition newTransition = state.CheckTransitions(FTrigger.Empty);
                if (newTransition != null)
                {
                    FireTransition(state, newTransition);
                }
            }
        }

        private void SyncActiveStatesThisFrame()
        {
            _activeStatesThisFrame.Clear();
            _activeStatesThisFrame.AddRange(_activeStates);
        }
        #endregion

        #region INameable Implementation
        FName INameable.Name => Owner.name;
        #endregion

        #region IInitializable Implementation
        async Task IInitializable.PreInitializeAsync()
        {
            foreach (KeyValuePair<FName, IState> statePair in _pooledStates)
            {
                await statePair.Value.PreInitializeAsync();
            }
        }

        async Task IInitializable.InitializeAsync()
        {
            foreach (KeyValuePair<FName, IState> statePair in _pooledStates)
            {
                await statePair.Value.InitializeAsync();
            }
        }

        async Task IInitializable.BeginPlayAsync()
        {
            foreach (KeyValuePair<FName, IState> statePair in _pooledStates)
            {
                await statePair.Value.BeginPlayAsync();
            }
        }

        async Task IInitializable.UninitializeAsync()
        {
            foreach (KeyValuePair<FName, IState> statePair in _pooledStates)
            {
                await statePair.Value.UninitializeAsync();
            }
        }
        #endregion

        #region ITickable Implementation

        void ITickable.Tick(float deltaTime)
        {
            SyncActiveStatesThisFrame();

            for (int i = 0; i < _activeStatesThisFrame.Count; ++i)
            {
                IState state = _activeStatesThisFrame[i];
                if (state.IsActive)
                {
                    state.Tick(deltaTime);
                }
            }
        }

        void ITickable.LateTick(float deltaTime)
        {
            for (int i = 0; i < _activeStatesThisFrame.Count; ++i)
            {
                IState state = _activeStatesThisFrame[i];
                if (state.IsActive)
                {
                    state.LateTick(deltaTime);
                }
            }
        }

        void ITickable.FixedTick(float fixedDeltaTime)
        {
            for (int i = 0; i < _activeStatesThisFrame.Count; ++i)
            {
                IState state = _activeStatesThisFrame[i];
                if (state.IsActive)
                {
                    state.FixedTick(fixedDeltaTime);
                }
            }
        }
        #endregion
    }
}