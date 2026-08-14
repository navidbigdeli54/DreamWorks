namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
{
    public enum EGameFrameworkLifecycleState : byte
    {
        None,

        PreInitialized,

        Initialized,

        PostInitialized,

        BegunPlay,

        EndedPlay,

        UnInitialized
    }
}