using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour
{
	[SerializeField] MusicToggle musicToggle;

	void OnClick()
	{
		musicToggle.ButtonPress();
	}
}
