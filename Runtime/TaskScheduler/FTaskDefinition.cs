/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public class FTaskDefinition
    {
        #region Fields
        private static int HandleCounter = -1;

        private Action _task;

        private float _remainingTime;
        #endregion

        #region Properties

        public FTaskHandle Handle { get; private set; }
        #endregion

        #region Constructor
        public FTaskDefinition(Action task, float remainingTime)
        {
            ++HandleCounter;

            _task = task;
            Handle = new FTaskHandle(HandleCounter);
            _remainingTime = remainingTime;
        }
        #endregion

        #region Public Methods
        public void Tick(float deltaTime)
        {
            _remainingTime -= deltaTime;
        }

        public bool IsReadyToPerform()
        {
            return _remainingTime <= 0;
        }

        public void PerformTask()
        {
            _task?.Invoke();
        }
        #endregion
    }
}