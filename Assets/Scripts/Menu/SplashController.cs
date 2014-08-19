using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
	const float loadWaitTime = 7f;

	void Start()
	{
		EncryptedPlayerPrefs.SetInt("Load Main Menu", 1);
		EncryptedPlayerPrefs.SetInt("Level 0 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 9 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 18 Status", 1);
		EncryptedPlayerPrefs.SetInt("Level 27 Status", 1);

		PlayerPrefs.Save();
		StartCoroutine(WaitAndLoad());
	}
	
	IEnumerator WaitAndLoad()
	{
		yield return new WaitForSeconds(loadWaitTime);
		Application.LoadLevel(1);
	}
}
