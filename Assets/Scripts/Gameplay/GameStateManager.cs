using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] GameObject pauseButton;
	[SerializeField] GameObject resumeButton;
	[SerializeField] GameObject summaryFade;
	[SerializeField] GameObject summaryScreen;
	[SerializeField] UILabel summaryLabel;
	[SerializeField] GameObject highScoreLabel;
	[SerializeField] GameObject starsCamera;
	[SerializeField] GameObject[] activeStars;
	[SerializeField] GameObject[] inactiveStars;
	GameCenterManager gameCenterManager;
	GameController gameController;
	GameObject gameControllerObject;
	bool gameControllerFound = false;
	LevelManager levelManager;
	int collectibleSpawners = 0;
	int collectiblesCollected = 0;
	int totalScore = 0;
	public GameObject SummaryFade { get { return summaryFade; }}
	public GameObject ResumeObjects { get { return resumeButton; }}
	public int CollectibleSpawners { get { return collectibleSpawners; } set { collectibleSpawners = value; }}
	public int CollectiblesCollected { get { return collectiblesCollected; } set { collectiblesCollected = value; }}
	public int TotalScore { get { return totalScore; } set { totalScore = value; }}
	
	public enum GameState
	{
		Play,
		Pause,
		Complete,
		Continue,
		Replay,
		MainMenu
	}
	
	public GameState state;
	
	public void SetState()
	{
		switch(state)
		{
		case GameState.Play:
			Play();
			break;
		case GameState.Pause:
			Pause();
			break;
		case GameState.Complete:
			Complete();
			break;
		case GameState.Continue:
			Continue();
			break;
		case GameState.Replay:
			Replay();
			break;
		case GameState.MainMenu:
			MainMenu();
			break;
		}
	}

	void Awake()
	{
		GameObject levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");
		levelManager = levelManagerObject.GetComponent<LevelManager>();
		summaryFade.SetActive(false);
		summaryScreen.SetActive(false);
		starsCamera.SetActive(false);
		highScoreLabel.SetActive(false);
	}

	void Update()
	{
		if (!gameControllerFound)
		{
			if (gameControllerObject == null)
				gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
			else
				GameSetup();
		}
	}

	void GameSetup()
	{
		gameControllerFound = true;
		gameController = gameControllerObject.GetComponent<GameController>();

		GameObject gameCenterManagerObject = GameObject.FindGameObjectWithTag("GameCenterManager");
		gameCenterManager = gameCenterManagerObject.GetComponent<GameCenterManager>();

		SetState();
	}
	
	void Play()
	{
		gameController.GamePaused = false;
		pauseButton.SetActive(true);
		resumeButton.SetActive(false);
	}
	
	void Pause()
	{
		gameController.GamePaused = true;
		pauseButton.SetActive(false);
		resumeButton.SetActive(true);
	}

	void Complete()
	{
		int levelNumber = EncryptedPlayerPrefs.GetInt("Level Number", 0);

		if (levelNumber != 0 && CollectiblesCollected == 3)
			gameCenterManager.SubmitAchievement("collect_three_stars", 100f);

		int highScore = EncryptedPlayerPrefs.GetInt("Level " + levelNumber + " Score", 0);
		EncryptedPlayerPrefs.SetInt("Level " + (levelNumber + 1) + " Status", 1);
		int stageNumber = 0;

		if (levelNumber <= 8)
			stageNumber = 1;
		else if (levelNumber > 8 && levelNumber <= 17)
			stageNumber = 2;
		else if (levelNumber > 17 && levelNumber <= 26)
			stageNumber = 3;
		else if (levelNumber > 26)
			stageNumber = 4;

		if (TotalScore > highScore)
		{
			if (highScore == 0)
			{
				Vector3 tempPosition = summaryLabel.transform.localPosition;
				tempPosition.y = -45f;
				summaryLabel.transform.localPosition = tempPosition;
				EncryptedPlayerPrefs.SetInt("Level " + levelNumber + " Score", TotalScore);
			}
			else
			{
				gameCenterManager.SubmitAchievement("new_high_score", 100f);
				highScoreLabel.SetActive(true);
				EncryptedPlayerPrefs.SetInt("Level " + levelNumber + " Score", TotalScore);
			}

			EncryptedPlayerPrefs.SetInt("Stage " + stageNumber + " Score", EncryptedPlayerPrefs.GetInt("Stage " + stageNumber + " Score", 0) + (TotalScore - highScore));
		}
		else
		{
			Vector3 tempPosition = summaryLabel.transform.localPosition;
			tempPosition.y = -45f;
			summaryLabel.transform.localPosition = tempPosition;
		}

		PlayerPrefs.Save();
		int stage1Score = EncryptedPlayerPrefs.GetInt("Stage 1 Score", 0);
		int stage2Score = EncryptedPlayerPrefs.GetInt("Stage 2 Score", 0);
		int stage3Score = EncryptedPlayerPrefs.GetInt("Stage 3 Score", 0);
		//int stage4Score = EncryptedPlayerPrefs.GetInt("Stage 4 Score", 0);
		int currentStageScore = 0;

		if (TotalScore > highScore)
			currentStageScore = EncryptedPlayerPrefs.GetInt("Stage " + stageNumber + " Score", 0);
		else
			currentStageScore = highScore;

		int overallTotalScore = stage1Score + stage2Score + stage3Score; //+ stage4Score;
		gameCenterManager.SubmitScore(stageNumber, currentStageScore, overallTotalScore);

		int previousEarnedStars = EncryptedPlayerPrefs.GetInt("Level " + levelNumber + " Stars", 0);

		if (CollectiblesCollected > previousEarnedStars)
		{
			EncryptedPlayerPrefs.SetInt("Level " + levelNumber + " Stars", CollectiblesCollected);
			EncryptedPlayerPrefs.SetInt("Stage " + stageNumber + " Stars", EncryptedPlayerPrefs.GetInt("Stage " + stageNumber + " Stars", 0) + (CollectiblesCollected - previousEarnedStars));
			int currentTotalStars = EncryptedPlayerPrefs.GetInt("Total Stars", 0);
			EncryptedPlayerPrefs.SetInt("Total Stars", currentTotalStars + (CollectiblesCollected - previousEarnedStars));
			PlayerPrefs.Save();
		}

		if (stageNumber == 1)
			gameCenterManager.SubmitAchievement("collect_gravity_stars", (EncryptedPlayerPrefs.GetInt("Stage 1 Stars", 0) * 100f) / 27f);
		else if (stageNumber == 2)
			gameCenterManager.SubmitAchievement("collect_antigravity_stars", (EncryptedPlayerPrefs.GetInt("Stage 2 Stars", 0) * 100f) / 27f);
		else if (stageNumber == 3)
			gameCenterManager.SubmitAchievement("collect_wormhole_stars", (EncryptedPlayerPrefs.GetInt("Stage 3 Stars", 0) * 100f) / 27f);

		int totalStars = EncryptedPlayerPrefs.GetInt("Total Stars", 0);

		if (totalStars >= 21 && totalStars < 46)
		{
			gameCenterManager.SubmitAchievement("unlock_anti_gravity_levels", 100f);
		}
		else if (totalStars >= 46 && totalStars < 72)
		{
			gameCenterManager.SubmitAchievement("unlock_anti_gravity_levels", 100f);
			gameCenterManager.SubmitAchievement("unlock_worm_hole_levels", 100f);
		}
		else if (totalStars >= 72)
		{
			gameCenterManager.SubmitAchievement("unlock_anti_gravity_levels", 100f);
			gameCenterManager.SubmitAchievement("unlock_worm_hole_levels", 100f);
			//gameCenterManager.SubmitAchievement("unlock_chaos_theory_levels", 100f);
		}

		//gameCenterManager.SubmitAchievement("collect_all_stars", (totalStars/108) * 100f);
		Collider[] pauseColliders = pauseButton.GetComponentsInChildren<Collider>();

		foreach (Collider pauseCollider in pauseColliders)
			pauseCollider.enabled = false;

		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		summaryFade.SetActive(true);
		summaryScreen.SetActive(true);
		summaryLabel.text = "Score: " + TotalScore.ToString("N0");
		StartCoroutine(EnableStars());
	}

	void Continue()
	{
		levelManager.LoadLevel(EncryptedPlayerPrefs.GetInt("Level Number", 0) + 1);
	}

	void Replay()
	{
		Collider[] pauseColliders = pauseButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider pauseCollider in pauseColliders)
			pauseCollider.enabled = false;

		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();

		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		levelManager.LoadLevel(EncryptedPlayerPrefs.GetInt("Level Number", 0));
	}

	void MainMenu()
	{
		if (Advertisement.isReady())
		{
			// Show with default zone, pause engine and print result to debug log
			Advertisement.Show(null, new ShowOptions {
				pause = true,
				resultCallback = result => {
					Debug.Log(result.ToString());
				}
			});
		}

		Collider[] pauseColliders = pauseButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider pauseCollider in pauseColliders)
			pauseCollider.enabled = false;

		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		levelManager.LoadLevel(99);
	}

	public void DelaySetState(float waitTime)
	{
		StartCoroutine(WaitAndSetState(waitTime));
	}

	IEnumerator WaitAndSetState(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SetState();
	}

	IEnumerator EnableStars()
	{
		float nextStepTime = 0.25f;
		starsCamera.SetActive(true);
		yield return new WaitForSeconds(1.0f);

		switch(CollectiblesCollected)
		{
		case 0:
			inactiveStars[0].SetActive(true);
			yield return new WaitForSeconds(nextStepTime);
			inactiveStars[1].SetActive(true);
			yield return new WaitForSeconds(nextStepTime);
			inactiveStars[2].SetActive(true);
			break;
		case 1:
			activeStars[0].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			yield return new WaitForSeconds(nextStepTime);
			inactiveStars[1].SetActive(true);
			yield return new WaitForSeconds(nextStepTime);
			inactiveStars[2].SetActive(true);
			break;
		case 2:
			activeStars[0].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			yield return new WaitForSeconds(nextStepTime);
			activeStars[1].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			yield return new WaitForSeconds(nextStepTime);
			inactiveStars[2].SetActive(true);
			break;
		case 3:
			activeStars[0].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			yield return new WaitForSeconds(nextStepTime);
			activeStars[1].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			yield return new WaitForSeconds(nextStepTime);
			activeStars[2].SetActive(true);
			Fabric.EventManager.Instance.PostEvent("SFX_Collected", Fabric.EventAction.PlaySound);
			break;
		default:
			Debug.LogError("Unsupported star count: " + CollectiblesCollected);
			break;
		}
	}

	public void DisableStars()
	{
		foreach (GameObject activeStar in activeStars)
		{
			ParticleSystem[] activeStarParticles = activeStar.GetComponentsInChildren<ParticleSystem>();

			foreach (ParticleSystem activeStarParticle in activeStarParticles)
				activeStarParticle.enableEmission = false;
		}

		foreach (GameObject inactiveStar in inactiveStars)
		{
			ParticleSystem[] inactiveStarParticles = inactiveStar.GetComponentsInChildren<ParticleSystem>();
			
			foreach (ParticleSystem inactiveStarParticle in inactiveStarParticles)
				inactiveStarParticle.enableEmission = false;
		}
	}
}
