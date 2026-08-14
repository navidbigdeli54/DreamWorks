using System.Threading.Tasks;
using DreamMachineGameStudio.DreamWorks.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.Modules.ObjectPool
{
    public class CObjectPoolSubSystem : CGameFrameworkComponent
    {
        #region Protected Methods
        protected override async Task PreInitializeAsync()
        {
            await base.PreInitializeAsync();

            DontDestroyOnLoad(this);
        }
        #endregion
    }
}
