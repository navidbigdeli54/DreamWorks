﻿/**Copyright 2016 - 2023, Dream Machine Game Studio. All Right Reserved.*/

using System.Text;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Serialization.Json
{
    public class FJsonArray : FJsonNode
    {
        #region Fields
        private readonly List<FJsonNode> _list = new List<FJsonNode>();
        #endregion

        #region Properties
        public int Count => _list.Count;

        public FJsonNode this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
        #endregion

        #region Public Methods
        public void Add(FJsonNode node)
        {
            _list.Add(node);
        }

        public override string ToString() => ToString();

        public override string ToString(int intentLevel = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            for (int i = 0; i < _list.Count; ++i)
            {
                if (i > 0)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.AppendLine().Append(' ', intentLevel + INTEND_SPACE_COUNT);

                stringBuilder.Append(_list[i].ToString(intentLevel + INTEND_SPACE_COUNT));
            }

            stringBuilder.AppendLine();

            for(int i = 0; i < intentLevel; ++i)
            {
                stringBuilder.Append(" ");
            }

            stringBuilder.Append("]"); 

            return stringBuilder.ToString();
        }
        #endregion
    }
}