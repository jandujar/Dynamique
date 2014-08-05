using UnityEngine;
using System.Collections;

public class StarsCounter : MonoBehaviour
{
	[SerializeField] UILabel starsCounter;

	void Awake()
	{
		int totalStars = EncryptedPlayerPrefs.GetInt("Total Stars", 0);
		starsCounter.text = totalStars.ToString();
	}
}
