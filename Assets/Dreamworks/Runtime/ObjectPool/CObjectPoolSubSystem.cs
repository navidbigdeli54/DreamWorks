using DreamMachineGameStudio.DreamWorks.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.ObjectPool
{
    public class CObjectPoolSubSystem : CGameFrameworkComponent
    {
        #region Protected Methods
        protected override void PreInitialize()
        {
            base.PreInitialize();

            DontDestroyOnLoad(this);
        }
        #endregion
    }
}
