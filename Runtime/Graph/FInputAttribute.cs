/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FInputAttribute : FNameAttribute
    {
        #region Constructors
        public FInputAttribute(string name) : base(name) { }
        #endregion
    }
}