using DreamMachineGameStudio.DreamWorks.World;

using UnityEngine;
using DreamMachineGameStudio.DreamWorks.World.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.GameFramework
{
    public interface IGameFrameworkComponent : IGameFrameworkInitializable, IGameFrameworkTickable
    {
        #region Properties
        public IGameWorld GameWorld { get; }

        public GameObject GameObject { get; }
        #endregion

        #region Public Methods
        internal void SetGameWorld(IGameWorld gameWorld);
        #endregion
    }
}