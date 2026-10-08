using System;

namespace DreamMachineGameStudio.DreamWorks.TaskScheduler.Abstraction
{
    public interface ITaskSchedulerSubSystem
    {
        FScheduledTaskHandle Schedule(Action action, float delay);

        FScheduledTaskHandle Every(Action action, float interval, bool startDelayed = false);

        void Cancel(FScheduledTaskHandle handle);
    }
}