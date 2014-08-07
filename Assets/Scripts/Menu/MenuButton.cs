using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuStateManager menuStateManager;
	[SerializeField] TriggerLevelLoad triggerLevelLoad;

	public enum ButtonState
	{
		Idle,
		Options,
		StageSelect,
		GravitySelect,
		AntiGravitySelect,
		WormHoleSelect,
		ChaosTheorySelect
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
			ButtonPress();
	}

	void OnClick()
	{
		ButtonPress();
	}

	void ButtonPress()
	{
		if (triggerLevelLoad != null)
		{
			triggerLevelLoad.LoadLevel();
		}
		else
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
			case ButtonState.GravitySelect:
				menuStateManager.menuState = MenuStateManager.MenuState.GravitySelect;
				break;
			case ButtonState.AntiGravitySelect:
				menuStateManager.menuState = MenuStateManager.MenuState.AntiGravitySelect;
				break;
			case ButtonState.WormHoleSelect:
				menuStateManager.menuState = MenuStateManager.MenuState.WormHoleSelect;
				break;
			case ButtonState.ChaosTheorySelect:
				menuStateManager.menuState = MenuStateManager.MenuState.ChaosTheorySelect;
				break;
			}
			
			menuStateManager.SetState();
		}
	}
}
