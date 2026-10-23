using System;
using DreamMachineGameStudio.DreamWorks.Application;
using DreamMachineGameStudio.DreamWorks.Foundation;
using DreamMachineGameStudio.DreamWorks.Foundation.Abstraction;


namespace DreamMachineGameStudio.DreamWorks.World.Abstraction
{
    public interface IGameWorldManager : IDreamWorksObject
    {
        event Action<IGameWorld> OnWorldInitialized;

        event Action<IGameWorld> OnWorldShutDown;

        IGameWorld GetFirstGameWorld();
    }
}