using System;

namespace DreamMachineGameStudio.Dreamworks.Console
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class FCommandAttribute : Attribute
    {
        #region Fields
        public readonly string Name;
        #endregion

        #region Constructors
        public FCommandAttribute(string name)
        {
            Name = name;
        }
        #endregion
    }
}
