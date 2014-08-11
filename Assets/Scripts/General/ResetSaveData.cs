using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

public class ResetSaveData : MonoBehaviour
{
	[SerializeField] bool resetData = false;

	void Awake()
	{
		if (resetData)
		{
			//Reset game save data
			PlayerPrefs.DeleteAll();

			//Set first level active
			EncryptedPlayerPrefs.SetInt("Level 0 Status", 1);

			Debug.Log("Save Data Reset");
		}
	}
}
