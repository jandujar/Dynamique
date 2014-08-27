using UnityEngine;
using System.Collections;

public class StopAudioOnEnable : MonoBehaviour
{
	[SerializeField] string eventName;

	void OnEnable()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.StopSound);
	}
}
