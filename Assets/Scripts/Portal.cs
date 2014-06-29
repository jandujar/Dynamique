using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	[SerializeField] GameObject portalEndObject;

	void OnTriggerEnter(Collider other)
	{
		if (portalEndObject != null)
			other.transform.position = portalEndObject.transform.position;
		else
			Debug.LogError("No End Portal Specified");
	}
}
