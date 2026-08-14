using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace DreamMachineGameStudio.DreamWorks.Editor
{
    public sealed class FTypeDropdown : AdvancedDropdown
    {
        #region Fields
        private readonly IReadOnlyList<Type> Types;
        private readonly Action<Type> OnSelected;
        private readonly Func<Type, string> DisplayNameProvider;
        private readonly string RootName;
        #endregion

        #region Constructor
        public FTypeDropdown(AdvancedDropdownState state, IReadOnlyList<Type> types, Action<Type> onSelected, Func<Type, string> displayNameProvider = null, string rootName = "Types") : base(state)
        {
            Types = types;
            OnSelected = onSelected;
            DisplayNameProvider = displayNameProvider ?? (type => type.Name);
            RootName = rootName;

            minimumSize = new Vector2(350, 400);
        }
        #endregion

        #region AdvancedDropdown
        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new(RootName);

            foreach (Type type in Types)
            {
                root.AddChild(new FTypeDropdownItem(type, DisplayNameProvider(type)));
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is FTypeDropdownItem typeItem)
            {
                OnSelected?.Invoke(typeItem.Type);
            }
        }
        #endregion

        #region Nested Types
        private sealed class FTypeDropdownItem : AdvancedDropdownItem
        {
            public Type Type { get; }

            public FTypeDropdownItem(Type type, string displayName) : base(displayName)
            {
                Type = type;
            }
        }
        #endregion
    }
}
