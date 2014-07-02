using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float maxVelocity = 1f;
	[SerializeField] float lifetime = 8f;
	[SerializeField] float respawnWait = 0.5f;
	[SerializeField] GameObject[] livingEffects;
	[SerializeField] GameObject[] trailRenderers;
	[SerializeField] GameObject deathEffect;
	GameController gameController;
	Spawner spawner;
	GameObject tools;
	bool levelComplete = false;
	
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
	}

	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}

	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	
	public void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject == gameObject)
		{
			spawner.TriggerSpawn(respawnWait);
			Death();
		}
	}

	void Start()
	{
		tools = GameObject.FindGameObjectWithTag("Tools");
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
			gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);

			foreach(GameObject livingEffect in livingEffects)
				livingEffect.gameObject.particleSystem.enableEmission = false;
			foreach(GameObject trail in trailRenderers)
				trail.SetActive(false);

			levelComplete = true;
			gameController.LevelComplete = true;
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

		if (!levelComplete)
		{
			spawner.TriggerSpawn(respawnWait);
			Death();
		}
	}

	void Death()
	{
		tools.BroadcastMessage("ResetTools");
		Instantiate(deathEffect, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
}
