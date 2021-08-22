/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public interface IGraphEditor
    {
        #region Properties
        Texture2D GridTexture { get; }

        Texture2D CrossTexture { get; }
        #endregion

        #region Methods
        void OnGUI();
        #endregion
    }
}