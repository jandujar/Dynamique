using UnityEngine;
using System.Collections;

public class TriggerLevelLoad : MonoBehaviour
{
	[SerializeField] LevelManager levelManager;
	[SerializeField] GameObject mainMenu;
	[SerializeField] int levelToLoad = 0;
	[SerializeField] UILabel buttonText;

	void Awake()
	{
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
		PlayerPrefs.SetInt("Load Main Menu", 0);
		PlayerPrefs.Save();
		levelManager.LoadLevel(levelToLoad - 1);
		mainMenu.SetActive(false);
	}
}
