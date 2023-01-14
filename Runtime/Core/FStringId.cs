/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public class FStringId : IEquatable<FStringId>, IEqualityComparer<FStringId>
    {
        #region Fields
        private readonly int _id;

        private readonly string _value;
        #endregion

        #region Constructors
        public FStringId(string str)
        {
            _value = str;

            _id = str.GetHashCode();
        }

        public static implicit operator FStringId(string str) => new FStringId(str);
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            FAssert.IsTrue(obj is FStringId, $"Given value is not an FStringId type.");

            if ((object)this == obj) return true;

            return _id == ((FStringId)obj)._id;
        }

        public override string ToString() => _value;

        public override int GetHashCode() => _id;
        #endregion

        #region Static Methods
        public static bool Equals(FStringId lhs, FStringId rhs)
        {
            if ((object)lhs == rhs) return true;

            if (lhs is null || rhs is null) return false;

            return lhs._id == rhs._id;
        }
        #endregion

        #region Operator Overloads
        public static bool operator ==(FStringId lhs, FStringId rhs) => Equals(lhs, rhs);

        public static bool operator !=(FStringId lhs, FStringId rhs) => !Equals(lhs, rhs);
        #endregion

        #region IEquatable<FStringId> Implementation
        bool IEquatable<FStringId>.Equals(FStringId other) => _id == other._id;
        #endregion

        #region IEqualityComparer<FStringId> Implementation
        bool IEqualityComparer<FStringId>.Equals(FStringId lhs, FStringId rhs) => Equals(lhs, rhs);

        int IEqualityComparer<FStringId>.GetHashCode(FStringId stringId) => stringId.GetHashCode();
        #endregion
    }
}