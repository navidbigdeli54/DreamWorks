using UnityEngine;
using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;
using DreamMachineGameStudio.Dreamworks.Debug;

namespace DreamMachineGameStudio.Dreamworks.Console
{
	public class CConsoleView : CComponent
	{
		#region Fields
		private const int CONSOLE_HEIGHT = 40;

		private bool _isActive;

		private string _inputText;

		private GUISkin _skin;
		#endregion

		#region CComponent Methods
		protected override async Task PreInitializeComponenetAsync()
		{
			await base.PreInitializeComponenetAsync();

			CanEverTick = true;

			_skin = Resources.Load<GUISkin>("console_skin");

			MakePersistent();
		}

		protected override void TickComponent(float deltaTime)
		{
			base.TickComponent(deltaTime);

			if (Input.GetKeyDown(KeyCode.BackQuote) || Input.touchCount > 2)
			{
				Activate();
			}
		}
		#endregion

		#region MonoBehaviour Methods
		private void OnGUI()
		{
			if (_isActive)
			{
				Event e = Event.current;
				if (e.type == EventType.KeyDown)
				{
					if (e.keyCode == KeyCode.BackQuote)
					{
						Deactivate();

						return;
					}
					else if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
					{
						Submit();

						return;
					}
				}

				GUILayout.BeginVertical(GUILayout.Width(Screen.width), GUILayout.Height(CONSOLE_HEIGHT * Screen.dpi));
				GUILayout.Space(Screen.height - CONSOLE_HEIGHT);
				GUILayout.BeginHorizontal();
				GUI.SetNextControlName(nameof(_inputText));
				_inputText = GUILayout.TextField(_inputText, _skin.textField, GUILayout.Height(CONSOLE_HEIGHT));
				GUI.FocusControl(nameof(_inputText));
				if (GUILayout.Button("Enter", GUILayout.Height(CONSOLE_HEIGHT), GUILayout.Width(100)))
				{
					Submit();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
		}
		#endregion

		#region Private Methods
		private void Activate()
		{
			_isActive = true;
			_inputText = string.Empty;
		}

		private void Deactivate()
		{
			_isActive = false;
		}

		private void Submit()
		{
			Deactivate();

			FConsole.Instance.ExecuteCommand(_inputText);
		}
		#endregion
	}
}
