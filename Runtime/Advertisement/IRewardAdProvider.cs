using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Advertisement
{
	public interface IRewardAdProvider
	{
		Task<EAdResult> DisplayAd();
	}
}