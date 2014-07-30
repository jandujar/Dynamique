﻿using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] GameObject pauseButton;
	[SerializeField] GameObject resumeButton;
	[SerializeField] GameObject summaryFade;
	[SerializeField] GameObject summaryScreen;
	[SerializeField] UILabel summaryLabel;
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
		SetState();

		GameObject levelManagerObject = GameObject.FindGameObjectWithTag("LevelManager");
		levelManager = levelManagerObject.GetComponent<LevelManager>();
		summaryFade.SetActive(false);
		summaryScreen.SetActive(false);
	}
	
	void Play()
	{
		pauseButton.SetActive(true);
		resumeButton.SetActive(false);
	}
	
	void Pause()
	{
		pauseButton.SetActive(false);
		resumeButton.SetActive(true);
	}

	void Complete()
	{
		pauseButton.transform.collider.enabled = false;
		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		summaryFade.SetActive(true);
		summaryScreen.SetActive(true);

		if (CollectibleSpawners > 0)
		{
			summaryLabel.text = "Stars Collected: " + CollectiblesCollected +
				"\nScore: " + totalScore.ToString("N0");
		}
		else
		{
			summaryLabel.text = "Score: " + totalScore.ToString("N0");
		}
	}

	void Continue()
	{
		levelManager.LoadLevel(EncryptedPlayerPrefs.GetInt("Level Number", 0) + 1);
	}

	void Replay()
	{
		pauseButton.transform.collider.enabled = false;
		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();

		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		levelManager.LoadLevel(EncryptedPlayerPrefs.GetInt("Level Number", 0));
	}

	void MainMenu()
	{
		pauseButton.transform.collider.enabled = false;
		Collider[] resumeColliders = resumeButton.GetComponentsInChildren<Collider>();
		
		foreach (Collider resumeCollider in resumeColliders)
			resumeCollider.enabled = false;

		EncryptedPlayerPrefs.SetInt("Load Main Menu", 1);
		levelManager.LoadLevel(0);
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
}
