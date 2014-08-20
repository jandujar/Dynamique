using UnityEngine;
using System.Collections;

public class MusicToggle : MonoBehaviour
{
	[SerializeField] UILabel buttonText;
	int musicStatus;

	void Start()
	{
		musicStatus = EncryptedPlayerPrefs.GetInt("Music Enabled", 1);

		if (musicStatus == 1)
			MusicOn();
		else
			MusicOff();
	}

	public void ButtonPress()
	{
		musicStatus = EncryptedPlayerPrefs.GetInt("Music Enabled", 1);

		if (musicStatus == 0)
		{
			EncryptedPlayerPrefs.SetInt("Music Enabled", 1);
			MusicOn();
		}
		else
		{
			EncryptedPlayerPrefs.SetInt("Music Enabled", 0);
			MusicOff();
		}

		PlayerPrefs.Save();
	}

	void MusicOn()
	{
		if (buttonText != null)
			buttonText.text = "Music: On";

		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.SetVolume, 1.0f);
	}

	void MusicOff()
	{
		if (buttonText != null)
			buttonText.text = "Music: Off";

		Fabric.EventManager.Instance.PostEvent("Music", Fabric.EventAction.SetVolume, 0f);
	}
}
