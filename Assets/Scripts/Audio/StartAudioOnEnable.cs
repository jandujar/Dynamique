using UnityEngine;
using System.Collections;

public class StartAudioOnEnable : MonoBehaviour
{
	[SerializeField] string eventName;
	
	void OnEnable()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound);
	}
}
