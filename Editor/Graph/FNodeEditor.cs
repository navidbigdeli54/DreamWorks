/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using UnityEditor;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public class FNodeEditor
    {
        #region Fields
        protected SerializedObject _serializedObject;
        private readonly string[] _serializedPropertyFilter = new string[] { "m_Script", "graph", "position", "ports" };
        #endregion
    }
}