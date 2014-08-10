using UnityEngine;
using System.Collections;

public class GameCenterManagerSpawner : MonoBehaviour
{
	[SerializeField] GameCenterManager gameCenterManager;

	void Awake()
	{
		GameObject gameCenterManagerObject = GameObject.FindGameObjectWithTag("GameCenterManager");

		if (gameCenterManagerObject == null)
			Instantiate(gameCenterManager, transform.position, transform.rotation);
	}
}
