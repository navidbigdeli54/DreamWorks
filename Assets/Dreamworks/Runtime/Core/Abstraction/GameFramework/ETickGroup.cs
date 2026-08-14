namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public enum ETickGroup : byte
    {
        None,

        Input,

        PrePhysic,

        Physic,

        PostPhysic,

        Animation,

        Gameplay,

        PostTick,

        Max
    }
}