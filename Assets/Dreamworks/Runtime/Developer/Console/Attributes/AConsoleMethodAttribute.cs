using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes
{
    /// <summary>
    /// Specifies that a method is a console method and provides metadata for its usage.
    /// </summary>
    /// <remarks>This attribute is used to mark methods as console commands, allowing them to be identified
    /// and invoked in a console-based application. The <see cref="Name"/> property defines the command name, and the
    /// <see cref="Description"/> property provides a brief explanation of the command's purpose.</remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AConsoleMethodAttribute : Attribute
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }
        #endregion

        #region Constructors
        public AConsoleMethodAttribute(string name, string description = "")
        {
            Name = name;

            Description = description;
        }
        #endregion
    }
}