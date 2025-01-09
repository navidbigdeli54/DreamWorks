using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DreamMachineGameStudio.Dreamworks.Audio;
using DreamMachineGameStudio.Dreamworks.ServiceLocator;

namespace DreamMachineGameStudio.Dreamworks.UI
{
	[RequireComponent(typeof(Button))]
	public class CButton : CUIWidget, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		#region Fields
		[SerializeField]
		protected AudioClip _clickSound;

		[SerializeField]
		protected AudioClip _hoverSound;

		[SerializeField]
		protected AudioClip _unhoverSound;
		#endregion

		#region Properties
		public Action OnClick { get; set; }

		public Action OnHover { get; set; }

		public Action OnUnhover { get; set; }
		#endregion

		#region Public Methods
		public void SetInteractible(bool isInteractible)
		{
			GetComponent<Button>().interactable = isInteractible;
		}
		#endregion

		#region Protected Methods
		protected virtual void OnButtonClick()
		{
		}

		protected virtual void OnButtonHover()
		{
		}

		protected virtual void OnButtonUnhover()
		{
		}
		#endregion

		#region IPointerClick Implementation
		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			IAudioController audioController = FServiceLocator.Get<IAudioController>();
			audioController?.PlayAudio(_clickSound);

			OnClick?.Invoke();

			OnButtonClick();
		}
		#endregion

		#region IPoinerEnterHandler Implementation
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			IAudioController audioController = FServiceLocator.Get<IAudioController>();
			audioController?.PlayAudio(_hoverSound);

			OnHover?.Invoke();

			OnButtonHover();
		}
		#endregion

		#region IPointerExitHandler Implementation
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			IAudioController audioController = FServiceLocator.Get<IAudioController>();
			audioController?.PlayAudio(_unhoverSound);

			OnUnhover?.Invoke();

			OnButtonUnhover();
		}
		#endregion
	}
}