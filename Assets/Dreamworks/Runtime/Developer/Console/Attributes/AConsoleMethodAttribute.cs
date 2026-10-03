using System;

namespace DreamMachineGameStudio.DreamWorks.Developer.Console.Attributes
{
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