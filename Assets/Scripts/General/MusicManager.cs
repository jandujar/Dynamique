using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	[SerializeField] GameObject fabricPrefab;

	void Awake()
	{
		GameObject fabricObject = GameObject.FindGameObjectWithTag("Fabric");

		if (fabricObject == null)
			Instantiate(fabricPrefab);
	}

	void Start ()
	{
		int loadMenu = EncryptedPlayerPrefs.GetInt("Load Main Menu", 1);
		int menuMusicPlaying = EncryptedPlayerPrefs.GetInt("Menu Music Playing", 0);
		int gameplayMusicPlaying = EncryptedPlayerPrefs.GetInt("Gameplay Music Playing", 0);

		if (Application.loadedLevel == 0 || (loadMenu == 1 && menuMusicPlaying == 0))
		{
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound);
			EncryptedPlayerPrefs.SetInt("Menu Music Playing", 1);
			EncryptedPlayerPrefs.SetInt("Gameplay Music Playing", 0);
		}
		else if (loadMenu == 0 && gameplayMusicPlaying == 0)
		{
			Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.StopSound);
			Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.PlaySound);
			EncryptedPlayerPrefs.SetInt("Menu Music Playing", 0);
			EncryptedPlayerPrefs.SetInt("Gameplay Music Playing", 1);
		}

		PlayerPrefs.Save();
	}
}
