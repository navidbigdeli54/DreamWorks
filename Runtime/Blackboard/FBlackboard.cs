/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;
using DreamMachineGameStudio.Dreamworks.Variant;

namespace DreamMachineGameStudio.Dreamworks.Blackboard
{
    public sealed class FBlackboard
    {
        #region Fields
        private readonly Dictionary<FStringId, IVariant> _values = new Dictionary<FStringId, IVariant>();
        #endregion

        #region Methods
        public void AddValue<T>(FStringId key, T value) where T : class, IVariant
        {
            FAssert.IsFalse(string.IsNullOrWhiteSpace(key), $"Name can't be null or empty.");
            FAssert.IsFalse(_values.ContainsKey(key), $"A value with {key} key is already exist in blackboard.");

            _values.Add(key, value);
        }

        public T GetValue<T>(FStringId key) where T : class, IVariant
        {
            FAssert.IsTrue(_values.ContainsKey(key), $"{key} key does not exist in the blackboard");

            return (T)_values[key];
        }

        public bool TryGetValue<T>(FStringId key, out T value) where T : class, IVariant
        {
            bool isSuccessful = _values.TryGetValue(key, out IVariant result);

            value = result as T;

            return isSuccessful;
        }
        #endregion
    }
}