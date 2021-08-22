/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEngine;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public class FGraphEditor : IGraphEditor
    {
        #region IGraphEditor Implementation
        Texture2D IGraphEditor.CrossTexture => FGraphEditorPrefrences.Setting.CrossTexture;

        Texture2D IGraphEditor.GridTexture => FGraphEditorPrefrences.Setting.GridTexture;

        void IGraphEditor.OnGUI() { }
        #endregion
    }
}