/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    [Serializable]
    public abstract class FNode
    {
        #region Fields
        [SerializeField]
        private SGraph _graph;

        [SerializeField]
        private Vector2Int _position;

        [SerializeField]
        private FNodePortDictionary _ports = new FNodePortDictionary();
        #endregion

        #region Methods
        protected virtual void OnEnable() { }

        protected virtual void OnDestroy() { }
        #endregion
    }
}