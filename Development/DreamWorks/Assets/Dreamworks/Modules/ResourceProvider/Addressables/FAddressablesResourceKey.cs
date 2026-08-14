using UnityEngine.ResourceManagement.ResourceLocations;
using DreamMachineGameStudio.DreamWorks.ResourceProvider.Abstraction;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider.AddressableAssets
{
    public class FAddressablesResourceKey : IResourceKey
    {
        #region Properties
        public IResourceLocation ResourceLocation { get; private set; }
        #endregion

        #region Constructors
        public FAddressablesResourceKey(IResourceLocation resourceLocation)
        {
            ResourceLocation = resourceLocation;
        }
        #endregion
    }
}