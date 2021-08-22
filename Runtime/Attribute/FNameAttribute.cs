/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using System.Diagnostics;

namespace DreamMachineGameStudio.Dreamworks
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.All)]
    public class FNameAttribute : Attribute
    {
        #region Fields
        public readonly static Type CLASS_TYPE = typeof(FNameAttribute);
        #endregion

        #region Properties
        public string Name { get; }
        #endregion

        #region Constructors
        public FNameAttribute(string name)
        {
            Name = name;
        }
        #endregion
    }
}