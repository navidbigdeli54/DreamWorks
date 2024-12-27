using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.Advertisement
{
	public enum EAdResult : byte
	{
		None = 0,
		Completed = 1,
		Cancelled = 2,
	}

	public interface IAdProvider : IService
	{
		#region Properties
		public IInterstitialAdProvider InterstitialAdProvider { get; protected set; }

		public IRewardAdProvider RewardAdProvider {  get; protected set; }
		#endregion

		#region Public Methods
		Task Initialize();
		#endregion
	}
}