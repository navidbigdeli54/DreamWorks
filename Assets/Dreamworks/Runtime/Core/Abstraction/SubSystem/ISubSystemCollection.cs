namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystemCollection<T> where T : class, ISubSystem
    {
        #region Methods
        void Initialize();

        void Tick(FFrameContext context);

        void ShutDown();

        TSystem GetSubSystem<TSystem>();

        void ClearSubSystems();
        #endregion
    }
}
