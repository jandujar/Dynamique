using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] float initialWait = 0f;
	[SerializeField] float additionalObjectSpawnWait = 0.5f;
	[SerializeField] CollectibleSpawner[] collectibleSpawners;
	[SerializeField] GameObject summaryFade;
	[SerializeField] GameObject summaryScreen;
	[SerializeField] UILabel summaryLabel;
	Spawner spawner;
	float collectibleSpawnWait = 0.1f;
	float elapsedTime = 0f;
	bool startTimer = false;
	int timeScore;
	int starScore;
	int totalScore;
	bool summaryTriggered = false;
	bool levelComplete = false;
	int collectiblesCollected = 0;
	public bool LevelComplete { get { return levelComplete;} set { levelComplete = value; }}
	public int CollectiblesCollected { get { return collectiblesCollected;} set { collectiblesCollected = value; }}

	void Start()
	{
		StartCoroutine(SpawnCollectibles(initialWait));
		var spawnerObject = GameObject.FindGameObjectWithTag("Spawner");

		if (spawnerObject != null)
		{
			spawner = spawnerObject.GetComponent<Spawner>();
			spawner.TriggerSpawn(initialWait + additionalObjectSpawnWait);
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

		StartCoroutine(SpawnCollectibles(0f));
	}

	IEnumerator SpawnCollectibles(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		foreach (CollectibleSpawner collectibleSpawner in collectibleSpawners)
		{
			yield return new WaitForSeconds(collectibleSpawnWait);
			collectibleSpawner.Spawn();
		}
	}

	IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(initialWait + additionalObjectSpawnWait);
		startTimer = true;
	}

	void LevelCompleted()
	{
		summaryFade.SetActive(true);
		summaryScreen.SetActive(true);

		if (elapsedTime < 60f)
			timeScore = Mathf.RoundToInt(6000f - (100f * elapsedTime));
		else
			timeScore = 0;

		starScore = 1000 * CollectiblesCollected;

		totalScore = timeScore + starScore;
		summaryLabel.text = "Stars Collected: " + CollectiblesCollected +
			"\nScore: " + totalScore.ToString("N0");
	}
}
