/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    public abstract class SGraph : SScriptableObject
    {
        #region Fields
        [SerializeReference]
        private List<FNode> _nodes = new List<FNode>();
        #endregion
    }
}