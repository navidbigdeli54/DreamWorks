/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [FName("Bool")]
    [Serializable]
    public sealed class FBool : FVariant<bool>
    {
        #region Constructors
        public FBool(bool value = false) : base(value) { }

        public FBool(bool isReadonly, bool value) : base(isReadonly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FBool castedOther = other as FBool;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FBool)}");

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
            FBool castedOther = other as FBool;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FBool)}");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FBool castedOther = other as FBool;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FBool)}");

            if (this._value == castedOther._value)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FBool(bool b) => new FBool(b);

        public static implicit operator bool(FBool b) => b._value;

        public static bool operator ==(FBool lhs, FBool rhs)
        {
            return Equals(rhs, lhs);
        }

        public static bool operator !=(FBool lhs, FBool rhs)
        {
            return !Equals(rhs, lhs);
        }

        public static bool Equals(FBool lhs, FBool rhs)
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