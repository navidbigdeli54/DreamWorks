/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Text;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonObject : FJsonNode
    {
        #region Fields
        private readonly Dictionary<string, FJsonNode> _dictionary = new Dictionary<string, FJsonNode>();
        #endregion

        #region Indexer
        public FJsonNode this[string key]
        {
            get => Get(key);
            set => Add(key, value);
        }
        #endregion

        #region Properties
        public int Count => _dictionary.Count;
        #endregion

        #region Public Methods
        public FJsonNode Get(string key)
        {
            if (_dictionary.TryGetValue(key, out FJsonNode node))
            {
                return node;
            }

            return FJsonNull.NULL;
        }

        public void Add(string key, FJsonNode node)
        {
            if (node == null)
            {
                node = FJsonNull.NULL;
            }

            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString();
            }

            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = node;
            }
            else
            {
                _dictionary.Add(key, node);
            }
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public override string ToString()
        {
            return ToString();
        }

        public override string ToString(int intendLevel = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();

            bool isFirst = true;

            stringBuilder.Append('{');

            foreach (var pair in _dictionary)
            {
                if (isFirst == false)
                {
                    stringBuilder.Append(',');
                }

                isFirst = false;

                stringBuilder.AppendLine().Append(' ', intendLevel + INTEND_SPACE_COUNT);

                stringBuilder.Append("\"").Append(Escape(pair.Key)).Append("\"");

                stringBuilder.Append(" : ");

                stringBuilder.Append(pair.Value.ToString(intendLevel + INTEND_SPACE_COUNT));
            }

            stringBuilder.AppendLine().Append(' ', intendLevel).Append("}");

            return stringBuilder.ToString();
        }
        #endregion
    }
}