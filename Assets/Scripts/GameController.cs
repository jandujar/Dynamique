using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] float initialSpawnWait = 0f;
	Spawner spawner;
	float elapsedTime = 0f;
	bool startTimer = false;
	bool summaryTriggered = false;
	bool levelComplete = false;
	public bool LevelComplete { get { return levelComplete;} set { levelComplete = value; }}

	void Start()
	{
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

	void LevelCompleted()
	{
		Debug.Log("Level Complete, Time: " + elapsedTime.ToString("F2"));
	}

	IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(initialSpawnWait);
		startTimer = true;
	}
}
