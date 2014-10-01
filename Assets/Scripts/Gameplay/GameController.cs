using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] float initialWait = 0f;
	[SerializeField] float additionalObjectSpawnWait = 0.5f;
	[SerializeField] CollectibleSpawner[] collectibleSpawners;
	int shootingStarCount = 0;
	int shootingStarsCollected = 0;
	GameObject[] spawnerObjects;
	GameStateManager gameStateManager;
	bool gamePaused = false;
	GameObject summaryFade;
	GameObject summaryScreen;
	UILabel summaryStats;
	Spawner spawner;
	GameObject tools;
	float collectibleSpawnWait = 0.1f;
	float elapsedTime = 0f;
	bool startTimer = false;
	int timeScore;
	int starScore;
	int totalScore;
	bool summaryTriggered = false;
	bool levelComplete = false;
	int collectiblesCollected = 0;
	bool dead = false;
	bool canSpawn = true;
	public int ShootingStarCount { get { return shootingStarCount; }}
	public int ShootingStarsCollected { get { return shootingStarsCollected; } set { shootingStarsCollected = value; }}
	public bool GamePaused { get { return gamePaused; } set { gamePaused = value; }}
	public bool LevelComplete { get { return levelComplete; } set { levelComplete = value; }}
	public int CollectiblesCollected { get { return collectiblesCollected; } set { collectiblesCollected = value; }}
	public bool Dead { get { return dead; } set { dead = value; }}

	void Start()
	{
		GameObject gameStateManagerObject = GameObject.FindGameObjectWithTag("GameStateManager");
		
		if (gameStateManagerObject != null)
			gameStateManager = gameStateManagerObject.GetComponent<GameStateManager>();
		else
			Debug.LogError("Couldn't Find Game State Manager");

		CollectiblesCollected = 0;
		gameStateManager.CollectiblesCollected = 0;
		StartCoroutine(SpawnCollectibles(initialWait));
		tools = GameObject.FindGameObjectWithTag("Tools");
		spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");
		shootingStarCount = spawnerObjects.Length;

		if (spawnerObjects.Length > 0)
		{	
			foreach (GameObject spawnerObject in spawnerObjects)
			{
				spawner = spawnerObject.GetComponent<Spawner>();
				spawner.TriggerSpawn(initialWait + additionalObjectSpawnWait);
				StartCoroutine(StartTimer());
			}
		}
		else
		{
			Debug.LogError("No Spawners Found");
		}
	}

	void Update()
	{
		if (startTimer)
		{
			if (!levelComplete)
				elapsedTime += Time.deltaTime;
			else if (!summaryTriggered)
			{
				LevelCompleted();
				summaryTriggered = true;
			}
		}
	}

	public void SpawnShootingStars(float respawnWait)
	{
		if (spawnerObjects.Length > 0)
		{	
			foreach (GameObject spawnerObject in spawnerObjects)
			{
				spawner = spawnerObject.GetComponent<Spawner>();
				spawner.TriggerSpawn(respawnWait);
				StartCoroutine(StartTimer());
			}
		}
	}

	public void KillShootingStars()
	{
		Dead = true;
		ResetCollectibles();
		ShootingStarsCollected = 0;
		tools.BroadcastMessage("ResetTools");
		GameObject[] shootingStarObjects = GameObject.FindGameObjectsWithTag("Object");

		foreach (GameObject shootingStarObject in shootingStarObjects)
		{
			SpawnedObject spawnedObject = shootingStarObject.GetComponent<SpawnedObject>();
			spawnedObject.Death();
		}

		Dead = false;
	}

	public void ResetCollectibles()
	{
		CollectiblesCollected = 0;
		gameStateManager.CollectiblesCollected = 0;
		GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");

		foreach (GameObject collectible in collectibles)
		{
			Collectible collectibleScript = collectible.GetComponent<Collectible>();
			collectibleScript.DestroyStar();
		}

		if (canSpawn)
			StartCoroutine(SpawnCollectibles(0f));
	}

	IEnumerator SpawnCollectibles(float waitTime)
	{
		canSpawn = false;
		yield return new WaitForSeconds(waitTime);

		foreach (CollectibleSpawner collectibleSpawner in collectibleSpawners)
		{
			yield return new WaitForSeconds(collectibleSpawnWait);
			collectibleSpawner.Spawn();
		}

		canSpawn = true;
	}

	IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(initialWait + additionalObjectSpawnWait);
		startTimer = true;
	}

	void LevelCompleted()
	{
		Debug.Log("Final: " + CollectiblesCollected);

		Fabric.EventManager.Instance.PostEvent("SFX_Vortex", Fabric.EventAction.PlaySound);

		if (elapsedTime < 60f)
			timeScore = Mathf.RoundToInt(6000f - (100f * elapsedTime));
		else
			timeScore = 0;

		if (EncryptedPlayerPrefs.GetInt("Level Number", 0) == 0)
			CollectiblesCollected = 3;

		starScore = 1000 * CollectiblesCollected;
		totalScore = timeScore + starScore;

		gameStateManager.CollectibleSpawners = collectibleSpawners.Length;
		gameStateManager.CollectiblesCollected = CollectiblesCollected;
		gameStateManager.TotalScore = totalScore;
		gameStateManager.state = GameStateManager.GameState.Complete;
		gameStateManager.SetState();
	}
}
