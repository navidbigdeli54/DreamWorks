using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider.AddressableAssets
{
    public class FAddressablesResourceProvider : IResourceProvider
    {
        #region IResourceProvider Implementation
        TObject IResourceProvider.LoadResource<TObject>(IResourceKey key)
        {
            throw new System.NotImplementedException();
        }

        Task<TObject> IResourceProvider.LoadResourceAsync<TObject>(IResourceKey key)
        {
            FAddressablesResourceKey addressablesResourceKey = key as FAddressablesResourceKey;

            return Addressables.LoadAssetAsync<TObject>(addressablesResourceKey.ResourceLocation).Task;
        }
        #endregion
    }
}