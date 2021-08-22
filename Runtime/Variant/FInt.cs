/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [FNameAttribute("Int")]
    [Serializable]
    public sealed class FInt : FVariant<int>
    {
        #region Constructors
        public FInt(int value) : base(value) { }

        public FInt(bool isReadOnly, int value) : base(isReadOnly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FInt castedOther = other as FInt;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FInt)}");

            if (other == null || castedOther == null)
            {
                return false;
            }
            else if ((object)this == other)
            {
                return true;
            }
            else
            {
                return _value == castedOther._value;
            }
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        #endregion

        #region Protected Methods
        protected override bool Equals(IVariant other)
        {
            FInt castedOther = other as FInt;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FInt)}");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FInt castedOther = other as FInt;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FInt)}");

            return _value.CompareTo(castedOther._value);
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FInt(int i) => new FInt(i);

        public static implicit operator int(FInt i) => i._value;

        public static bool operator ==(FInt lhs, FInt rhs) => Equals(rhs, lhs);

        public static bool operator !=(FInt lhs, FInt rhs) => !Equals(rhs, lhs);

        public static bool Equals(FInt lhs, FInt rhs)
        {
            if ((object)lhs == rhs)
            {
                return true;
            }
            else if (null == lhs || null == rhs)
            {
                return false;
            }
            else
            {
                return rhs._value == lhs._value;
            }
        }
        #endregion
    }
}