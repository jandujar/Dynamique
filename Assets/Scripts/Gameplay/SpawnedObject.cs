using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float maxVelocity = 1f;
	[SerializeField] float lifetime = 8f;
	[SerializeField] float respawnWait = 0.5f;
	[SerializeField] float collectibleRadius = 1f;
	[SerializeField] float collectiblePower = 1f;
	[SerializeField] GameObject[] livingEffects;
	[SerializeField] GameObject[] trailRenderers;
	[SerializeField] GameObject deathEffect;
	GameController gameController;
	Spawner spawner;
	GameObject tools;
	bool levelComplete = false;
	bool dead = false;
	Vector3 direction;

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

		if (velocity == Vector3.zero)
			return;
		
		var magnitude = velocity.magnitude;

		if (magnitude > maxVelocity)
		{
			velocity *= (maxVelocity / magnitude);
			rigidbody.velocity = velocity;
		}
	}

	void FixedUpdate()
	{
		if (!dead)
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectibleRadius);
			
			foreach (Collider objectInRange in hitColliders)
			{
				if (objectInRange.tag == "Collectible")
				{
					direction = objectInRange.transform.position - transform.position;
					objectInRange.rigidbody.AddForceAtPosition(direction.normalized * -collectiblePower, transform.position);
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Finish")
		{
			var endPointObject = GameObject.FindGameObjectWithTag("Finish");
			EndPoint endPointScript = endPointObject.GetComponent<EndPoint>();
			endPointScript.StopEffects();

			levelComplete = true;
			gameController.LevelComplete = true;
			Death();
		}
		else if (collision.transform.tag == "Obstacle")
		{
			spawner.TriggerSpawn(respawnWait);
			Death();
		}
	}

	IEnumerator SelfDestruct(float objectLifetime)
	{
		yield return new WaitForSeconds(objectLifetime);

		if (!dead && !levelComplete)
		{
			spawner.TriggerSpawn(respawnWait);
			Death();
		}
	}

	void Death()
	{
		dead = true;
		tools.BroadcastMessage("ResetTools");
		gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);

		if (!levelComplete)
		{
			Fabric.EventManager.Instance.PostEvent("SFX_Death", Fabric.EventAction.PlaySound);
			Instantiate(deathEffect, transform.position, transform.rotation);
			gameController.ResetCollectibles();
		}

		Collider[] colliders = gameObject.GetComponents<Collider>();

		foreach(Collider objectCollider in colliders)
			objectCollider.enabled = false;

		foreach(GameObject livingEffect in livingEffects)
			livingEffect.gameObject.particleSystem.enableEmission = false;

		foreach(GameObject trail in trailRenderers)
			trail.SetActive(false);

		Destroy(transform.gameObject, 2f);
	}
}
