using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Advertisement.Dummy
{
	public class FDummyRewardAdProvider : IRewardAdProvider
	{
		#region IRewardAdProvider
		Task<EAdResult> IRewardAdProvider.DisplayAd()
		{
			return Task.FromResult(EAdResult.Completed);
		}
		#endregion
	}
}