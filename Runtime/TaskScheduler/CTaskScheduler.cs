/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public class CTaskScheduler : CComponent, ITaskScheduler
    {
        #region Fields
        private List<FTaskDefinition> _taskDefinitions = new List<FTaskDefinition>(50);

        private List<int> _taskToRemove = new List<int>(50);
        #endregion

        #region CComponent Methods
        protected override async Task PreInitializeComponenetAsync()
        {
            await base.PreInitializeComponenetAsync();

            MakePersistent();
        }

        protected override async Task InitializeComponentAsync()
        {
            await base.InitializeComponentAsync();

            CanEverTick = true;
        }

        protected override void TickComponent(float deltaTime)
        {
            base.TickComponent(deltaTime);

            RemoveDepricatedTasks();

            for (int i = 0; i < _taskDefinitions.Count; ++i)
            {
                FTaskDefinition taskDefinition = _taskDefinitions[i];

                taskDefinition.Tick(deltaTime);

                if (taskDefinition.IsReadyToPerform())
                {
                    try
                    {
                        AddTaskToRemove(i);

                        taskDefinition.PerformTask();
                    }
                    catch (Exception exception)
                    {
                        FLog.Error(nameof(CTaskScheduler), exception);
                    }
                }
            }

            RemoveDepricatedTasks();
        }
        #endregion

        #region Private Methods
        private void AddTaskToRemove(int index)
        {
            _taskToRemove.Add(index);
        }

        private void RemoveDepricatedTasks()
        {
            for (int i = 0; i < _taskToRemove.Count; ++i)
            {
                int index = _taskToRemove[i];

                _taskDefinitions.RemoveAt(index);
            }

            _taskToRemove.Clear();
        }
        #endregion

        #region ITaskScheduler Implementation
        FTaskHandle ITaskScheduler.ScheduleTask(float seconds, Action task)
        {
            FTaskDefinition createdTask = new FTaskDefinition(task, seconds);
            _taskDefinitions.Add(createdTask);
            return createdTask.Handle;
        }

        void ITaskScheduler.RemoveTask(FTaskHandle handle)
        {
            for (int i = 0; i < _taskDefinitions.Count; ++i)
            {
                if (_taskDefinitions[i].Handle == handle)
                {
                    AddTaskToRemove(i);

                    break;
                }
            }
        }
        #endregion
    }
}