﻿using UnityEngine;
using System.Collections;

public class SummaryScreenButton : MonoBehaviour
{
	[SerializeField] GameStateManager gameStateManager;
	[SerializeField] TweenAlpha summaryScreen;

	public enum ButtonType
	{
		Continue,
		Replay,
		LevelSelect
	}
	
	public ButtonType buttonType;

	void OnClick()
	{
		summaryScreen.from = summaryScreen.value;
		summaryScreen.to = 0f;
		summaryScreen.duration = 1f;
		summaryScreen.delay = 0f;
		summaryScreen.ResetToBeginning();
		summaryScreen.PlayForward();

		StartCoroutine(WaitAndTrigger());
	}

	IEnumerator WaitAndTrigger()
	{
		yield return new WaitForSeconds(1.0f);

		switch(buttonType)
		{
		case ButtonType.Continue:
			gameStateManager.state = GameStateManager.GameState.Continue;
			break;
		case ButtonType.Replay:
			gameStateManager.state = GameStateManager.GameState.Replay;
			break;
		case ButtonType.LevelSelect:
			gameStateManager.state = GameStateManager.GameState.LevelSelect;
			break;
		}
		
		gameStateManager.SetState();
	}
}
