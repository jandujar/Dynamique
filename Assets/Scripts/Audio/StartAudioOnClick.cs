using UnityEngine;
using System.Collections;

public class StartAudioOnClick : MonoBehaviour
{
	[SerializeField] string eventName;
	
	void OnClick()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound);
	}
}
