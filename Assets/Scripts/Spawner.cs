using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	int spawnNumber = 1;

	public void TriggerSpawn(float spawnWaitTime)
	{
		StartCoroutine(Spawn(spawnWaitTime));
	}

	IEnumerator Spawn(float spawnWaitTime)
	{
		yield return new WaitForSeconds(spawnWaitTime);
		var clone = (GameObject)Instantiate(objectPrefab, transform.position, transform.rotation);
		clone.name = "Object " + spawnNumber.ToString();
		spawnNumber++;
	}
}
