using System;

namespace DreamMachineGameStudio.DreamWorks.Editor.SubSystems
{
    public sealed class FSubSystemMetadata
    {
        #region Properties
        public Type Type { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public string Category { get; }

        public bool Experimental { get; }

        public bool Advanced { get; }

        public int Order { get; }

        public string Keywords { get; }
        #endregion

        #region Constructors
        public FSubSystemMetadata(Type type, string displayName, string description, string category, bool experimental, bool advanced, int order, string keywords)
        {
            Type = type;
            DisplayName = displayName;
            Description = description;
            Category = category;
            Experimental = experimental;
            Advanced = advanced;
            Order = order;
            Keywords = keywords;
        }
        #endregion
    }
}