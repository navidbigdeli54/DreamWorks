using DreamMachineGameStudio.DreamWorks.Application;

namespace DreamMachineGameStudio.DreamWorks.Foundation.Abstraction
{
    public interface IDreamWorksObject
    {
        #region Methods
        void Initialize();

        public void Tick(FFrameContext frameContext);

        void ShutDown();
        #endregion
    }
}