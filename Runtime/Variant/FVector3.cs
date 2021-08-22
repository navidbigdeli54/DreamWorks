/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [Serializable]
    [FNameAttribute("Vector3")]
    public sealed class FVector3 : FVariant<Vector3>
    {
        #region Constructors
        public FVector3(Vector3 value) : base(value) { }

        public FVector3(bool isReadonly, Vector3 value) : base(isReadonly, value) { }
        #endregion

        #region Public Methods
        public override bool Equals(object other)
        {
            FVector3 castedOther = other as FVector3;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FVector3)}");

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
            FVector3 castedOther = other as FVector3;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FVector3)}");

            return this._value == castedOther._value;
        }

        protected override int CompareTo(IVariant other)
        {
            FVector3 castedOther = other as FVector3;
            FAssert.IsNotNull(castedOther, $"Other is not type of {nameof(FVector3)}.");

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
        public static implicit operator FVector3(Vector3 vector) => new FVector3(vector);

        public static implicit operator Vector3(FVector3 vector) => vector._value;

        public static bool operator ==(FVector3 lhs, FVector3 rhs) => Equals(rhs, lhs);

        public static bool operator !=(FVector3 lhs, FVector3 rhs) => !Equals(rhs, lhs);

        public static bool Equals(FVector3 lhs, FVector3 rhs)
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