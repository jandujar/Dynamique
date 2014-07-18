using UnityEngine;
using System.Collections;

public class TriggerLevelLoad : MonoBehaviour
{
	[SerializeField] LevelManager levelManager;
	[SerializeField] GameObject gameStartUI;
	[SerializeField] int levelToLoad = 0;

	void Awake()
	{
		var summaryFade = GameObject.FindGameObjectWithTag("SummaryFade");
		var summaryScreen = GameObject.FindGameObjectWithTag("SummaryScreen");
		summaryFade.SetActive(false);
		summaryScreen.SetActive(false);
	}

	void OnClick()
	{
		PlayerPrefs.SetInt("Start Button Pressed", 1);
		levelManager.LoadLevel(levelToLoad);
		gameStartUI.SetActive(false);
	}
}
