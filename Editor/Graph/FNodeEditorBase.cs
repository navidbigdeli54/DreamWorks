/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Collections.Generic;
using DreamMachineGameStudio.Dreamworks.Utility;

namespace DreamMachineGameStudio.Dreamworks.Graph.Editor
{
    public class FNodeEditorBase<NodeEditorType, AttributeType, NodeType> where NodeEditorType : FNodeEditorBase<NodeEditorType, AttributeType, NodeType> where AttributeType : FCustomeGraphEditorAttribute where NodeType : SGraph
    {
        #region Fields
        private static readonly Dictionary<Type, Type> _editorTypes = new Dictionary<Type, Type>();

        private static Dictionary<NodeType, NodeEditorType> _editors = new Dictionary<NodeType, NodeEditorType>();
        #endregion

        #region Properties
        public NodeType Target { get; private set; }

        public UGraphEditorWindow Window { get; private set; }
        #endregion

        #region Public Methods
        public static NodeEditorType GetEditor(NodeType target, UGraphEditorWindow window)
        {
            if (target == null) return null;

            if (_editors.TryGetValue(target, out NodeEditorType editor) == false)
            {
                Type graphType = target.GetType();
                Type editorType = GetEditorType(graphType);
                editor = Activator.CreateInstance(editorType) as NodeEditorType;
                editor.Window = window;
                editor.Target = target;
                editor.OnCreate();

                _editors.Add(target, editor);
            }

            if (editor.Target == null) editor.Target = target;
            if (editor.Window != window) editor.Window = window;

            return editor;
        }
        #endregion

        #region Protected Methods
        protected virtual void OnCreate() { }
        #endregion

        #region Private Methods
        private static Type GetEditorType(Type type)
        {
            if (type == null) return null;

            if (_editorTypes.Count == 0)
            {
                CacheCustomGraphEditors();
            }

            if (_editorTypes.TryGetValue(type, out Type result))
            {
                return result;
            }

            return GetEditorType(type.BaseType);
        }

        private static void CacheCustomGraphEditors()
        {
            IEnumerable<Type> customEditors = FReflectionUtility.GetSubTypesOf<NodeEditorType>();

            foreach (Type editor in customEditors)
            {
                if (editor.IsAbstract) continue;

                object[] attributes = editor.GetCustomAttributes(typeof(AttributeType), false);

                if (attributes == null || attributes.Length == 0) continue;

                foreach (object attribute in attributes)
                {
                    if (attribute is AttributeType graphEditorAttribute)
                    {
                        _editorTypes.Add(graphEditorAttribute.InspectedType, editor);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}