namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public struct FPoolableInstanceOwner
    {
        #region Properties
        public string Name { get; }
        #endregion

        #region Constructors
        public FPoolableInstanceOwner(string name)
        {
            Name = name;
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            return obj is FPoolableInstanceOwner owner && Name == owner.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(FPoolableInstanceOwner lfs, FPoolableInstanceOwner rhs)
        {
            return lfs.Name == rhs.Name;
        }

        public static bool operator !=(FPoolableInstanceOwner lhs, FPoolableInstanceOwner rhs)
        {
            return lhs.Name != rhs.Name;
        } 
        #endregion
    }
}