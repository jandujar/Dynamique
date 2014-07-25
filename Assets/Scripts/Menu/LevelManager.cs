using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject mainMenu;
	[SerializeField] GameObject inGameHUD;
	[SerializeField] bool loadSpecificLevel = false;
	[SerializeField] int specificLevelNumber = 0;
	[SerializeField] GameObject[] levels;

	void Awake()
	{
		mainMenu.SetActive(false);
		inGameHUD.SetActive(false);
	}

	void Start()
	{
		int loadMainMenu = EncryptedPlayerPrefs.GetInt("Load Main Menu", 1);
		specificLevelNumber -= 1;

		if (loadMainMenu == 1)
			mainMenu.SetActive(true);
		else
		{
			if (loadSpecificLevel)
				Instantiate(levels[specificLevelNumber], transform.position, transform.rotation);
			else
				Instantiate(levels[EncryptedPlayerPrefs.GetInt("Level Number", 0)], transform.position, transform.rotation);

			inGameHUD.SetActive(true);
		}
	}

	public void LoadLevel(int levelNumber)
	{
		if (levelNumber < levels.Length)
		{
			EncryptedPlayerPrefs.SetInt("Level Number", levelNumber);
			PlayerPrefs.Save();
		}
		else
		{
			Debug.Log("Level Index Exceeded");
			EncryptedPlayerPrefs.SetInt("Load Main Menu", 1);
			PlayerPrefs.Save();
		}

		Application.LoadLevel(1);
	}
}
