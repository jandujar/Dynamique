using UnityEngine;
using System.Collections;

public class SFXToggle : MonoBehaviour
{
	[SerializeField] UILabel buttonText;
	int SFXStatus;

	void Start()
	{
		SFXStatus = EncryptedPlayerPrefs.GetInt("SFX Enabled", 1);

		if (SFXStatus == 1)
			MusicOn();
		else
			MusicOff();
	}

	public void ButtonPress()
	{
		SFXStatus = EncryptedPlayerPrefs.GetInt("SFX Enabled", 1);

		if (SFXStatus == 0)
		{
			EncryptedPlayerPrefs.SetInt("SFX Enabled", 1);
			MusicOn();
		}
		else
		{
			EncryptedPlayerPrefs.SetInt("SFX Enabled", 0);
			MusicOff();
		}

		PlayerPrefs.Save();
	}

	void MusicOn()
	{
		if (buttonText != null)
			buttonText.text = "SFX: On";

		Fabric.EventManager.Instance.PostEvent("SFX", Fabric.EventAction.SetVolume, 1.0f);
	}

	void MusicOff()
	{
		if (buttonText != null)
			buttonText.text = "SFX: Off";

		Fabric.EventManager.Instance.PostEvent("SFX", Fabric.EventAction.SetVolume, 0f);
	}
}
