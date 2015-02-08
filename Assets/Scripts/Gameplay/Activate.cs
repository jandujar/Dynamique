using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour
{
	[SerializeField] GameObject objectToActivate;
	GameController gameController;
	bool activated = false;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();
		else
			Debug.LogError("Unable to find Game Controller");
	}

	// Subscribe to events 
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
	}
	
	// Unsubscribe
	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	// Unsubscribe
	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	// At the touch beginning
	public void On_TouchStart(Gesture gesture)
	{
		if (!gameController.GamePaused && !gameController.LevelComplete)
		{
			// Verification that the action is on the object 
			if (!activated && gesture.pickedObject == gameObject && objectToActivate != null)
				activated = true;
			else if (activated && gesture.pickedObject == gameObject && objectToActivate != null)
				activated = false;
		}
	}

	void ResetTools()
	{
		activated = false;
	}

	void Update()
	{
		if (activated && !objectToActivate.activeSelf)
			objectToActivate.SetActive(true);
		else if (!activated && objectToActivate.activeSelf)
		{
			objectToActivate.SetActive(false);
			Fabric.EventManager.Instance.PostEvent("SFX_Deactivate", Fabric.EventAction.PlaySound);
		}
	}
}
