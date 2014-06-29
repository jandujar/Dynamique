using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float maxVelocity = 1f;
	[SerializeField] float lifetime = 8f;

	void Start()
	{
		transform.rigidbody.AddRelativeForce(Vector3.forward * initialForce, ForceMode.Impulse);
		Destroy(transform.gameObject, lifetime);
	}

	void Update()
	{
		var velocity = rigidbody.velocity;
		if (velocity == Vector3.zero) return;
		
		var magnitude = velocity.magnitude;
		if (magnitude > maxVelocity)
		{
			velocity *= (maxVelocity / magnitude);
			rigidbody.velocity = velocity;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag != "Deflector")
			Destroy(transform.gameObject);
	}
}
