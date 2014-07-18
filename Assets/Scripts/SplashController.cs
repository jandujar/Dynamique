using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
	const float loadWaitTime = 7f;

	void Start()
	{
		PlayerPrefs.SetInt("Start Button Pressed", 0);
		PlayerPrefs.SetInt("Level Number", 0);
		StartCoroutine(WaitAndLoad());
	}
	
	IEnumerator WaitAndLoad()
	{
		yield return new WaitForSeconds(loadWaitTime);
		Application.LoadLevel(1);
	}
}
