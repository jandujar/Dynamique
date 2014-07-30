using UnityEngine;
using System.Collections;

public class MenuAsteroidSpawner : MonoBehaviour
{
	[SerializeField] GameObject[] astroidPrefabs;
	[SerializeField] float initialForce = 1f;
	[SerializeField] Transform[] astroidSpawnPositions;
	int previousSpawnLocation;
	int previousSpawnAstroid;

	void Start()
	{
		InvokeRepeating("SpawnAstroid", 2f, Random.Range(20f, 30f));
	}

	void SpawnAstroid()
	{
		int randomInt = Random.Range(0, astroidSpawnPositions.Length);
		int randomAstroid = Random.Range(0, astroidPrefabs.Length);

		if (randomInt != previousSpawnLocation && randomAstroid != previousSpawnAstroid)
		{
			previousSpawnLocation = randomInt;
			previousSpawnAstroid = randomAstroid;
			Transform spawnTransform = astroidSpawnPositions[randomInt];
			GameObject spawnAstroid = astroidPrefabs[randomAstroid];

			GameObject cameraObject = GameObject.FindGameObjectWithTag("BackgroundCamera");
			Camera camera = cameraObject.GetComponent<Camera>();
			Vector3 modifiedPosition = new Vector3(spawnTransform.position.x * (camera.pixelWidth/camera.pixelHeight), spawnTransform.position.y, spawnTransform.position.z);

			GameObject clone = (GameObject)Instantiate(spawnAstroid, modifiedPosition, spawnTransform.rotation);
			clone.rigidbody.AddRelativeForce(Vector3.forward * Random.Range(initialForce * 0.9f, initialForce * 1.1f), ForceMode.Impulse);
			Destroy(clone, 90f);
		}
		else
			SpawnAstroid();
	}
}
