/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
	public interface ITaskScheduler
	{
		#region Public Methods
		void ScheduleTask(float seconds, Action task);
		#endregion
	}
}