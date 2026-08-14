using System;
using DreamMachineGameStudio.DreamWorks.Modules.TaskScheduler;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction.Logger;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public abstract class FTaskDefinitionBase
    {
        #region Fields
        private static int HandleCounter = -1;

        protected readonly Action action;

        protected readonly ILogProvider logProvider;
        #endregion

        #region Properties
        public FScheduledTaskHandle Handle { get; private set; }

        public bool IsCompleted { get; internal set; }
        #endregion

        #region Constructor
        internal FTaskDefinitionBase(ILogProvider logProvider, Action action)
        {
            ++HandleCounter;

            this.action = action;

            this.logProvider = logProvider;

            Handle = new FScheduledTaskHandle(HandleCounter);
        }
        #endregion

        #region Public Methods
        internal abstract void Tick(float deltaTime);

        internal abstract bool IsReady();

        internal abstract void Execute();
        #endregion
    }
}