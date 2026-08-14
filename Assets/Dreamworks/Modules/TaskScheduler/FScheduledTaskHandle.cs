using System;

namespace DreamMachineGameStudio.DreamWorks.Modules.TaskScheduler
{
    public struct FScheduledTaskHandle
    {
        #region Properties
        public int Number { get; private set; }

        public readonly bool IsValid => Number >= 0;
        #endregion

        #region Constructors
        public FScheduledTaskHandle(int number)
        {
            Number = number;
        }
        #endregion

        #region Operator Overloads
        public static bool operator ==(FScheduledTaskHandle left, FScheduledTaskHandle right)
        {
            return left.Number == right.Number;
        }

        public static bool operator !=(FScheduledTaskHandle left, FScheduledTaskHandle right)
        {
            return !(left == right);
        }

        public override bool Equals(object other)
        {
            return other is FScheduledTaskHandle handle && Number == handle.Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }
        #endregion

        #region Public Methods
        public void Invalidate()
        {
            Number = -1;
        } 
        #endregion
    }
}