/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [FNameAttribute("Float")]
    [Serializable]
    public sealed class FFloat : FVariant<float>
    {
        #region Constructors
        public FFloat(float value) : base(value) { }

        public FFloat(bool isReadonly, float value) : base(isReadonly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FFloat castedOther = other as FFloat;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FFloat)}");

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

        public override int GetHashCode() => _value.GetHashCode();
        #endregion

        #region Protected Methods
        protected override bool Equals(IVariant other)
        {
            FFloat castedOther = other as FFloat;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FFloat)}");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FFloat castedOther = other as FFloat;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FFloat)}");

            return _value.CompareTo(castedOther._value);
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FFloat(float f) => new FFloat(f);

        public static implicit operator float(FFloat f) => f._value;

        public static bool operator ==(FFloat lhs, FFloat rhs) => Equals(rhs, lhs);

        public static bool operator !=(FFloat lhs, FFloat rhs) => !Equals(rhs, lhs);

        public static bool Equals(FFloat lhs, FFloat rhs)
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