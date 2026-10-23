namespace DreamMachineGameStudio.DreamWorks.GameFramework
{
    public enum EGameFrameworkLifecycleState : byte
    {
        None = 0,

        PreInitialized = 1,

        Initialized = 2,

        PostInitialized = 3,

        BegunPlay = 4,

        EndedPlay = 5,

        UnInitialized = 6
    }
}