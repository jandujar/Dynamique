using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] GameObject pauseButton;
	[SerializeField] GameObject resumeButton;

	public enum GameState
	{
		Play,
		Pause,
	}
	
	public GameState state;
	
	public void SetState()
	{
		switch(state)
		{
		case GameState.Play:
			Play();
			break;
		case GameState.Pause:
			Pause();
			break; 
		}
	}

	void Awake()
	{
		SetState();
	}
	
	void Play()
	{
		pauseButton.SetActive(true);
		resumeButton.SetActive(false);
	}
	
	void Pause()
	{
		pauseButton.SetActive(false);
		resumeButton.SetActive(true);
	}
}
