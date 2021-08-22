/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public interface IState : IInitializable, ITickable
    {
        #region Properties
        bool IsActive { get; }

        IState Parent { get; }

        IState InitialState { get; }

        IHistoryState HistoryState { get; }

        IReadOnlyList<IState> Children { get; }
        #endregion

        #region Methods
        void OnEnter();

        void OnExit();

        ITransition CheckTransitions(FTrigger trigger);

        bool HasAsAncestor(IState state);

        IReadOnlyList<IState> GetAncestors();

        void AddChild(IState child);

        void SetParent(IState parent);

        void SetMachine(IHFSM machine);
        #endregion
    }
}