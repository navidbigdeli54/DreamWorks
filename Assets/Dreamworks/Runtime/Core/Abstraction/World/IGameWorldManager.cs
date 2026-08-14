using System;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction
{
    public interface IGameWorldManager : IDreamWorksObject
    {
        event Action<IGameWorld> OnWorldInitialized;

        event Action<IGameWorld> OnWorldShutDown;

        IGameWorld GetFirstGameWorld();
    }
}