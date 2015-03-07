using UnityEngine;
using System.Collections;

public class StartAudioOnEnable : MonoBehaviour
{
	[SerializeField] string eventName;
	GameObject audioListener;

	void Start()
	{
		audioListener = GameObject.FindGameObjectWithTag("Fabric");
	}

	void OnEnable()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.PlaySound, audioListener);
	}
}
