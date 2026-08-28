using DreamMachineGameStudio.Dreamworks.TaskScheduler;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.Log;
using DreamMachineGameStudio.DreamWorks.Modules.TaskScheduler.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamMachineGameStudio.DreamWorks.Modules.TaskScheduler
{
    [ADreamWorksSubSystem(
        displayName: "Task Scheduler",
        description: "Provides facilities to schedule tasks to be run with a delay.",
        category: "Core",
        order: 30,
        Experimental = false,
        Advanced = false,
        Keywords = "task scheduler")]
    public class FTaskSchdulerSubSystem : FGameInstanceSubSystem, ITaskSchedulerSubSystem
    {
        #region Fields
        private readonly float tickSubsteps = 5.0f;

        private readonly List<FTaskDefinitionBase> scheduledTasks = new List<FTaskDefinitionBase>(50);

        private readonly FScopedLogger scopedLogger = new FScopedLogger(new FLogCategory(nameof(FTaskSchdulerSubSystem), ELogVerbosity.Verbose));
        #endregion

        #region Properties
        public override bool CanTick => true;

        public override Type RegistrationType => typeof(ITaskSchedulerSubSystem);
        #endregion

        #region Constructors
        public FTaskSchdulerSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
        }
        #endregion

        #region ITaskScheduler Implementation
        FScheduledTaskHandle ITaskSchedulerSubSystem.Schedule(Action action, float delay)
        {
            var task = new FOneShotTaskDefinition(scopedLogger, action, delay);

            scheduledTasks.Add(task);

            return task.Handle;
        }

        FScheduledTaskHandle ITaskSchedulerSubSystem.Every(Action action, float interval, bool startDelayed)
        {
            var task = new FLoopedTaskDefinition(scopedLogger, action, interval, startDelayed);

            scheduledTasks.Add(task);

            return task.Handle;
        }

        void ITaskSchedulerSubSystem.Cancel(FScheduledTaskHandle handle)
        {
            FTaskDefinitionBase task = scheduledTasks.SingleOrDefault(x => x.Handle == handle);
            if (task != null)
            {
                task.IsCompleted = true;
            }
        }
        #endregion

        #region Protected Methods
        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            RemoveCompletedTasks();

            TryExecuteScheduledTasks(deltaTime);
        }
        #endregion

        #region Private Methods
        private void RemoveCompletedTasks()
        {
            scheduledTasks.RemoveAll(x=>x.IsCompleted);
        }

        private void TryExecuteScheduledTasks(float deltaTime)
        {
            float subStepDeltaTime = deltaTime / tickSubsteps;

            for (int i = 0; i < tickSubsteps; i++)
            {
                for (int j = 0; j < scheduledTasks.Count; j++)
                {
                    FTaskDefinitionBase task = scheduledTasks[j];

                    if (task.IsCompleted) continue;

                    task.Tick(subStepDeltaTime);

                    if (task.IsReady())
                    {
                        task.Execute();
                    }
                }
            }
        }
        #endregion
    }
}