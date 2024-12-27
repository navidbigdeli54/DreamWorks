using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.Advertisement
{
	public interface IInterstitialAdProvider
	{
		Task<EAdResult> DisplayAd();
	}
}