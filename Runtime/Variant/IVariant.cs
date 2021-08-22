/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;

namespace DreamMachineGameStudio.Dreamworks.Variant
{
    public interface IVariant : IEquatable<IVariant>, IComparable<IVariant>
    {
        #region Properties
        bool IsReadOnly { get; }
        #endregion
    }
}