using System;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public sealed class FOneShotTaskDefinition : FTaskDefinitionBase
    {
        #region Fields
        private float remainingTime;
        #endregion

        #region Constructors
        internal FOneShotTaskDefinition(ILogProvider logProvider, Action action, float delay)
            : base(logProvider, action)
        {
            this.remainingTime = delay;
        }
        #endregion

        #region Public Methods
        internal override void Tick(float deltaTime)
        {
            remainingTime -= deltaTime;
        }

        internal override bool IsReady()
        {
            return remainingTime <= 0;
        }

        internal override void Execute()
        {
            try
            {
                action?.Invoke();

                IsCompleted = true;
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Encounter an error while executing {action.Method}: {exception}");
            }
        }
        #endregion
    }
}