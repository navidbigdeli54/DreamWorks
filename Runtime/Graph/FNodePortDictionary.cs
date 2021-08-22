/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    [SerializeField]
    public sealed class FNodePortDictionary : Dictionary<string, FNodePort>, ISerializationCallbackReceiver
    {
        #region ISerializationCallbackReceiver Implementation
        [SerializeField]
        [HideInInspector]
        private List<string> _keys = new List<string>();

        [SerializeField]
        [HideInInspector]
        private List<FNodePort> _values = new List<FNodePort>();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (KeyValuePair<string, FNodePort> item in this)
            {
                _keys.Add(item.Key);
                _values.Add(item.Value);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            FAssert.AreEqual(_keys.Count, _values.Count, $"There are {_keys.Count} keys and {_values.Count} values after deserialization. Make sure all of your types are serializable.");

            Clear();

            for (int i = 0; i < _keys.Count; i++)
            {
                Add(_keys[i], _values[i]);
            }
        }
        #endregion
    }
}