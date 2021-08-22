/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface ITransition
    {
        #region Properties
        IState Target { get; }

        FTrigger Trigger { get; }

        ICondition Condition { get; }

        IReadOnlyList<ITransitionAction> Actions { get; }
        #endregion

        #region Methods
        bool IsTriggered(FTrigger trigger);

        void PerformActions();
        #endregion
    }
}