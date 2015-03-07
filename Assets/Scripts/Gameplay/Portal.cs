using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	[SerializeField] GameObject portalEndObject;
	[SerializeField] float radius = 1f;
	[SerializeField] float power = 1f;
	[SerializeField] GameObject[] portalActiveEffects;
	Vector3 direction;
	bool effectsActive = false;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
	}

	void FixedUpdate()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		
		foreach (Collider objectInRange in hitColliders)
		{
			if (objectInRange.tag == "Object")
			{
				direction = objectInRange.transform.position - transform.position;
				objectInRange.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized * -power, transform.position);

				if (!effectsActive)
					effectsActive = true;
			}
			else
				effectsActive = false;
		}

		if (effectsActive)
		{
			foreach (GameObject activeEffect in portalActiveEffects)
			{
				activeEffect.GetComponent<ParticleSystem>().enableEmission = true;
			}
		}
		else
		{
			foreach (GameObject activeEffect in portalActiveEffects)
			{
				activeEffect.GetComponent<ParticleSystem>().enableEmission = false;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (portalEndObject != null)
		{
			Fabric.EventManager.Instance.PostEvent("SFX_Wormhole", Fabric.EventAction.PlaySound, audioListener);
			other.transform.position = portalEndObject.transform.position;
		}
		else
			Debug.LogError("No End Portal Specified");
	}
}
