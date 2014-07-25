using UnityEngine;
using System.Collections;

public class MenuStateManager : MonoBehaviour
{
	public enum MenuState
	{
		Idle,
		Unfold,
		LevelSelect,
		Options
	}
	
	public MenuState menuState;
	
	public void SetState()
	{
		switch(menuState)
		{
		case MenuState.Idle:
			Idle();
			break;
		case MenuState.Unfold:
			Unfold();
			break;
		case MenuState.LevelSelect:
			LevelSelect();
			break;
		case MenuState.Options:
			Options();
			break;
		}
	}

	void Idle()
	{
		Debug.Log("Idle");
	}

	void Unfold()
	{
		Debug.Log("Unfold");
	}

	void LevelSelect()
	{
		Debug.Log("Level Select");
	}

	void Options()
	{
		Debug.Log("Options");
	}
}
