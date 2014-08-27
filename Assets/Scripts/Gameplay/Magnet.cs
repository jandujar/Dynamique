using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	[SerializeField] float radius = 1f;
	[SerializeField] float power = 1f;
	Vector3 direction;

	void OnEnable()
	{
		Debug.Log(transform.parent.name);

		if (transform.parent.name == "Attract")
			Fabric.EventManager.Instance.PostEvent("SFX_Attract", Fabric.EventAction.PlaySound);
		else if (transform.parent.name == "Repel")
			Fabric.EventManager.Instance.PostEvent("SFX_Repel", Fabric.EventAction.PlaySound);
	}

	void FixedUpdate()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

		foreach (Collider objectInRange in hitColliders)
		{
			if (objectInRange.tag == "Object")
			{
				direction = objectInRange.transform.position - transform.position;
				objectInRange.rigidbody.AddForceAtPosition(direction.normalized * -power, transform.position);
			}
		}
	}
}
