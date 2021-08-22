/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Graph
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FOutputAttribute : FNameAttribute
    {
        #region Constructors
        public FOutputAttribute(string name) : base(name) { }
        #endregion
    }
}