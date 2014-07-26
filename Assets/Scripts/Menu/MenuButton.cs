using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuStateManager menuStateManager;

	public enum ButtonState
	{
		Idle,
		Options,
		StageSelect,
		LevelSelect
	}
	
	public ButtonState buttonState;
	
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
	}

	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	public void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject == gameObject)
		{
			switch(buttonState)
			{
			case ButtonState.Idle:
				menuStateManager.menuState = MenuStateManager.MenuState.Idle;
				break;
			case ButtonState.Options:
				menuStateManager.menuState = MenuStateManager.MenuState.Options;
				break;
			case ButtonState.StageSelect:
				menuStateManager.menuState = MenuStateManager.MenuState.StageSelect;
				break;
			case ButtonState.LevelSelect:
				menuStateManager.menuState = MenuStateManager.MenuState.LevelSelect;
				break;
			}

			menuStateManager.SetState();
		}
	}
}
