using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
	const float loadWaitTime = 7f;

	void Start()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt("Load Main Menu", 1);
		PlayerPrefs.Save();
		StartCoroutine(WaitAndLoad());
	}
	
	IEnumerator WaitAndLoad()
	{
		yield return new WaitForSeconds(loadWaitTime);
		Application.LoadLevel(1);
	}
}
