using UnityEngine;
using System.Collections;

public class MusicManagerSpawner : MonoBehaviour
{
	[SerializeField] MusicManager musicManager;
	
	void Awake()
	{
		GameObject gameCenterManagerObject = GameObject.FindGameObjectWithTag("MusicManager");
		
		if (gameCenterManagerObject == null)
			Instantiate(musicManager, transform.position, transform.rotation);
	}
}
