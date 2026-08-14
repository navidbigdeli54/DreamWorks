using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.Core.Abstraction.SubSystem
{
    public interface ISubSystemCollection<T> where T : class, ISubSystem
    {
        #region Methods
        Task InitializeAsync();

        Task ShutDownAsync();

        TSystem GetSubSystem<TSystem>();

        void ClearSubSystems();
        #endregion
    }
}
