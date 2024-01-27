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
		private List<TaskDefinition> _taskDefinitions = new List<TaskDefinition>(50);

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

			_taskToRemove.Clear();

			for (int i = 0; i < _taskDefinitions.Count; ++i)
			{
				TaskDefinition definition = _taskDefinitions[i];

				definition.RemainingTime -= deltaTime;

				if (definition.RemainingTime <= 0)
				{
					try
					{
						definition.Task?.Invoke();

						_taskToRemove.Add(i);
					}
					catch (Exception exception)
					{

						FLog.Error(nameof(CTaskScheduler), exception);
					}
				}
			}

			for (int i = 0; i < _taskToRemove.Count; ++i)
			{
				int index = _taskToRemove[i];

				_taskDefinitions.RemoveAt(index);
			}
		}
		#endregion

		#region ITaskScheduler Implementation
		void ITaskScheduler.ScheduleTask(float seconds, Action task)
		{
			_taskDefinitions.Add(new TaskDefinition { RemainingTime = seconds, Task = task });
		}
		#endregion
	}
}