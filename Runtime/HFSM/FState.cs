/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class FState : IState
    {
        #region Fields
        private IState _initialState;

        private IHistoryState _historyState;

        private readonly FName _name;

        private readonly List<IState> _children = new List<IState>();

        private readonly List<ITransition> _transitions = new List<ITransition>();
        #endregion

        #region Constructor
        public FState()
        {
            _name = GetType().Name;
        }

        public FState(string name)
        {
            _name = name;
        }

        public FState(IState parent) : this()
        {
            parent.AddChild(this);
            Parent = parent;
        }

        public FState(string name, IState parent) : this(parent)
        {
            _name = name;
        }
        #endregion

        #region Properties
        public FHFSM Machine { get; private set; }

        public bool IsActive { get; private set; }

        protected IState Parent { get; private set; }
        #endregion

        #region Public Methods
        public override string ToString() => _name;

        public void SetInitialState(IState initialState) => _initialState = initialState;

        public void AddTransition(FTrigger trigger, IState target)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");

            _transitions.Add(new FTransition(trigger, target));
        }

        public void AddTransition(FTrigger trigger, IState target, ICondition condition)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");
            FAssert.IsNotNull(condition, "Condition can't be null");

            _transitions.Add(new FTransition(trigger, target, condition));
        }

        public void AddTransition(FTrigger trigger, IState target, ITransitionAction action)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");
            FAssert.IsNotNull(action, "Action can't be null");

            _transitions.Add(new FTransition(trigger, target, action));
        }

        public void AddTransition(FTrigger trigger, IState target, IEnumerable<ITransitionAction> actions)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");
            FAssert.IsNotNull(actions, "Actions can't be null");

            _transitions.Add(new FTransition(trigger, target, actions));
        }

        public void AddTransition(FTrigger trigger, IState target, ICondition condition, ITransitionAction action)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");
            FAssert.IsNotNull(condition, $"Condition can't be null.");
            FAssert.IsNotNull(action, "Action can't be null");

            _transitions.Add(new FTransition(trigger, target, condition, action));
        }

        public void AddTransition(FTrigger trigger, IState target, ICondition condition, IEnumerable<ITransitionAction> actions)
        {
            FAssert.IsTrue(trigger != null, "Trigger is invalid.");
            FAssert.IsNotNull(target, $"Target state can't be null.");
            FAssert.IsNotNull(condition, $"Condition can't be null.");
            FAssert.IsNotNull(actions, "Actions can't be null");

            _transitions.Add(new FTransition(trigger, target, condition, actions));
        }
        #endregion

        #region Protected Methods
        protected virtual void OnEnter() { }

        protected virtual void OnExit() { }

        protected virtual Task PreInitializeAsync() => Task.CompletedTask;

        protected virtual Task InitializeAsync() => Task.CompletedTask;

        protected virtual Task BeginPlayAsync() => Task.CompletedTask;

        protected virtual Task UninitializeAsync() => Task.CompletedTask;

        protected virtual void Tick(float deltaTime) { }

        protected virtual void LateTick(float deltaTime) { }

        protected virtual void FixedTick(float fixedDeltaTime) { }
        #endregion

        #region IState Implementation
        IState IState.Parent => Parent;

        IState IState.InitialState => _initialState;

        IHistoryState IState.HistoryState => _historyState;

        IReadOnlyList<IState> IState.Children => _children;

        void IState.OnEnter()
        {
            IsActive = true;

            if (Parent != null && Parent.HistoryState != null)
            {
                Parent.HistoryState.LastState = this;
            }

            OnEnter();
        }

        void IState.OnExit()
        {
            IsActive = false;

            OnExit();
        }

        ITransition IState.CheckTransitions(FTrigger trigger)
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                ITransition transaction = _transitions[i];

                if (transaction.IsTriggered(trigger))
                {
                    return transaction;
                }
            }

            return null;
        }

        bool IState.HasAsAncestor(IState state)
        {
            FAssert.IsNotNull(state, "State parameter can't be null.");

            IState currentState = Parent;
            while (currentState != null)
            {
                if (currentState == state) return true;

                currentState = currentState.Parent;
            }

            return false;
        }

        IReadOnlyList<IState> IState.GetAncestors()
        {
            List<IState> ancestors = new List<IState>();
            IState current = this;
            do
            {
                ancestors.Add(current);
                current = current.Parent;
            } while (current != null);

            return ancestors;
        }

        void IState.SetMachine(IHFSM machine)
        {
            Machine = (FHFSM)machine;
        }

        void IState.SetParent(IState parent)
        {
            Parent = parent;
        }

        void IState.AddChild(IState child)
        {
            FAssert.IsFalse(_children.Contains(child) || _historyState == child, $"`{child.Name}` is already child of `{_name}` state.");

            if (child is IHistoryState historyState)
            {
                _historyState = historyState;
            }
            else
            {
                _children.Add(child);
            }

            child.SetParent(this);
        }
        #endregion

        #region INameable Implementation
        FName INameable.Name => _name;
        #endregion

        #region IInitializable Implementation
        Task IInitializable.PreInitializeAsync() => PreInitializeAsync();

        Task IInitializable.InitializeAsync() => InitializeAsync();

        Task IInitializable.BeginPlayAsync() => BeginPlayAsync();

        Task IInitializable.UninitializeAsync() => UninitializeAsync();
        #endregion

        #region ITickable Implementation
        void ITickable.Tick(float deltaTime) => Tick(deltaTime);

        void ITickable.LateTick(float deltaTime) => LateTick(deltaTime);

        void ITickable.FixedTick(float fixedDeltaTime) => FixedTick(fixedDeltaTime);
        #endregion
    }
}