using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] int nextLevel = 0;
	[SerializeField] float initialWait = 0f;
	[SerializeField] float additionalObjectSpawnWait = 0.5f;
	[SerializeField] CollectibleSpawner[] collectibleSpawners;
	LevelManager levelManager;
	GameObject summaryFade;
	GameObject summaryScreen;
	UILabel summaryStats;
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

	void Awake()
	{
		GameObject levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");
		levelManager = levelManagerObject.GetComponent<LevelManager>();

		summaryFade = GameObject.FindGameObjectWithTag("SummaryFade");
		summaryScreen = GameObject.FindGameObjectWithTag("SummaryScreen");
		summaryFade.SetActive(false);
		summaryScreen.SetActive(false);
	}

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

		GameObject summaryStatsObject = GameObject.FindGameObjectWithTag("SummaryStats");
		summaryStats = summaryStatsObject.GetComponent<UILabel>();

		if (elapsedTime < 60f)
			timeScore = Mathf.RoundToInt(6000f - (100f * elapsedTime));
		else
			timeScore = 0;

		starScore = 1000 * CollectiblesCollected;

		totalScore = timeScore + starScore;

		if (collectibleSpawners.Length > 0)
		{
			summaryStats.text = "Stars Collected: " + CollectiblesCollected +
			"\nScore: " + totalScore.ToString("N0");
		}
		else
		{
			summaryStats.text = "Score: " + totalScore.ToString("N0");
		}

		StartCoroutine(NextLevel());
	}

	IEnumerator NextLevel()
	{
		yield return new WaitForSeconds(7.0f);
		levelManager.LoadLevel(nextLevel);
	}
}
