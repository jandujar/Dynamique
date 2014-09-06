using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	[SerializeField] GameObject fabricPrefab;
	bool firstload = true;
	bool menuMusicPlaying = false;
	bool gameplayMusicPlaying = false;
	bool fadeMenuMusic;
	bool fadeGameplayMusic;
	float menuMusicVolume;
	float gameplayMusicVolume;

	void Awake()
	{
		GameObject fabricObject = GameObject.FindGameObjectWithTag("Fabric");

		if (fabricObject == null)
			Instantiate(fabricPrefab);
	}

	void Start()
	{
		if (firstload)
		{
			DontDestroyOnLoad(transform.gameObject);
			firstload = false;
			menuMusicPlaying = true;
			gameplayMusicPlaying = false;
			fadeMenuMusic = true;
			menuMusicVolume = 0f;
			gameplayMusicVolume = 0f;
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound);
		}
	}

	void OnLevelWasLoaded()
	{
		ProcessMusic();
	}

	void ProcessMusic()
	{
		int loadMenu = EncryptedPlayerPrefs.GetInt("Load Main Menu", 1);

		if (Application.loadedLevel == 0 || (loadMenu == 1 && !menuMusicPlaying))
		{
			fadeMenuMusic = true;
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound);
			menuMusicPlaying = true;
			gameplayMusicPlaying = false;
		}
		else if (loadMenu == 0 && !gameplayMusicPlaying)
		{
			fadeMenuMusic = false;
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.PlaySound);
			menuMusicPlaying = false;
			gameplayMusicPlaying = true;
		}
	}

	void Update()
	{
		if (fadeMenuMusic)
		{
			if (menuMusicVolume < 1f)
				menuMusicVolume += Time.deltaTime / 1.5f;
			else if (menuMusicVolume > 1f)
				menuMusicVolume = 1f;

			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.SetVolume, menuMusicVolume);

			if (gameplayMusicVolume > 0f)
			{
				gameplayMusicVolume -= Time.deltaTime;
				Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.SetVolume, gameplayMusicVolume);
			}
			else if (gameplayMusicVolume <= 0f)
			{
				gameplayMusicVolume = 0f;
				Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound);
			}
		}
		else
		{
			if (menuMusicVolume > 0f)
			{
				menuMusicVolume -= Time.deltaTime;
				Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.SetVolume, menuMusicVolume);
			}
			else if (menuMusicVolume <= 0f)
			{
				menuMusicVolume = 0f;
				Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.StopSound);
			}
			
			if (gameplayMusicVolume < 0.5f)
				gameplayMusicVolume += Time.deltaTime / 1.5f;
			else if (gameplayMusicVolume > 0.5f)
				gameplayMusicVolume = 0.5f;
			
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.SetVolume, gameplayMusicVolume);
		}
	}
}
