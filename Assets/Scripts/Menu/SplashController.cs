using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class SplashController : MonoBehaviour
{
	const float loadWaitTime = 7f;
	GameObject audioListener;

	void Awake()
	{
		if (Advertisement.isSupported)
		{
			Advertisement.allowPrecache = true;
			Advertisement.Initialize("17687");
		}
		else
		{
			Debug.Log("Platform not supported");
		}
	}

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");

		EncryptedPlayerPrefs.SetInt("Load Main Menu", 1);
		EncryptedPlayerPrefs.SetInt("Level 0 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 9 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 18 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 27 Status", 1);

		Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound, audioListener);
		Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound, audioListener);

		PlayerPrefs.Save();
		StartCoroutine(WaitAndLoad());
	}
	
	IEnumerator WaitAndLoad()
	{
		yield return new WaitForSeconds(loadWaitTime);
		Application.LoadLevel(1);
	}
}
