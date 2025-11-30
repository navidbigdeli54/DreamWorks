/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.TaskScheduler
{
    public struct FTaskHandle
    {
        #region Properties
        public int Number { get; private set; }

        public readonly bool IsValid => Number >= 0;
        #endregion

        #region Constructors
        public FTaskHandle(int number)
        {
            Number = number;
        }
        #endregion

        #region Operator Overloads
        public static bool operator ==(FTaskHandle left, FTaskHandle right)
        {
            return left.Number == right.Number;
        }

        public static bool operator !=(FTaskHandle left, FTaskHandle right)
        {
            return !(left == right);
        }

        public override bool Equals(object other)
        {
            return other is FTaskHandle handle && Number == handle.Number;
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