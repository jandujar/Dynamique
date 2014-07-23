﻿using UnityEngine;
using System.Collections;

public class TriggerLevelLoad : MonoBehaviour
{
	[SerializeField] LevelManager levelManager;
	[SerializeField] TweenAlpha backgroundFade;
	[SerializeField] int levelToLoad = 0;
	[SerializeField] UILabel buttonText;

	void Awake()
	{
		PlayerPrefs.Save();
		buttonText.text = "Level " + levelToLoad.ToString();

		var summaryFade = GameObject.FindGameObjectWithTag("SummaryFade");
		var summaryScreen = GameObject.FindGameObjectWithTag("SummaryScreen");

		if (summaryFade != null)
			summaryFade.SetActive(false);

		if (summaryScreen != null)
			summaryScreen.SetActive(false);
	}

	void OnClick()
	{
		backgroundFade.from = backgroundFade.value;
		backgroundFade.to = 1f;
		backgroundFade.duration = 1f;
		backgroundFade.delay = 0f;
		backgroundFade.ResetToBeginning();
		backgroundFade.PlayForward();

		StartCoroutine(WaitAndTrigger());
	}

	IEnumerator WaitAndTrigger()
	{
		yield return new WaitForSeconds(1.0f);

		if (levelToLoad > 0)
		{
			PlayerPrefs.SetInt("Load Main Menu", 0);
			levelManager.LoadLevel(levelToLoad - 1);
		}
		else
		{
			PlayerPrefs.SetInt("Load Main Menu", 1);
			Debug.LogError("Loading Level 0 or less....");
			levelManager.LoadLevel(0);
		}
	}
}
