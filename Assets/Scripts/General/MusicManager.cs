using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	
	void Start ()
	{
		int loadMenu = EncryptedPlayerPrefs.GetInt("Load Main Menu", 1);
		int menuMusicPlaying = EncryptedPlayerPrefs.GetInt("Menu Music Playing", 0);
		int gameplayMusicPlaying = EncryptedPlayerPrefs.GetInt("Gameplay Music Playing", 0);

		if (Application.loadedLevel == 0 || (loadMenu == 1 && menuMusicPlaying == 0))
		{
			CancelInvoke("PlayGameplayMusic");
			InvokeRepeating("PlayMenuMusic", 0f, 60f);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound);
			EncryptedPlayerPrefs.SetInt("Menu Music Playing", 1);
			EncryptedPlayerPrefs.SetInt("Gameplay Music Playing", 0);
		}
		else if (loadMenu == 0 && gameplayMusicPlaying == 0)
		{
			CancelInvoke("PlayMenuMusic");
			InvokeRepeating("PlayGameplayMusic", 0f, 60f);
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.StopSound);
			EncryptedPlayerPrefs.SetInt("Menu Music Playing", 0);
			EncryptedPlayerPrefs.SetInt("Gameplay Music Playing", 1);
		}

		PlayerPrefs.Save();
	}

	void PlayMenuMusic()
	{
		Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound);
	}

	void PlayGameplayMusic()
	{
		Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.PlaySound);
	}
}
