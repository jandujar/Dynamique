using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] float initialSpawnWait = 0f;
	[SerializeField] CollectibleSpawner[] collectibleSpawners;
	Spawner spawner;
	float collectibleSpawnWait = 0.1f;
	float elapsedTime = 0f;
	bool startTimer = false;
	bool summaryTriggered = false;
	bool levelComplete = false;
	int collectiblesCollected = 0;
	public bool LevelComplete { get { return levelComplete;} set { levelComplete = value; }}
	public int CollectiblesCollected { get { return collectiblesCollected;} set { collectiblesCollected = value; }}

	void Start()
	{
		StartCoroutine(SpawnCollectibles());
		var spawnerObject = GameObject.FindGameObjectWithTag("Spawner");

		if (spawnerObject != null)
		{
			spawner = spawnerObject.GetComponent<Spawner>();
			spawner.TriggerSpawn(initialSpawnWait);
			StartCoroutine(StartTimer());
		}
		else
		{
			Debug.LogError("No Spawner Found");
		}
	}

	void Update()
	{
		if (startTimer)
		{
			if (!LevelComplete)
				elapsedTime += Time.deltaTime;
			else if (!summaryTriggered)
			{
				LevelCompleted();
				summaryTriggered = true;
			}
		}
	}

	public void ResetCollectibles()
	{
		CollectiblesCollected = 0;
		GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");

		foreach (GameObject collectible in collectibles)
			Destroy(collectible);

		StartCoroutine(SpawnCollectibles());
	}

	IEnumerator SpawnCollectibles()
	{
		foreach (CollectibleSpawner collectibleSpawner in collectibleSpawners)
		{
			yield return new WaitForSeconds(collectibleSpawnWait);
			collectibleSpawner.Spawn();
		}
	}

	void LevelCompleted()
	{
		Debug.Log("Level Completed, Time: " + elapsedTime.ToString("F2"));
		Debug.Log("Collected " + CollectiblesCollected + " Collectibles");
	}

	IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(initialSpawnWait);
		startTimer = true;
	}
}
