using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	[SerializeField] GameObject fabricPrefab;
	bool firstload = true;
	bool menuMusicPlaying = false;
	bool gameplayMusicPlaying = false;
	GameObject audioListener;

	void Awake()
	{
		GameObject fabricObject = GameObject.FindGameObjectWithTag("Fabric");

		if (fabricObject == null)
			Instantiate(fabricPrefab);

		if (firstload)
		{
			firstload = false;
			menuMusicPlaying = true;
			gameplayMusicPlaying = false;
			DontDestroyOnLoad(transform.gameObject);
		}
	}

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
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
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound, audioListener);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound, audioListener);
			menuMusicPlaying = true;
			gameplayMusicPlaying = false;
		}
		else if (loadMenu == 0 && !gameplayMusicPlaying)
		{
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.StopSound, audioListener);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.PlaySound, audioListener);
			menuMusicPlaying = false;
			gameplayMusicPlaying = true;
		}
	}

	void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PauseSound, audioListener);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.PauseSound, audioListener);
		}
		else
		{
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.UnpauseSound, audioListener);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.UnpauseSound, audioListener);
		}
	}
}
