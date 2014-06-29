using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour
{
	[SerializeField] GameObject objectToActivate;

	// Subscribe to events 
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchUp += On_TouchUp;
	}
	
	// Unsubscribe
	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}

	// Unsubscribe
	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}

	// At the touch beginning
	public void On_TouchStart(Gesture gesture)
	{
		// Verification that the action is on the object 
		if (gesture.pickObject == gameObject && objectToActivate != null)
		{
			objectToActivate.SetActive(true);
			gameObject.renderer.material.color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
		}
	}

	// At the touch end
	public void On_TouchUp(Gesture gesture)
	{
		// Verification that the action is on the object 
		if (gesture.pickObject == gameObject && objectToActivate != null)
		{
			objectToActivate.SetActive(false);
			gameObject.renderer.material.color = Color.white;
		}
	}
}
