using UnityEngine;
using System.Collections;

public class SFXButton : MonoBehaviour
{
	[SerializeField] SFXToggle sfxToggle;

	void OnClick()
	{
		sfxToggle.ButtonPress();
	}
}
