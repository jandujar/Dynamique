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
			SFXOn();
		else
			SFXOff();
	}

	public void ButtonPress()
	{
		SFXStatus = EncryptedPlayerPrefs.GetInt("SFX Enabled", 1);

		if (SFXStatus == 0)
		{
			EncryptedPlayerPrefs.SetInt("SFX Enabled", 1);
			SFXOn();
		}
		else
		{
			EncryptedPlayerPrefs.SetInt("SFX Enabled", 0);
			SFXOff();
		}

		PlayerPrefs.Save();
	}

	void SFXOn()
	{
		if (buttonText != null)
			buttonText.text = "SFX: On";

		Fabric.EventManager.Instance.PostEvent("SFX", Fabric.EventAction.SetVolume, 0.2f);
	}

	void SFXOff()
	{
		if (buttonText != null)
			buttonText.text = "SFX: Off";

		Fabric.EventManager.Instance.PostEvent("SFX", Fabric.EventAction.SetVolume, 0f);
	}
}
