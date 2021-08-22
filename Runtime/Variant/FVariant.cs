/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    [Serializable]
    public abstract class FVariant<T> : IVariant
    {
        #region Fields
        [SerializeField]
        protected T _value;

        [SerializeField]
        protected readonly bool _isReadOnly;
        #endregion

        #region Constructors
        public FVariant() { }

        public FVariant(T value) => this._value = value;

        public FVariant(bool isReadOnly, T value)
        {
            this._value = value;
            this._isReadOnly = isReadOnly;
        }
        #endregion

        #region Properties
        public T Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (_isReadOnly)
                {
                    FLog.Error(nameof(FVariant<T>), $"Value is read-only but you trying to assign a new value.");
                    return;
                }

                this._value = value;
            }
        }
        #endregion

        #region Protected Methods
        protected abstract bool Equals(IVariant other);

        protected abstract int CompareTo(IVariant other);
        #endregion

        #region IValue Implementation
        bool IVariant.IsReadOnly => _isReadOnly;
        #endregion

        #region IEquatable Implementation
        bool IEquatable<IVariant>.Equals(IVariant other) => Equals(other);
        #endregion

        #region IComprable Implementation
        int IComparable<IVariant>.CompareTo(IVariant other) => CompareTo(other);
        #endregion
    }
}