using UnityEngine;
using System.Collections;

public class ButtonAudioTrigger : MonoBehaviour
{
	[SerializeField] string eventName;
	
	public void PlayAudio()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound);
	}
}
