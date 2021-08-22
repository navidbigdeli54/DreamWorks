/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public sealed class FTransition : ITransition
    {
        #region Fields
        public static readonly FTransition Empty = new FTransition(FTrigger.Empty);

        private readonly FTrigger _trigger;

        private readonly IState _target;

        private readonly ICondition _condition;

        private readonly List<ITransitionAction> _actions = new List<ITransitionAction>();
        #endregion

        #region Constructors
        private FTransition(FTrigger trigger)
        {
            this._trigger = trigger;
        }

        public FTransition(IState target) : this(FTrigger.Empty)
        {
            this._target = target;
        }

        public FTransition(FTrigger trigger, IState target) : this(trigger)
        {
            this._target = target;
        }

        public FTransition(FTrigger trigger, IState target, ITransitionAction action) : this(trigger, target)
        {
            _actions.Add(action);
        }

        public FTransition(FTrigger trigger, IState target, IEnumerable<ITransitionAction> actions) : this(trigger, target)
        {
            this._actions.AddRange(actions);
        }

        public FTransition(FTrigger trigger, IState target, ICondition condition) : this(trigger, target)
        {
            this._condition = condition;
        }

        public FTransition(FTrigger trigger, IState target, ICondition condition, ITransitionAction action) : this(trigger, target, condition)
        {
            _actions.Add(action);
        }

        public FTransition(FTrigger trigger, IState target, ICondition condition, IEnumerable<ITransitionAction> actions) : this(trigger, target, condition)
        {
            this._actions.AddRange(actions);
        }
        #endregion

        #region ITransition Implementation
        IState ITransition.Target => _target;

        FTrigger ITransition.Trigger => _trigger;

        ICondition ITransition.Condition => _condition;

        IReadOnlyList<ITransitionAction> ITransition.Actions => _actions;

        bool ITransition.IsTriggered(FTrigger trigger)
        {
            return this._trigger == trigger && (_condition == null || _condition.Evaluate());
        }

        void ITransition.PerformActions()
        {
            for (int i = 0; i < _actions.Count; ++i)
            {
                _actions[i].Perform();
            }
        }
        #endregion
    }
}