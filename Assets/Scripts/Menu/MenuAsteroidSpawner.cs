using UnityEngine;
using System.Collections;

public class MenuAsteroidSpawner : MonoBehaviour
{
	[SerializeField] AsteroidFastSpawn mySpawnManager;
	[SerializeField] int astroidPrefabs = 4;
	[SerializeField] Transform[] astroidSpawnPositions;
	int previousSpawnLocation;
	int previousSpawnAstroid;
	FastSpawnObject clone1;
	FastSpawnObject clone2;
	FastSpawnObject clone3;
	FastSpawnObject clone4;

	void Start()
	{
		mySpawnManager.LoadObjects(mySpawnManager.asteroids);

		clone1 = mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid1, transform.position, transform.rotation);
		clone2 = mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid1, transform.position, transform.rotation);
		clone3 = mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid1, transform.position, transform.rotation);
		clone4 = mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid1, transform.position, transform.rotation);

		StartCoroutine(WaitAndUnload());
		InvokeRepeating("SpawnAstroid", 0f, Random.Range(10f, 20f));
	}

	void SpawnAstroid()
	{
		int randomInt = Random.Range(0, astroidSpawnPositions.Length);
		int randomAstroid = Random.Range(0, astroidPrefabs);

		if (randomInt != previousSpawnLocation && randomAstroid != previousSpawnAstroid)
		{
			previousSpawnLocation = randomInt;
			previousSpawnAstroid = randomAstroid;
			Transform spawnTransform = astroidSpawnPositions[randomInt];

			GameObject cameraObject = GameObject.FindGameObjectWithTag("BackgroundCamera");
			Camera camera = cameraObject.GetComponent<Camera>();
			Vector3 modifiedPosition = new Vector3(spawnTransform.position.x * (camera.pixelWidth/camera.pixelHeight), spawnTransform.position.y, spawnTransform.position.z);

			switch(randomAstroid)
			{
			case 0:
				mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid1, modifiedPosition, spawnTransform.rotation);
				break;
			case 1:
				mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid2, modifiedPosition, spawnTransform.rotation);
				break;
			case 2:
				mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid3, modifiedPosition, spawnTransform.rotation);
				break;
			case 3:
				mySpawnManager.SpawnObject(mySpawnManager.asteroids.asteroid4, modifiedPosition, spawnTransform.rotation);
				break;
			}
		}
		else
			SpawnAstroid();
	}

	IEnumerator WaitAndUnload()
	{
		yield return new WaitForSeconds(0.1f);
		mySpawnManager.UnspawnObject(clone1);
		mySpawnManager.UnspawnObject(clone2);
		mySpawnManager.UnspawnObject(clone3);
		mySpawnManager.UnspawnObject(clone4);
	}
}
