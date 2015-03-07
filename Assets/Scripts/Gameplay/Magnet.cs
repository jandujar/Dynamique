using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	[SerializeField] float radius = 1f;
	[SerializeField] float power = 1f;
	Vector3 direction;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
	}

	void OnEnable()
	{
		if (transform.parent.name == "Attract")
			Fabric.EventManager.Instance.PostEvent("SFX_Attract", Fabric.EventAction.PlaySound, audioListener);
		else if (transform.parent.name == "Repel")
			Fabric.EventManager.Instance.PostEvent("SFX_Repel", Fabric.EventAction.PlaySound, audioListener);
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
			}
		}
	}
}
