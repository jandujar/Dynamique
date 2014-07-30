using UnityEngine;
using System.Collections;

public class PauseScreenButton : MonoBehaviour
{
	[SerializeField] GameStateManager gameStateManager;

	public enum ButtonType
	{
		Resume,
		Replay,
		MainMenu
	}
	
	public ButtonType buttonType;

	void OnClick()
	{
		switch(buttonType)
		{
		case ButtonType.Resume:
			gameStateManager.state = GameStateManager.GameState.Play;
			gameStateManager.SetState();
			break;
		case ButtonType.Replay:
			gameStateManager.SummaryFade.SetActive(true);
			gameStateManager.state = GameStateManager.GameState.Replay;
			gameStateManager.DelaySetState(2f);
			gameStateManager.ResumeObjects.SetActive(false);
			break;
		case ButtonType.MainMenu:
			gameStateManager.SummaryFade.SetActive(true);
			gameStateManager.state = GameStateManager.GameState.MainMenu;
			gameStateManager.DelaySetState(2f);
			gameStateManager.ResumeObjects.SetActive(false);
			break;
		}
	}
}
