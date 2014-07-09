using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] bool loadSpecificLevel = false;
	[SerializeField] int specificLevelNumber = 0;
	[SerializeField] GameObject[] levels;

	void Awake()
	{
		if (loadSpecificLevel)
			Instantiate(levels[specificLevelNumber], transform.position, transform.rotation);
		else
			Instantiate(levels[PlayerPrefs.GetInt("Level Number", 0)], transform.position, transform.rotation);
	}

	public void LoadLevel(int levelNumber)
	{
		PlayerPrefs.SetInt("Level Number", levelNumber);
		Application.LoadLevel(0);
	}
}
