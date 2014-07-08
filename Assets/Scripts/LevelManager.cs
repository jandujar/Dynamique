using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject[] levels;

	void Awake()
	{
		Instantiate(levels[PlayerPrefs.GetInt("Level Number", 0)], transform.position, transform.rotation);
	}

	public void LoadLevel(int levelNumber)
	{
		PlayerPrefs.SetInt("Level Number", levelNumber);
		Application.LoadLevel(0);
	}
}
