using UnityEngine;
using System.Collections;

public class Activate : MonoBehaviour
{
	[SerializeField] GameObject objectToActivate;
	bool activated = false;

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
		// Verification that the action is on the object 
		if (!activated && gesture.pickObject == gameObject && objectToActivate != null)
			activated = true;
		else if (activated && gesture.pickObject == gameObject && objectToActivate != null)
			activated = false;
	}

	void ResetTools()
	{
		activated = false;
	}

	void Update()
	{
		if (activated && !objectToActivate.activeSelf)
		{
			objectToActivate.SetActive(true);
			gameObject.renderer.material.color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
		}
		else if (!activated && objectToActivate.activeSelf)
		{
			objectToActivate.SetActive(false);
			gameObject.renderer.material.color = Color.white;
		}
	}
}
