﻿using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
	[SerializeField] GameObject collectExplosion;
	[SerializeField] GameObject[] livingEffects;
	GameController gameController;

	void Start()
	{
		var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Object")
		{
			gameController.CollectiblesCollected += 1;
			gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);
			Instantiate(collectExplosion, transform.position, transform.rotation);
			collider.enabled = false;
			
			foreach(GameObject livingEffect in livingEffects)
				Destroy(livingEffect);
			
			Destroy(transform.gameObject, 2f);
		}
	}
}