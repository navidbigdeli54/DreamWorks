using System;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public sealed class FLoopedTaskDefinition : FTaskDefinitionBase
    {
        #region Fields
        private float accumulatedTime;

        private readonly float interval;

        private bool isFirstTime;
        #endregion

        #region Constructors
        public FLoopedTaskDefinition(ILogProvider logProvider, Action action, float interval, bool startDelayed)
            : base(logProvider, action)
        {
            this.accumulatedTime = 0;

            this.isFirstTime = startDelayed;

            this.interval = interval;
        }
        #endregion

        #region Public Methods
        internal override void Tick(float deltaTime)
        {
            accumulatedTime += deltaTime;
        }

        internal override bool IsReady()
        {
            if (isFirstTime)
            {
                return true;
            }

            return accumulatedTime >= interval;
        }

        internal override void Execute()
        {
            try
            {
                action?.Invoke();

                isFirstTime = false;

                accumulatedTime -= interval;
            }
            catch (Exception exception)
            {
                logProvider.LogError($"Encounter an error while executing {action.Method}: {exception}");
            }
        }
        #endregion
    }
}