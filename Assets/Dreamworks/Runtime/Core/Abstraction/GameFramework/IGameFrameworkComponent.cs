using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.GameFramework
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