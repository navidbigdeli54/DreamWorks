using UnityEngine;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.UI
{
	[RequireComponent(typeof(Canvas))]
	public class CUIScreen : CUIBase
	{
		#region Properties
		protected Canvas Canvas { get; private set; }
		#endregion

		#region CComponent Methods
		protected override async Task PreInitializeComponenetAsync()
		{
			await base.PreInitializeComponenetAsync();

			Canvas = GetComponent<Canvas>();
		}
		#endregion
	}
}