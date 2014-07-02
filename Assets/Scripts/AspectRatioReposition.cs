using UnityEngine;
using System.Collections;

public class AspectRatioReposition : MonoBehaviour
{
	[SerializeField] GameObject[] moveableObjects;

	void Awake()
	{
		float aspectMultiplier = camera.aspect;

		foreach (GameObject moveableObject in moveableObjects)
		{
			var modifiedPosition = moveableObject.transform.position;
			modifiedPosition.x += modifiedPosition.x * (aspectMultiplier - (1024f/768f));
			moveableObject.transform.position = modifiedPosition;
		}
	}
}
