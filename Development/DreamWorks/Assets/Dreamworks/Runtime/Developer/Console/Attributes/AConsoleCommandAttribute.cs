using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AConsoleCommandAttribute : Attribute
    {
        #region Properties
        public string Name { get; }

        public string Description { get; }
        #endregion

        #region Constructors
        public AConsoleCommandAttribute(string name, string description = "")
        {
            Name = name;
            Description = description;
        } 
        #endregion
    }
}