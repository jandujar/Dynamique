using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float maxVelocity = 1f;
	[SerializeField] float lifetime = 8f;
	[SerializeField] float respawnWait = 0.75f;
	[SerializeField] float collectibleRadius = 1f;
	[SerializeField] float collectiblePower = 1f;
	[SerializeField] GameObject[] livingEffects;
	[SerializeField] GameObject[] trailRenderers;
	[SerializeField] GameObject deathEffect;
	GameController gameController;
	bool levelComplete = false;
	Vector3 direction;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
		var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
	
		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();

		transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * initialForce, ForceMode.Impulse);
		StartCoroutine(SelfDestruct(lifetime));
	}

	void Update()
	{
		var velocity = GetComponent<Rigidbody>().velocity;

		if (velocity == Vector3.zero)
			return;
		
		var magnitude = velocity.magnitude;

		if (magnitude > maxVelocity)
		{
			velocity *= (maxVelocity / magnitude);
			GetComponent<Rigidbody>().velocity = velocity;
		}
	}

	void FixedUpdate()
	{
		if (!gameController.Dead)
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectibleRadius);
			
			foreach (Collider objectInRange in hitColliders)
			{
				if (objectInRange.tag == "Collectible")
				{
					direction = objectInRange.transform.position - transform.position;
					objectInRange.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized * -collectiblePower, transform.position);
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Finish")
		{
			Death();
			gameController.ShootingStarsCollected++;

			if (gameController.ShootingStarsCollected >= gameController.ShootingStarCount)
			{
				var endPointObject = GameObject.FindGameObjectWithTag("Finish");
				EndPoint endPointScript = endPointObject.GetComponent<EndPoint>();
				endPointScript.StopEffects();

				levelComplete = true;
				gameController.LevelComplete = true;
			}
		}
		else if (collision.transform.tag == "Obstacle" || collision.transform.tag == "Object")
		{
			gameController.SpawnShootingStars(respawnWait);
			gameController.KillShootingStars();
		}
	}

	IEnumerator SelfDestruct(float objectLifetime)
	{
		yield return new WaitForSeconds(objectLifetime);

		if (!gameController.Dead && !levelComplete)
		{
			gameController.SpawnShootingStars(respawnWait);
			gameController.KillShootingStars();
		}
	}

	public void Death()
	{
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

		if (!levelComplete)
		{
			Fabric.EventManager.Instance.PostEvent("SFX_Death", Fabric.EventAction.PlaySound, audioListener);
			Instantiate(deathEffect, transform.position, transform.rotation);
		}

		Collider[] colliders = gameObject.GetComponents<Collider>();

		foreach(Collider objectCollider in colliders)
			objectCollider.enabled = false;

		foreach(GameObject livingEffect in livingEffects)
			livingEffect.gameObject.GetComponent<ParticleSystem>().enableEmission = false;

		foreach(GameObject trail in trailRenderers)
			trail.SetActive(false);

		Destroy(gameObject, 1f);
	}
}
