namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction
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