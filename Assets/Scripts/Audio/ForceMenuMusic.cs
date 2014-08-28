using UnityEngine;
using System.Collections;

public class ForceMenuMusic : MonoBehaviour
{

	void OnEnable()
	{
		Fabric.EventManager.Instance.PostEvent("Music_Gameplay", Fabric.EventAction.StopSound);
		Fabric.EventManager.Instance.PostEvent("Music_Menu", Fabric.EventAction.PlaySound);
	}
}
