using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using DreamMachineGameStudio.DreamWorks.GameInstance;
using DreamMachineGameStudio.DreamWorks.GameInstance.SubSystems;
using DreamMachineGameStudio.DreamWorks.SubSystems.Attributes;
using DreamMachineGameStudio.DreamWorks.GameInstance.Abstraction;

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
        protected override void Initialize()
        {
            base.Initialize();

            Addressables.InitializeAsync().WaitForCompletion();
        }
        #endregion
    }
}