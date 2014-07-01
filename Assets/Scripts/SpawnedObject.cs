using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float maxVelocity = 1f;
	[SerializeField] float lifetime = 8f;
	[SerializeField] float respawnWait = 0.5f;
	[SerializeField] GameObject[] livingEffects;
	[SerializeField] GameObject deathEffect;
	GameController gameController;
	Spawner spawner;

	void Start()
	{
		var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
	
		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();

		var spawnerObject = GameObject.FindGameObjectWithTag("Spawner");

		if (spawnerObject != null)
			spawner = spawnerObject.GetComponent<Spawner>();

		transform.rigidbody.AddRelativeForce(Vector3.forward * initialForce, ForceMode.Impulse);
		StartCoroutine(SelfDestruct(lifetime));
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
		if (collision.transform.tag == "Finish")
		{
			gameController.LevelComplete = true;
			Death();
		}
		else if (collision.transform.tag != "Deflector")
		{
			spawner.TriggerSpawn(respawnWait);
			Death();
		}
	}

	IEnumerator SelfDestruct(float objectLifetime)
	{
		yield return new WaitForSeconds(objectLifetime);
		spawner.TriggerSpawn(respawnWait);
		Death();
	}

	void Death()
	{
		Instantiate(deathEffect, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
}
