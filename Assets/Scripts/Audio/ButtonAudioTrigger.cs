using UnityEngine;
using System.Collections;

public class ButtonAudioTrigger : MonoBehaviour
{
	[SerializeField] string eventName;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
	}

	public void PlayAudio()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound, audioListener);
	}
}
