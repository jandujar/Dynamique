﻿using UnityEngine;
using System.Collections;

public class StopAudioOnClick : MonoBehaviour
{
	[SerializeField] string eventName;
	
	void OnClick()
	{
		Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.StopSound);
	}
}
