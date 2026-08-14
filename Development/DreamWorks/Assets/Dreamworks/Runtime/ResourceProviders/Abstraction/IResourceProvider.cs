using System.Threading.Tasks;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction
{
    public interface IResourceProvider
    {
        TObject LoadResource<TObject>(IResourceKey key) where TObject : UnityEngine.Object;

        Task<TObject> LoadResourceAsync<TObject>(IResourceKey key) where TObject : UnityEngine.Object;
    }
}
