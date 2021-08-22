/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [Serializable]
    [FNameAttribute("Vector2")]
    public sealed class FVector2 : FVariant<Vector2>
    {
        #region Constructors
        public FVector2(Vector2 value) : base(value) { }

        public FVector2(bool isReadonly, Vector2 value) : base(isReadonly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FVector2 castedOther = other as FVector2;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FVector2)}");

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
            FVector2 castedOther = other as FVector2;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FVector2)}.");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FVector2 castedOther = other as FVector2;
            FAssert.IsNotNull(castedOther, $"Other is type of {nameof(FVector2)}.");

            float sqrMagnitude = _value.sqrMagnitude;
            float otherSqrMagnitude = castedOther._value.sqrMagnitude;
            if (sqrMagnitude > otherSqrMagnitude)
            {
                return 1;
            }
            else if (sqrMagnitude < otherSqrMagnitude)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FVector2(Vector2 vector) => new FVector2(vector);

        public static implicit operator Vector2(FVector2 vector) => vector._value;

        public static bool operator ==(FVector2 lhs, FVector2 rhs) => Equals(rhs, lhs);

        public static bool operator !=(FVector2 lhs, FVector2 rhs) => !Equals(rhs, lhs);

        public static bool Equals(FVector2 lhs, FVector2 rhs)
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