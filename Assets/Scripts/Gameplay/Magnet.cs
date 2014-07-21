using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
	[SerializeField] float radius = 1f;
	[SerializeField] float power = 1f;
	Vector3 direction;

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
