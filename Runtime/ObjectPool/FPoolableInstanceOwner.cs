namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public struct FPoolableInstanceOwner
    {
        public string Name { get; }

        public FPoolableInstanceOwner(string name)
        {
            Name = name;
        }
    }
}