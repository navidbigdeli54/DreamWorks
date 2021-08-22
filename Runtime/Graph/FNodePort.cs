/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using System.Collections.Generic;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    [Serializable]
    public class FNodePort
    {
        #region Fields
        [SerializeField]
        [HideInInspector]
        private FNode _node;

        [SerializeField]
        [HideInInspector]
        private List<FPortConnection> _connections = new List<FPortConnection>();
        #endregion
    }
}