using UnityEngine;
using System.Collections;

public class StartAudioOnClick : MonoBehaviour
{
	[SerializeField] string eventName;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
	}

	void OnClick()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound, audioListener);
	}
}
