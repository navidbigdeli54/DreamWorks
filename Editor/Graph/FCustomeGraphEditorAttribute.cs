/**Copyright 2016 - 2022, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FCustomeGraphEditorAttribute : Attribute
    {
        #region Properties
        public Type InspectedType { get; }
        #endregion

        #region Constructors
        public FCustomeGraphEditorAttribute(Type inspectedType)
        {
            InspectedType = inspectedType;
        }
        #endregion
    }
}