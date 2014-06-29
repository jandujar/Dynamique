using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	[SerializeField] float initialWait = 0f;
	[SerializeField] float spawnRate = 1f;
	int spawnNumber = 1;

	void Start()
	{
		InvokeRepeating("Spawn", initialWait, spawnRate);
	}

	void Spawn()
	{
		var clone = (GameObject)Instantiate(objectPrefab, transform.position, transform.rotation);
		clone.name = "Object " + spawnNumber.ToString();
		spawnNumber++;
	}
}
