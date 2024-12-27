using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Advertisement.Dummy
{
	public class FDummyInterstitialAdProvider : IInterstitialAdProvider
	{
		#region IInterstitialAdProvider Implementation
		Task<EAdResult> IInterstitialAdProvider.DisplayAd()
		{
			return Task.FromResult(EAdResult.Completed);
		} 
		#endregion
	}
}