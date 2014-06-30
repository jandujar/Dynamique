using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	[SerializeField] float spawnWait = 0f;
	int spawnNumber = 1;

	void Start()
	{
		StartCoroutine(Spawn(spawnWait));
	}

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
