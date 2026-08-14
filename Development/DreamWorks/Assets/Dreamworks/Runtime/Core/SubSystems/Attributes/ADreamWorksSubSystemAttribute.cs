using System;

namespace DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes
{
    /// <summary>
    /// Provides editor metadata for a subsystem.
    /// This metadata is consumed by DreamWorks editor tools and
    /// has no effect on runtime behaviour.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ADreamWorksSubSystemAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Friendly name displayed in the editor.
        /// If null or empty, the class name is nicified.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Short description displayed underneath the subsystem header.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Logical editor category.
        /// Examples:
        /// Gameplay
        /// Developer
        /// Rendering
        /// Audio
        /// AI
        /// Networking
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Optional ordering within a category.
        /// Lower values appear first.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Whether the subsystem is considered experimental.
        /// </summary>
        public bool Experimental { get; set; }

        /// <summary>
        /// Whether the subsystem should only appear when
        /// advanced settings are enabled.
        /// </summary>
        public bool Advanced { get; set; }

        public string Keywords { get; set; }
        #endregion

        #region Constructors
        public ADreamWorksSubSystemAttribute(string displayName, string description, string category, int order = 0)
        {
            DisplayName = displayName;
            Description = description;
            Category = category;
            Order = order;
        }
        #endregion
    }
}