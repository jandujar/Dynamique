using UnityEngine;
using System.Collections;

public class AsteroidPhysics : MonoBehaviour
{
	[SerializeField] float initialForce = 1f;
	[SerializeField] float minRandom = -0.5f;
	[SerializeField] float maxRandom = 0.5f;
	AsteroidFastSpawn mySpawnManager;
	FastSpawnObject thisSpawnObject;

	void Awake()
	{
		mySpawnManager = (AsteroidFastSpawn)FindObjectOfType(typeof(AsteroidFastSpawn));
		thisSpawnObject = GetComponent<FastSpawnObject>();
	}

	void OnEnable()
	{
		StartCoroutine(WaitAndDisable());
		rigidbody.velocity = new Vector3(0f, 0f, 0f);
		rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
		rigidbody.AddRelativeForce(Vector3.forward * Random.Range(initialForce * 0.8f, initialForce * 1.2f), ForceMode.Impulse);

		float xRot = Random.Range(minRandom, maxRandom);
		float yRot = Random.Range(minRandom, maxRandom);
		float zRot = Random.Range(minRandom, maxRandom);
		rigidbody.angularVelocity = new Vector3(xRot, yRot, zRot);
	}

	IEnumerator WaitAndDisable()
	{
		yield return new WaitForSeconds(45f);
		mySpawnManager.UnspawnObject(thisSpawnObject);
	}
}
