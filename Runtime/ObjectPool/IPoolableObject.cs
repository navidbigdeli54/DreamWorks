namespace DreamMachineGameStudio.Dreamworks.ObjectPool
{
    public interface IPoolableObject
    {
        bool IsFree { get; }

        FPoolableInstanceOwner Owner { get; set; }

        void Acquired();

        void Released();
    }
}