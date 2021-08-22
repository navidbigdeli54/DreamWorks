/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Core
{
    public class FName : IEquatable<FName>, IEqualityComparer<FName>
    {
        #region Fields
        private static readonly Dictionary<string, object> _cachedNames = new Dictionary<string, object>(1000);

        private readonly object _value;
        #endregion

        #region Constructors
        public FName(string name)
        {
            if (_cachedNames.TryGetValue(name, out object cachedName))
            {
                _value = cachedName;
            }
            else
            {
                _cachedNames.Add(name, name);
                _value = name;
            }
        }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return (string)_value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((FName)obj)._value == _value;
        }
        #endregion

        #region Operator Overloading
        public static implicit operator FName(string name) => new FName(name);

        public static implicit operator string(FName name) => (string)name._value;

        public static bool operator==(FName rhs, FName lhs)
        {
            return rhs._value == lhs._value;
        }

        public static bool operator!=(FName rhs, FName lhs)
        {
            return rhs._value != lhs._value;
        }
        #endregion

        #region IEquatable Implementation
        bool IEquatable<FName>.Equals(FName other) => _value == other._value;
        #endregion

        #region IEqualityComparer Implementation
        int IEqualityComparer<FName>.GetHashCode(FName name) => name._value.GetHashCode();

        bool IEqualityComparer<FName>.Equals(FName lhs, FName rhs) => lhs._value == rhs._value;
        #endregion
    }
}