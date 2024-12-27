using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Advertisement.Dummy
{
	public class FDummyAdProvider : IAdProvider
	{
		#region IAdProvider Implementation
		IInterstitialAdProvider IAdProvider.InterstitialAdProvider { get; set; } = new FDummyInterstitialAdProvider();

		IRewardAdProvider IAdProvider.RewardAdProvider { get; set; } = new FDummyRewardAdProvider();

		Task IAdProvider.Initialize()
		{
			return Task.CompletedTask;
		} 
		#endregion
	}
}