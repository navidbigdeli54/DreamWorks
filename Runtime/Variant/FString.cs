/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [Serializable]
    [FNameAttribute("String")]
    public sealed class FString : FVariant<string>
    {
        #region Constructors
        public FString(string value) : base(value) { }

        public FString(bool isReadonly, string value) : base(isReadonly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FString castedOther = other as FString;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FString)}");

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
            FString castedOther = other as FString;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FString)}");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FString castedOther = other as FString;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FString)}");

            return _value.CompareTo(castedOther._value);
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FString(string str) => new FString(str);

        public static implicit operator string(FString str) => str._value;

        public static bool operator ==(FString lhs, FString rhs) => Equals(rhs, lhs);

        public static bool operator !=(FString lhs, FString rhs) => !Equals(rhs, lhs);

        public static bool Equals(FString lhs, FString rhs)
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
                return rhs._value.Equals(rhs);
            }
        }
        #endregion
    }
}