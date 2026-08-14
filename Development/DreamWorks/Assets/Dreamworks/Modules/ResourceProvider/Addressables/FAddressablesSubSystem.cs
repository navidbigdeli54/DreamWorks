using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using DreamMachineGameStudio.DreamWorks.Core.Abstraction;
using DreamMachineGameStudio.DreamWorks.Core.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.Core.SubSystems.Attributes;

namespace DreamMachineGameStudio.DreamWorks.ResourceProvider.AddressableAssets
{
    [ADreamWorksSubSystem(
        displayName: "Addressables",
        description: "Resource loading system using Unity Addressables.",
        category: "Resources",
        order: 10,
        Experimental = false,
        Advanced = false,
        Keywords = "addressables async load asset bundle resource")]
    public class FAddressablesSubSystem : FGameInstanceSubSystem
    {
        #region Constructors
        public FAddressablesSubSystem(IGameInstance gameInstance)
            : base(gameInstance)
        {
        }
        #endregion

        #region Protected Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await Addressables.InitializeAsync().Task;
        }
        #endregion
    }
}