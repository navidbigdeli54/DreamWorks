using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction
{
    public interface IDreamWorksObject
    {
        #region Methods
        Task InitializeAsync();

        public void Tick(FFrameContext frameContext);

        Task ShutDownAsync();
        #endregion
    }
}