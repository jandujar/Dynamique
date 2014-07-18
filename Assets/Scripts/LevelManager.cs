using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject gameStartUI;
	[SerializeField] bool loadSpecificLevel = false;
	[SerializeField] int specificLevelNumber = 0;
	[SerializeField] GameObject[] levels;

	void Start()
	{
		int startButtonPressed = PlayerPrefs.GetInt("Start Button Pressed", 0);
		int levelToLoad = PlayerPrefs.GetInt("Level Number", 0);
		specificLevelNumber -= 1;

		if (startButtonPressed == 0 && levelToLoad == 0)
			gameStartUI.SetActive(true);
		else if (loadSpecificLevel)
			Instantiate(levels[specificLevelNumber], transform.position, transform.rotation);
		else
			Instantiate(levels[PlayerPrefs.GetInt("Level Number", 0)], transform.position, transform.rotation);
	}

	public void LoadLevel(int levelNumber)
	{
		PlayerPrefs.SetInt("Level Number", levelNumber);
		Application.LoadLevel(1);
	}
}
