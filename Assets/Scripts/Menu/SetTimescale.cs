using UnityEngine;
using System.Collections;

public class SetTimescale : MonoBehaviour
{
	[SerializeField] GameStateManager gameStateManager;
	[SerializeField] float newTimescale = 1f;
	[SerializeField] float lerpTime = 0f;

	void OnClick()
	{
		if (newTimescale == 0)
		{
			CustomTimeManager.Pause(lerpTime);
			gameStateManager.state = GameStateManager.GameState.Pause;
			gameStateManager.SetState();
		}
		else if (newTimescale == 1)
		{
			CustomTimeManager.Play(lerpTime);
		}
		else
			CustomTimeManager.FadeTo(lerpTime, newTimescale);
	}
}
