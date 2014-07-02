using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	int spawnNumber = 1;
	bool canSpawn = true;

	public void TriggerSpawn(float spawnWaitTime)
	{
		if (canSpawn)
			StartCoroutine(Spawn(spawnWaitTime));
	}

	IEnumerator Spawn(float spawnWaitTime)
	{
		canSpawn = false;
		yield return new WaitForSeconds(spawnWaitTime);
		var clone = (GameObject)Instantiate(objectPrefab, transform.position, transform.rotation);
		clone.name = "Object " + spawnNumber.ToString();
		spawnNumber++;
		canSpawn = true;
	}
}
