namespace DreamMachineGameStudio.DreamWorks.GameFramework
{
    public enum ETickGroup : byte
    {
        None = 0,

        Input = 1,

        PrePhysic = 2,

        Physic = 3,

        PostPhysic = 4,

        Animation = 5,

        Gameplay = 5,

        PostTick = 6,

        Max = 7
    }
}