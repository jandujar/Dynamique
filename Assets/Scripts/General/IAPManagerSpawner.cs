using UnityEngine;
using System.Collections;

public class IAPManagerSpawner : MonoBehaviour
{
	[SerializeField] IAPManager iapManager;

	void Awake()
	{
		GameObject iapManagerObject = GameObject.FindGameObjectWithTag("IAPManager");

		if (iapManagerObject == null)
			Instantiate(iapManager, transform.position, transform.rotation);
	}
}
